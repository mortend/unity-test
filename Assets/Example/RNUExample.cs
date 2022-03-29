using System.Collections;
using System.Collections.Generic;
using RNUnity;
using UnityEngine;

public class RNUExample : MonoBehaviour, IRNCommandsReceiver
{
    void Awake()
    {
        RNBridge.RegisterCommandsReceiver(this);
    }

    public void HandleCommand(RNCommand command)
    {
        switch (command.name)
        {
            default:
                Debug.Log(command.name);
                // command.Reject(new {});
                command.Resolve(new {});
                break;
        }
    }
}
