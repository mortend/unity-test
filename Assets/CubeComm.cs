using System.Collections;
using System.Collections.Generic;
using Azesmway.RNU;
using UnityEngine;

public class CubeComm : MonoBehaviour, IRNCommandsReceiver
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
                // command.Resolve(new {}) || command.Reject(new {})
                break;
        }
    }
}
