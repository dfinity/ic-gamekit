using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace InternetComputer
{
    internal class ICSettings : ScriptableObject
    {
        public const string k_ICSettingsPath = "Assets/Editor/icsettings.asset";

        public string m_CanisterName;

        public bool m_ICBuildEnabled;

        internal static ICSettings GetSettings(bool create = false)
        {
            var settings = AssetDatabase.LoadAssetAtPath<ICSettings>(k_ICSettingsPath);
            if (create && settings == null)
            {
                settings = ScriptableObject.CreateInstance<ICSettings>();
                settings.m_CanisterName = "unity_webgl_template_assets";
                settings.m_ICBuildEnabled = false;

                if (!Directory.Exists("Assets/Editor"))
                {
                    Directory.CreateDirectory("Assets/Editor");
                }

                AssetDatabase.CreateAsset(settings, k_ICSettingsPath);
                AssetDatabase.SaveAssets();
            }

            return settings;
        }

        internal static SerializedObject GetSerializedSettings()
        {
            return new SerializedObject(GetSettings(true));
        }
    }

    internal class ICSettingsProvider : SettingsProvider
    {
        private SerializedObject m_ICSettings;

        private static GUIContent s_CanisterNameStyle = new GUIContent("Canister Name");
        private static GUIContent s_ICBuildStyle = new GUIContent("Enable IC Build");

        private ICSettingsProvider(string path, SettingsScope scopes)
            : base(path, scopes)
        {
            label = "Internet Computer";
        }

        [SettingsProvider]
        static SettingsProvider CreateICSettingsProvider()
        {
            return new ICSettingsProvider("Project/Internet Computer", SettingsScope.Project);
        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            base.OnActivate(searchContext, rootElement);

            m_ICSettings = ICSettings.GetSerializedSettings();
        }

        public override void OnDeactivate()
        {
            base.OnDeactivate();
        }

        public override void OnGUI(string searchContext)
        {
            const int kPadding = 15;
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(kPadding);
            EditorGUILayout.PropertyField(m_ICSettings.FindProperty("m_CanisterName"), s_CanisterNameStyle);
            GUILayout.Space(kPadding);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(kPadding);
            EditorGUILayout.PropertyField(m_ICSettings.FindProperty("m_ICBuildEnabled"), s_ICBuildStyle);
            GUILayout.Space(kPadding);
            EditorGUILayout.EndHorizontal();

            m_ICSettings.ApplyModifiedProperties();
        }
    }
}
