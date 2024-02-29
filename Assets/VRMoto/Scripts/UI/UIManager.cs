using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
public class UIManager : MonoBehaviour
{

    [SerializeField] private GameObject _vrCanvas;
    [SerializeField] private GameObject _nonVrCanvas;
    [SerializeField] private GameObject _vrPlayer;
    [SerializeField] private GameObject _nonVrPlayer;
    [SerializeField] private bool _isVrMode;

    public void SetVrMode(bool isVrMode)
    {
        _isVrMode = isVrMode;

        _vrCanvas.SetActive(_isVrMode);
        _vrPlayer.SetActive(_isVrMode);

        _nonVrCanvas.SetActive(!_isVrMode);
        _nonVrPlayer.SetActive(!_isVrMode);
    }
    // Start is called before the first frame update
    void Start()
    {
        // check if VR is enabled 
        SetVrMode(XRSettings.enabled);
    }
}
