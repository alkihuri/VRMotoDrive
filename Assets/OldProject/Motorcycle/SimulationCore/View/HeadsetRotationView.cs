using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadsetRotationView : MonoBehaviour
{

    const int STEP = -40;
    [SerializeField] Transform _currentState;
    [SerializeField] Transform _centerPosition;


    [SerializeField] Image _steerImage;
    [SerializeField] Image _torqueImage;

    // Update is called once per frame
    void Update()
    {
        _currentState.localEulerAngles = new Vector3(0, 0, InputHandler.Instance.Steer * STEP);
        // set anchored position to the head position   
        _currentState.localPosition = new Vector3(0, InputHandler.Instance.Torque * -STEP, 0);

        _torqueImage.color = new Color(Mathf.Abs(InputHandler.Instance.Torque), 1 - Mathf.Abs(InputHandler.Instance.Torque), 0, 1);
        _steerImage.color = new Color(Mathf.Abs(InputHandler.Instance.Steer), 1 - Mathf.Abs(InputHandler.Instance.Steer), 0, 1);

    }
}
