using System.Collections;
using System.Collections.Generic;
using System;
using RNUnity;
using UnityEngine;

public class RNUExample : MonoBehaviour
{
    public void foo(object param)
    {
        Debug.Log($"foo: {param}");
        RNBridge.SendMessage(new Account
        {
            Email = "james@example.com",
            Active = true,
            CreatedDate = new DateTime(2013, 1, 20, 0, 0, 0, DateTimeKind.Utc),
            Roles = new List<string>
            {
                "User",
                "Admin"
            }
        });
        RNBridge.SendMessage("hello?");
        var promise = RNBridge.Begin(param);
        promise.Resolve(true);
        promise.Resolve(null);
        promise.Reject(0);
    }
}

public class Account
{
    public string Email { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedDate { get; set; }
    public IList<string> Roles { get; set; }
}
