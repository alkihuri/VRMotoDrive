using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SpeedToText : MonoBehaviour
{

    [SerializeField]
    private Rigidbody _rigidBody;
    private TextMeshProUGUI _text;

    [SerializeField] RigidBodyController _velocityController;

    // Update is called once per frame

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _velocityController = GameObject.FindObjectOfType<RigidBodyController>();
    }
    void Update()
    {
        _text.text = "speed : "
            + (_velocityController.CurrentSpeed).ToString("#.")
            + "/" + _velocityController.MaxSpeed.ToString("#.");
    }
}
