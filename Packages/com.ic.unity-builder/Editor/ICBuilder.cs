using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using System.IO;

namespace InternetComputer
{
    public class ICBuilder : IPostprocessBuildWithReport
    {
        internal const string kWebGLBuildDirectory = "Build";
        internal const string kWebGLTemplateDataDirectory = "TemplateData";
        internal const string kWebGLIndexFile = "index.html";

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

        // Convert the Unity WebGL build content into a dfx project under "ic-builder" folder.
        private void MoveFiles(BuildReport report, ICSettings icSettings)
        {
            if (string.IsNullOrEmpty(icSettings.m_CanisterName))
            {
                Debug.LogError("Canister name is invalid.");
                return;
            }

            // Prepare an empty path for ic builder, along with the subdirectories for canister assets and src.
            var icBuilderPath = Path.Combine(report.summary.outputPath, "ic-builder");
            var canisterPath = Path.Combine(icBuilderPath, "src", icSettings.m_CanisterName);
            var canisterAssetsPath = Path.Combine(canisterPath, "assets");
            var canisterSrcPath = Path.Combine(canisterPath, "src");
            if (Directory.Exists(icBuilderPath))
                Directory.Delete(icBuilderPath, true);

            Directory.CreateDirectory(icBuilderPath);
            Directory.CreateDirectory(canisterPath);
            Directory.CreateDirectory(canisterAssetsPath);
            Directory.CreateDirectory(canisterSrcPath);

            // Start to copy files to ic-builder path.
            var webglBuildPath = Path.Combine(report.summary.outputPath, kWebGLBuildDirectory);
            if (Directory.Exists(webglBuildPath))
            {
                FileUtil.CopyFileOrDirectory(webglBuildPath, Path.Combine(canisterAssetsPath, kWebGLBuildDirectory));
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

            File.WriteAllText(Path.Combine(report.summary.outputPath, "ic-builder", "dfx.json"), sw.ToString());
        }
    }
}
