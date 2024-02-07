using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class VehiclceInputSystem : MonoBehaviour
{

    private IVehicleController _controller;

    private float _torque;
    private float _steer;
    private float _brake;
    private float _wheellie;

    public float Torque { get => Mathf.Clamp(_torque, -1, 1); set => _torque = value; }
    public float Steer { get => Mathf.Clamp(_steer, -1, 1); set => _steer = value; }
    public float Brake { get => Mathf.Clamp(_brake, -1, 1); set => _brake = value; }
    public float Wheellie { get => Mathf.Clamp(_wheellie, -1, 1); set => _wheellie = value; }

    private void Awake()
    {
        Cashing();
    }

    private void Cashing()
    {
        _controller = GetComponent<IVehicleController>();
    }

    private void Update()
    {
        GetValues();
        ApplyValues();
    }

    private void ApplyValues()
    {
        _controller.ApplyInputs(Torque, Steer, Brake);
    }

    private void GetValues()
    {
        Torque = InputHandler.Instance.Torque;
        Steer = InputHandler.Instance.Steer;
        Brake = InputHandler.Instance.Brake;
    }




}
