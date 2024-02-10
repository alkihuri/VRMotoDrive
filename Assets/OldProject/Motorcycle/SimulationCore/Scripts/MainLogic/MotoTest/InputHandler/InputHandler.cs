using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR; 

public class InputHandler : MonoSinglethon<InputHandler>
{
    public float Torque { get; internal set; }
    public float Steer { get; internal set; }
    public float Brake { get; internal set; }

    [SerializeField]
    private XRNode xRNode = XRNode.RightHand;

    private List<InputDevice> devices = new List<InputDevice>();

    private InputDevice device;

    //to avoid repeat readings
    private bool triggerIsPressed;
    private bool primaryButtonIsPressed;
    private bool primary2DAxisIsChosen;
    private Vector2 primary2DAxisValue = Vector2.zero;
    private Vector2 prevPrimary2DAxisValue;
    private bool gripIsPressed;

    void GetDevice()
    {
        InputDevices.GetDevicesAtXRNode(xRNode, devices);
        device = devices.FirstOrDefault();
    }

    void OnEnable()
    {
        if (!device.isValid)
        {
            GetDevice();
        }
    }

    void Update()
    {
        if (!device.isValid)
        {
            GetDevice();
        }


        if (device.isValid)
            VrInput();
        else
            OldInput();
    }

    private void OldInput()
    {
        Torque = Input.GetAxis("Vertical");
        Steer = Input.GetAxis("Horizontal");    
        Brake = Input.GetAxis("Jump");  
    }

    private void VrInput()
    {
        // capturing trigger button press and release    
        bool triggerButtonValue = false;
        if (device.TryGetFeatureValue(CommonUsages.triggerButton, out triggerButtonValue) && triggerButtonValue && !triggerIsPressed)
        {
            triggerIsPressed = true;
        }
        else if (!triggerButtonValue && triggerIsPressed)
        {
            triggerIsPressed = false;
        }

        // capturing primary button press and release
        bool primaryButtonValue = false;
        InputFeatureUsage<bool> primaryButtonUsage = CommonUsages.primaryButton;

        if (device.TryGetFeatureValue(primaryButtonUsage, out primaryButtonValue) && primaryButtonValue && !primaryButtonIsPressed)
        {
            primaryButtonIsPressed = true;
        }
        else if (!primaryButtonValue && primaryButtonIsPressed)
        {
            primaryButtonIsPressed = false;
        }

        // capturing primary 2D Axis changes and release
        InputFeatureUsage<Vector2> primary2DAxisUsage = CommonUsages.primary2DAxis;
        // make sure the value is not zero and that it has changed
        if (primary2DAxisValue != prevPrimary2DAxisValue)
        {
            primary2DAxisIsChosen = false;
            //Debug.Log($"CHANGED and prev value is {prevPrimary2DAxisValue} and the new value is {primary2DAxisValue}");
        }
        // was for checking to see if the axis values were reading as changed properly
        /* else
        {
            Debug.Log($"Nope, prev value is {prevPrimary2DAxisValue} and the new value is {primary2DAxisValue}");
        } */
        if (device.TryGetFeatureValue(primary2DAxisUsage, out primary2DAxisValue) && primary2DAxisValue != Vector2.zero && !primary2DAxisIsChosen)
        {
            prevPrimary2DAxisValue = primary2DAxisValue;
            primary2DAxisIsChosen = true;
        }
        else if (primary2DAxisValue == Vector2.zero && primary2DAxisIsChosen)
        {
            prevPrimary2DAxisValue = primary2DAxisValue;
            primary2DAxisIsChosen = false;
        }

        // capturing grip value
        float gripValue;
        InputFeatureUsage<float> gripUsage = CommonUsages.grip;

        if (device.TryGetFeatureValue(gripUsage, out gripValue) && gripValue > 0 && !gripIsPressed)
        {
            gripIsPressed = true;
        }
        else if (gripValue == 0 && gripIsPressed)
        {
            gripIsPressed = false;
        }



        Torque = primary2DAxisValue.y;
        Steer = primary2DAxisValue.x;

        Brake = triggerButtonValue ? 1 : 0;
    }
}
