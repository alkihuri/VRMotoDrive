using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class MotoViewPortController : MonoBehaviour
{
    [SerializeField] Transform _head;

    [SerializeField] Transform _origin;

    [SerializeField] Transform _target;


    [ContextMenu("recetner")]
    public void Recenter()
    {
        Vector3 offset = _head.position - _origin.position;
        offset.y = 0;
        _origin.position = _target.position - offset;

        Vector3 targetForward = _target.forward;
        targetForward.y = 0;
        Vector3 cameraForward = _head.forward;
        cameraForward.y = 0;

        float angle = Vector3.SignedAngle(cameraForward, targetForward, Vector3.up);

        _origin.RotateAround(_head.position, Vector3.up, angle);
    }

    private void Start()
    {
        Recenter(); 
    }
}


/*
 [SerializeField] XROrigin _xrOrigin;

    [SerializeField] Transform _target;

    [SerializeField] Transform _position;

    private void Awake()
    {
        Cashing();
    }

    private void Cashing()
    {
        _xrOrigin = _xrOrigin == null ? GetComponentInChildren<XROrigin>() : _xrOrigin;
    }

    private void Start()
    {
        Invoke("SetUp", 2);
    }


    [ContextMenu("Test")]
    public void SetUp()
    {
        if (_xrOrigin != null && _target != null)
        {
            SetUpXrOrigin(_target, _position);
        }
        else
        {
            Debug.LogError("XrOrigin or Target is null");
        }
    }

    public void SetUpXrOrigin(Transform target, Transform position)
    {
        var trackingMode = _xrOrigin.CurrentTrackingOriginMode;
        trackingMode = UnityEngine.XR.TrackingOriginModeFlags.Unbounded;
        _xrOrigin.MoveCameraToWorldLocation(position.position);  
    }
*/