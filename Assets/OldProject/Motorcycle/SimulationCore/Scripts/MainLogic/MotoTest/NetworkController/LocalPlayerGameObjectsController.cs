using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;
using UnityEditor;

public class LocalPlayerGameObjectsController : NetworkBehaviour
{
    [SerializeField] List<GameObject> gameObjectsOfLocalPlayer = new List<GameObject>();

    public void TurnOffObject() => gameObjectsOfLocalPlayer.ForEach(x => x.SetActive(false));
    public void TurnOnObject() => gameObjectsOfLocalPlayer.ForEach(x => x.SetActive(true));



    private void Awake() => InnitObjects();
    private void Start() => InnitObjects();
    private void OnEnable() => InnitObjects();

    private void InnitObjects()
    {
        if (isLocalPlayer)
        {
            TurnOnObject();
        }
        else
        {
            TurnOffObject();
        }
    }
}
