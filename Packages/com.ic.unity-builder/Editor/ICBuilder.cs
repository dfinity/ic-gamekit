using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace InternetComputer
{
    public class ICBuilder : IPostprocessBuildWithReport
    {
        // Set it to a higer number to make sure it gets called after other postprocess scripts.
        public int callbackOrder { get { return 100; } }

        public void OnPostprocessBuild(BuildReport report)
        {
            if (report.summary.platform != BuildTarget.WebGL)
                return;

            var icSettings = ICSettings.GetSettings();
            if (icSettings == null || !icSettings.m_ICBuildEnabled)
                return;

            Debug.Log("IC Builder gets called for Unity WebGL build.");
        }
    }
}