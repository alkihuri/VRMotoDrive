using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;

public class MotoNetworkController : NetworkBehaviour
{
    [SerializeField] Camera playerCamera;

    private void Awake()
    {

    }

    private void OnEnable()
    {
        if (!isLocalPlayer)
        {
            playerCamera.gameObject.SetActive(false);
        }
        else
        {
            playerCamera.gameObject.SetActive(true);
        }
    }
    // Start is called before the first frame update
    void Start()
    { 
        if (!isLocalPlayer)
        {
            playerCamera.gameObject.SetActive(false);
        }
        else
        {
            playerCamera.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
