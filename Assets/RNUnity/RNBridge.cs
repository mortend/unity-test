using System.Runtime.InteropServices;
using Newtonsoft.Json;
using UnityEngine;

using Newtonsoft.Json.Linq;

namespace RNUnity
{
    public static class RNBridge
    {
        static IRN _rn;

        public static void EmitEvent(string name, string data)
        {
            if (Debug.isDebugBuild)
                Debug.Log($"{nameof(RNBridge)}: try emit event <{name}>");

            try
            {
                _rn.EmitEvent(name, data);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"{nameof(RNBridge)}: exception during try emit event <{e.Message}>");
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Initialize()
        {
            if (Application.isEditor)
                return;

            if (Debug.isDebugBuild)
                Debug.Log($"{nameof(RNBridge)}: try initialize");

            try
            {
                if (Application.platform == RuntimePlatform.Android)
                    _rn = new AndroidRN();
                else if (Application.platform == RuntimePlatform.IPhonePlayer)
                    _rn = new NativeRN();
            }
            catch (System.Exception e)
            {
                Debug.LogError($"{nameof(RNBridge)}: exception during try initialize <{e.Message}>");
            }
        }

        interface IRN
        {
            void EmitEvent(string name, string data);
        }

        class NativeRN : IRN
        {
            void IRN.EmitEvent(string name, string data)
            {
                RNUProxyEmitEvent(name, data);
            }

            [DllImport("__Internal")]
            static extern void RNUProxyEmitEvent(string name, string data);
        }

        class AndroidRN : IRN
        {
            readonly AndroidJavaObject _jobj;

            public AndroidRN()
            {
                AndroidJavaClass jc = new AndroidJavaClass("com.rnunity.UnityBridge");
                _jobj = jc.CallStatic<AndroidJavaObject>("getInstance");
            }

            void IRN.EmitEvent(string name, string data)
            {
                _jobj.Call("emitEvent", name, data);
            }
        }
    }
}
