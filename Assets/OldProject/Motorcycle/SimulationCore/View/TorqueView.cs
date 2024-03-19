using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class TorqueView : MonoBehaviour
{

    [SerializeField] WheelCollider _rearWheel;
    [SerializeField] Transform _torqueArrow;
    [SerializeField] MotoEngine _motoEngine;
    [SerializeField] TextMeshProUGUI _torqueText;


    const int MIN_TORQUE_ANGLE = 0;
    const int MAX_TORQUE_ANGLE = 270;
    // Start is called before the first frame update

    private void Awake()
    {
        Cashing();
    }

    private void Cashing()
    {
        if (_rearWheel == null || _motoEngine == null)
        {
            Debug.LogError("Rear wheel or moto engine is not assigned");
        }
    }

    private void Update()
    {
        UpdateArrowRotation(_motoEngine, _torqueArrow);
    }

    public void UpdateArrowRotation(MotoEngine motoEngine, Transform arrow)
    {
        arrow.localEulerAngles = new Vector3(0, 0, -Mathf.Lerp(MIN_TORQUE_ANGLE, MAX_TORQUE_ANGLE, InputHandler.Instance.Torque));
        _torqueText.text = "Torque: " + motoEngine.CurrentRearWheelTorque.ToString("#.");
    }

}
