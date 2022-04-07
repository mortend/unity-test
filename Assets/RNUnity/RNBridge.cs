using System.Runtime.InteropServices;
using System;
using Newtonsoft.Json;
using UnityEngine;

namespace RNUnity
{
    public class RNPromise
    {
        public object handle;

        public void Reject<TReason>(TReason reason)
        {
            RNBridge.EmitEvent("reject", new {
                handle = this.handle,
                reason = reason
            });
        }

        public void Resolve<TRetval>(TRetval retval)
        {
            RNBridge.EmitEvent("resolve", new {
                handle = this.handle,
                retval = retval
            });
        }

        public void Resolve()
        {
            RNBridge.EmitEvent("resolve", new {
                handle = this.handle
            });
        }
    }

    public class RNPromise<T> : RNPromise
    {
        public T input;
    }

    public static class RNBridge
    {
        public static RNPromise Begin(object param)
        {
            return Begin<object>(param);
        }

        public static RNPromise<T> Begin<T>(object param)
        {
            if (Application.isEditor)
                return new RNPromise<T>();

            if (Debug.isDebugBuild)
                Debug.Log($"{nameof(RNBridge)}: begin");

            try
            {
                return JsonConvert.DeserializeObject<RNPromise<T>>((string) param);
            }
            catch (Exception e)
            {
                Debug.LogError($"{nameof(RNBridge)}: {e.Message}");
                return new RNPromise<T>();
            }
        }

        public static void SendMessage(object data)
        {
            EmitEvent("message", data);
        }

        internal static void EmitEvent(string name, object data)
        {
            if (Application.isEditor)
                return;

            if (Debug.isDebugBuild)
                Debug.Log($"{nameof(RNBridge)}: event <{name}>");

            try
            {
                _rn.EmitEvent(name, JsonConvert.SerializeObject(data));
            }
            catch (Exception e)
            {
                Debug.LogError($"{nameof(RNBridge)}: {e.Message}");
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Initialize()
        {
            if (Application.isEditor)
                return;

            if (Debug.isDebugBuild)
                Debug.Log($"{nameof(RNBridge)}: initialize");

            try
            {
                if (Application.platform == RuntimePlatform.Android)
                    _rn = new AndroidRN();
                else if (Application.platform == RuntimePlatform.IPhonePlayer)
                    _rn = new NativeRN();
            }
            catch (Exception e)
            {
                Debug.LogError($"{nameof(RNBridge)}: {e.Message}");
            }
        }

        static IRN _rn;

        interface IRN
        {
            void EmitEvent(string name, string json);
        }

        class NativeRN : IRN
        {
            void IRN.EmitEvent(string name, string json)
            {
                RNUProxyEmitEvent(name, json);
            }

            [DllImport("__Internal")]
            static extern void RNUProxyEmitEvent(string name, string json);
        }

        class AndroidRN : IRN
        {
            readonly AndroidJavaObject _jobj;

            public AndroidRN()
            {
                AndroidJavaClass jc = new AndroidJavaClass("com.azesmway.rnunity.RNUnityModule");
                _jobj = jc.CallStatic<AndroidJavaObject>("getInstance");
            }

            void IRN.EmitEvent(string name, string json)
            {
                _jobj.Call("emitEvent", name, json);
            }
        }
    }
}
