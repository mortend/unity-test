using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine;
using RNUnity;

public class SpinCube : MonoBehaviour
{
    bool _rotate;
    bool _scale;

    public Vector3 RotateAmount;

    // Start is called before the first frame update
    void Start()
    {
        _rotate = true;
        _scale = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_rotate)
            transform.Rotate(RotateAmount);
    }

    void toggleRotate()
    {
        _rotate = !_rotate;
    }

    void toggleScale()
    {
        _scale = !_scale;
        transform.localScale *= _scale ? 2.0f : 0.5f;
    }

    void OnMouseDown()
    {
        RNBridge.SendMessage("click!");
        toggleScale();
    }

    // Call the following methods from your RN app

    void toggleRotateRN(object param)
    {
        toggleRotate();
        RNBridge
            .Begin(param)
            .Resolve(_rotate);
    }

    void getAccountRN(object param)
    {
        RNBridge
            .Begin(param)
            .Resolve(new Account
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
    }

    void failRN(object param)
    {
        RNBridge
            .Begin(param)
            .Reject("This doesn't work");
    }

    void voidRN(object param)
    {
        RNBridge
            .Begin(param)
            .Resolve();
    }
}

public class Account
{
    public string Email { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedDate { get; set; }
    public IList<string> Roles { get; set; }
}
