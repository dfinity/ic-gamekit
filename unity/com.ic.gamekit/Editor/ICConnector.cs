using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using System.IO;
using System.Collections.Generic;

namespace InternetComputer
{
    // This is a post-process script to generate the ic project from the untiy webgl build.
    public class ICConnector : IPostprocessBuildWithReport
    {
        internal const string kWebGLBuildDirectory = "Build";
        internal const string kWebGLTemplateDataDirectory = "TemplateData";
        internal const string kWebGLIndexFile = "index.html";
        internal const string kICPojectFolder = "ic-project";

        // Set it to a higer number to make sure it gets called after other postprocess scripts.
        public int callbackOrder { get { return 100; } }

        public void OnPostprocessBuild(BuildReport report)
        {
            if (report.summary.platform != BuildTarget.WebGL)
                return;

            var icSettings = ICSettings.GetSettings();
            if (icSettings == null || !icSettings.m_ICBuildEnabled)
                return;

            MoveFiles(report, icSettings);
            GenerateDfxJsonFile(report, icSettings);
        }

        // Convert the Unity WebGL build content into a dfx project under "ic-project" folder.
        private void MoveFiles(BuildReport report, ICSettings icSettings)
        {
            if (string.IsNullOrEmpty(icSettings.m_CanisterName))
            {
                Debug.LogError("Canister name is invalid.");
                return;
            }

            // Prepare an empty path for the ic project, along with the subdirectories for canister assets and src.
            var icProjectPath = Path.Combine(report.summary.outputPath, kICPojectFolder);
            var canisterPath = Path.Combine(icProjectPath, "src", icSettings.m_CanisterName);
            var canisterAssetsPath = Path.Combine(canisterPath, "assets");
            var canisterSrcPath = Path.Combine(canisterPath, "src");
            if (Directory.Exists(icProjectPath))
                Directory.Delete(icProjectPath, true);

            Directory.CreateDirectory(icProjectPath);
            Directory.CreateDirectory(canisterPath);
            Directory.CreateDirectory(canisterAssetsPath);
            Directory.CreateDirectory(canisterSrcPath);

            // Start to copy files to ic-builder path.
            var webglBuildPath = Path.Combine(report.summary.outputPath, kWebGLBuildDirectory);
            if (Directory.Exists(webglBuildPath))
            {
                var outputDirectory = Path.Combine(canisterAssetsPath, kWebGLBuildDirectory);
                FileUtil.CopyFileOrDirectory(webglBuildPath, outputDirectory);
                GenerateContentTypeMappingJsonFile(outputDirectory);
            }

            // Template data exists for Default build.
            var webglTemplateDataPath = Path.Combine(report.summary.outputPath, kWebGLTemplateDataDirectory);
            if (Directory.Exists(webglTemplateDataPath))
            {
                FileUtil.CopyFileOrDirectory(webglTemplateDataPath, Path.Combine(canisterSrcPath, kWebGLTemplateDataDirectory));
            }

            var webglIndexFilePath = Path.Combine(report.summary.outputPath, kWebGLIndexFile);
            if (File.Exists(webglIndexFilePath))
            {
                FileUtil.CopyFileOrDirectory(webglIndexFilePath, Path.Combine(canisterSrcPath, kWebGLIndexFile));
            }
        }

        struct MIMETypeMap
        {
            public string FileExtension;
            public string ContentType;
            public string ContentEncoding;

            public MIMETypeMap(string fileExtension, string contentType, string contentEncoding)
            {
                this.FileExtension = fileExtension;
                this.ContentType = contentType;
                this.ContentEncoding = contentEncoding;
            }
        }

        // Generating the content type mapping json file to fix the below issue
        // https://github.com/dfinity/ic-gamekit/blob/main/unity/README.md#cant-load-the-game-successfully-with-compression-enabled-for-webgl
        private void GenerateContentTypeMappingJsonFile(string outputDirectory)
        {
            // Skip if compression is disabled or decompression fallback is enabled.
            if (PlayerSettings.WebGL.compressionFormat == WebGLCompressionFormat.Disabled || PlayerSettings.WebGL.decompressionFallback)
                return;

            // Set different file extensions and encoding types according to the compression format.
            string fileExtension = string.Empty;
            string encoding = string.Empty;
            if (PlayerSettings.WebGL.compressionFormat == WebGLCompressionFormat.Gzip)
            {
                fileExtension = ".gz";
                encoding = "gzip";
            }
            else if(PlayerSettings.WebGL.compressionFormat == WebGLCompressionFormat.Brotli)
            {
                fileExtension = ".br";
                encoding = "br";
            }

            var mimeTypes = new List<MIMETypeMap>()
            {
                new MIMETypeMap("*.data" + fileExtension,  "application/octet-stream", encoding),
                new MIMETypeMap("*.js" + fileExtension,  "application/javascript", encoding),
                new MIMETypeMap("*.wasm" + fileExtension,  "application/wasm", encoding)
            };

            // Generate json string from the mime type list.
            StringWriter sw = new StringWriter();
            sw.WriteLine("[");
            for (int i = 0; i < mimeTypes.Count; i++)
            {
                var mimeType = mimeTypes[i];
                sw.WriteLine("  {");
                sw.WriteLine("    \"match\": \"" + mimeType.FileExtension + "\",");
                sw.WriteLine("    \"headers\": {");
                sw.WriteLine("      \"Content-Type\": \"" + mimeType.ContentType + "\",");
                sw.WriteLine("      \"Content-Encoding\": \"" + mimeType.ContentEncoding + "\"");
                sw.WriteLine("    }");
                if (i != mimeTypes.Count - 1)
                    sw.WriteLine("  },");
                else
                    sw.WriteLine("  }");                    
            }
            sw.WriteLine("]");

            File.WriteAllText(Path.Combine(outputDirectory, ".ic-assets.json"), sw.ToString());
        }

        // For now, the json support in Unity is bound to Unity serialization system, which has limitations like not supporting Dictionary etc.
        // Also using 3rd-party json libs might cause unexpected problems as we're building a tool package,
        // the 3rd-party libs introduced by us might conflict with the ones added by end-users.
        // For now I simply generate json text without using advanced libs, will find out if there's a better way to do this.
        private void GenerateDfxJsonFile(BuildReport report, ICSettings icSettings)
        {
            var canisterRelatativePath = Path.Combine("src", icSettings.m_CanisterName);

            StringWriter sw = new StringWriter();
            sw.WriteLine("{");
            sw.WriteLine("  \"canisters\": {");
            sw.WriteLine("    \"" + icSettings.m_CanisterName +  "\": {");
            sw.WriteLine("      \"frontend\": {");
            sw.WriteLine("        \"entrypoint\": \"" + Path.Combine(canisterRelatativePath, "src", kWebGLIndexFile).Replace("\\", "/") + "\"");
            sw.WriteLine("      },");
            sw.WriteLine("      \"source\": [");
            sw.WriteLine("        \"" + Path.Combine(canisterRelatativePath, "assets").Replace("\\", "/") + "\",");
            sw.WriteLine("        \"" + Path.Combine(canisterRelatativePath, "src").Replace("\\", "/") + "\"");
            sw.WriteLine("      ],");
            sw.WriteLine("      \"type\": \"assets\"");
            sw.WriteLine("    }");
            sw.WriteLine("  },");

            sw.WriteLine("  \"version\": 1");
            sw.WriteLine("}");

            File.WriteAllText(Path.Combine(report.summary.outputPath, kICPojectFolder, "dfx.json"), sw.ToString());
        }
    }
}
