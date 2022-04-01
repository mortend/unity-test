using System.Collections;
using System.Collections.Generic;
using RNUnity;
using UnityEngine;

public class RNUExample : MonoBehaviour
{
    public void foo(object param)
    {
        Debug.Log($"foo: {param}");
        RNBridge.EmitEvent("lol", "{}");
    }
}
