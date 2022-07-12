using UnityEditor;
using UnityEngine.UIElements;

namespace InternetComputer
{
    internal class ICSettings
    {

    }

    internal class ICSettingsProvider : SettingsProvider
    {
        public const string k_ICSettingsPath = "ProjectSettings/icsettings.asset";

        private ICSettingsProvider(string path, SettingsScope scopes)
            : base(path, scopes)
        {
            Initialize();
        }

        [SettingsProvider]
        static SettingsProvider CreateICSettingsProvider()
        {
            return new ICSettingsProvider("Project/Internet Computer", SettingsScope.Project);
        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            base.OnActivate(searchContext, rootElement);
        }

        public override void OnDeactivate()
        {
            base.OnDeactivate();
        }

        private void Initialize()
        {
            label = "Internet Computer";
        }

        public override void OnGUI(string searchContext)
        {
        }
    }
}