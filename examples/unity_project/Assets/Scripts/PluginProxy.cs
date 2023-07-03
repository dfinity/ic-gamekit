using UnityEngine;
using EdjCase.ICP.Candid.Utilities;

namespace IC.GameKit
{
    public class PluginProxy : MonoBehaviour
    {
        string testTarget = "https://6x7nu-oaaaa-aaaan-qdaua-cai.ic0.app/";
        public string gameObjectName = "AgentAndPlugin";
        public string methodName = "OnMessageSent";

        TestICPAgent mTestICPAgent = null;

#if UNITY_ANDROID
        private AndroidJavaObject mPlugin = null;
#endif

        public void Start()
        {
            mTestICPAgent = gameObject.GetComponent<TestICPAgent>();

#if UNITY_ANDROID
            var pluginClass = new AndroidJavaClass("com.icgamekit.plugin.ICGameKitPlugin");
            mPlugin = pluginClass.CallStatic<AndroidJavaObject>("initImpl");
#endif
        }

        public void OpenBrowser()
        {
            var target = testTarget + "?sessionkey=" + ByteUtil.ToHexString(mTestICPAgent.TestIdentity.PublicKey.Value);

#if UNITY_ANDROID
            mPlugin.Call("openBrowser", target);
#endif
        }

        public void OnApplicationPause(bool pause)
        {
            // If it's resuming.
            if (!pause)
            {
#if UNITY_ANDROID
                // OnApplicationPause will be called while launching the app, before mPlugin is initialized.
                if (mPlugin == null)
                    return;

                mPlugin.Call("sendMessage", new string[] { gameObjectName, methodName});
#endif
            }
        }
    }
}
