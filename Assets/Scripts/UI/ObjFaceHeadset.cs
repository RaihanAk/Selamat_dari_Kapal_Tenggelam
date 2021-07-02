using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class ObjFaceHeadset : MonoBehaviour
{
    [Tooltip("If this is checked then this obj will be rotated so it always face the headset.")]
    public bool alwaysFaceHeadset = true;

    private Transform headset;

    protected virtual void OnEnable()
    {
        headset = VRTK_DeviceFinder.HeadsetTransform();
    }

    protected virtual void Update()
    {
        if (alwaysFaceHeadset && headset != null)
        {
            transform.LookAt(headset);
        }
        else
            headset = VRTK_DeviceFinder.HeadsetTransform();


    }
}
