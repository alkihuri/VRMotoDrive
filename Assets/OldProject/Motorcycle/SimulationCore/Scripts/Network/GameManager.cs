using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameManager : NetworkBehaviour
{



    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {

    }

    public override void OnStartServer()
    {
        base.OnStartServer(); 
        Debug.Log("Server started");
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        Debug.Log("Client started");
        var game = new NonVrGame();
       // SpawnPlayer(game);
    }

    public void SpawnPlayer(IGameType game) => game.SpawnPlayer();

}

public interface IGameType
{
    void SpawnPlayer();
}

public class VrGame : IGameType
{
    public void SpawnPlayer()
    {
        var vrPlayer = GameObject.Instantiate(Resources.Load<GameObject>("MotoVR"));
        NetworkServer.Spawn(vrPlayer);
        Debug.Log("Spawn player in VR");
    }
}


public class NonVrGame : IGameType
{
    public void SpawnPlayer()
    { 
        var nonVrPlayer = GameObject.Instantiate(Resources.Load<GameObject>("MotoNonVR"));
        NetworkServer.Spawn(nonVrPlayer);
        Debug.Log("Spawn player in NonVR");
    }
}