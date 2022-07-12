using UnityEngine;
using UnityEditor;

namespace InternetComputer
{
    public class ICBuilderWindow : EditorWindow
    {
        string myString = "IC Builder";

        // Add menu named "Internet Comupter Builder" to the Window menu
        [MenuItem("Window/Internet Comupter Builder", false, 31)]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            ICBuilderWindow window = (ICBuilderWindow)EditorWindow.GetWindow(typeof(ICBuilderWindow));
            window.titleContent = new GUIContent("Internet Computer Builder");
            window.Show();
        }

        void OnGUI()
        {
            GUILayout.Label("Base Settings", EditorStyles.boldLabel);
            myString = EditorGUILayout.TextField("Text Field", myString);

            if (GUILayout.Button("Build", EditorStyles.miniButton))
            {
                Debug.Log("Building to Internet Computer.");
            }
        }
    }
}