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

        internal static ICSettings GetOrCreateSettings()
        {
            var settings = AssetDatabase.LoadAssetAtPath<ICSettings>(k_ICSettingsPath);
            if (settings == null)
            {
                settings = ScriptableObject.CreateInstance<ICSettings>();
                settings.m_CanisterName = "unity_webgl_template_assets";

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
            return new SerializedObject(GetOrCreateSettings());
        }
    }

    internal class ICSettingsProvider : SettingsProvider
    {
        private SerializedObject m_ICSettings;

        private static GUIContent s_CanisterNameStyle = new GUIContent("Canister Name");

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

            if (m_ICSettings != null)
            {
            }
        }

        public override void OnGUI(string searchContext)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(15);
            EditorGUILayout.PropertyField(m_ICSettings.FindProperty("m_CanisterName"), s_CanisterNameStyle);
            GUILayout.Space(15);
            EditorGUILayout.EndHorizontal();

            m_ICSettings.ApplyModifiedProperties();
        }
    }
}