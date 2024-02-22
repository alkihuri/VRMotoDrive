using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class HeadPositionHandler : MonoSinglethon<HeadPositionHandler>
{
    [Header("Curves")]
    [SerializeField] AnimationCurve _steerCurve;


    [Header("Settings")]
    [SerializeField, Range(1, 100)] private int SENSIVITY = 10;
    [SerializeField] Transform _vrHead;

    [SerializeField, Range(-1, 1)] float _headPositionX;
    [SerializeField, Range(-1, 1)] float _headPositionY;
    [SerializeField, Range(-1, 1)] float _headPositionZ;

    [SerializeField, Range(-0, 360)] float _headZAngle;

    [SerializeField, Range(-0, 360)] float _headXAngle;

    public float HeadPositionX { get => Mathf.Clamp(_headPositionX * 10, -1, 1); set => _headPositionX = value; }
    public float HeadPositionY { get => Mathf.Clamp(_headPositionY * 10, -1, 1); set => _headPositionY = value; }
    public float HeadPositionZ { get => Mathf.Clamp(_headPositionZ * 10, -1, 1); set => _headPositionZ = value; }
    public float HeadZAngle
    {

        get
        {

            var angle = _headZAngle;
            if (angle > 180)
            {
                angle -= 360;
            }

            var absAngle = Mathf.Abs(angle);


            return Mathf.Clamp(-angle /SENSIVITY, -1, 1);
        }

        set => _headZAngle = value;
    }

    public float HeadXAngle
    {
        get
        {
            var angle = _headXAngle;
            if (angle > 180)
            {
                angle -= 360;
            }

            var absAngle = Mathf.Abs(angle);

            float reversedClamp = Mathf.InverseLerp(0, angle, SENSIVITY);


            return Mathf.Clamp((angle / (SENSIVITY * _steerCurve.Evaluate(reversedClamp))) * 2, -1, 1);
        }
        set => _headXAngle = value;
    }

    void Update()
    {
        HeadPositionX = _vrHead.localPosition.x;
        HeadPositionY = _vrHead.localPosition.y;
        HeadPositionZ = _vrHead.localPosition.z;
        HeadZAngle = _vrHead.localEulerAngles.z;
        HeadXAngle = _vrHead.localEulerAngles.x;

    }
}
