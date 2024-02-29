using Mirror;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public partial class ConnectionManager : MonoBehaviour
{
    private const string CONFIG_PATH = "/ConnectionConfig.json";

    [SerializeField] private NetworkManager NetworkManager;

    public ConnectionInfo CurrentConnectionInfo;




    private async void Awake()
    {
        await ParseDataAsync();
#if UNITY_SERVER
        Debug.Log("In server");
        StartServer();  
#else
        Debug.Log("Not in server");
        StartClient();
#endif
    }

    public async Task ParseDataAsync()
    {
        Debug.Log("Parsing connection data from congfig file...");
        await ReadConfigFromFile();
        NetworkManager.networkAddress = CurrentConnectionInfo.IP_Add;
    }

    public void StartHost()
    {
        Debug.Log("Starting host...");
        NetworkManager.StartHost();
    }

    public void StartClient()
    {
        Debug.Log("Starting client...");
        try
        { 
            NetworkManager.StartClient();
        }
        catch
        {
            Debug.Log("Failed to start client");    
        }
    }

    public void StartServer()
    {

    }

    // [Button("ТЕСТОВАЯ ЗАПИСЬ КОНФИГА")]
    public void WriteTestConfigToFile()
    {
        var testConfig = new ConnectionInfo
        {
            IP_Add = "192.168.1.1",
            Port = "7777",
            Password = "1234",
            PlayerName = "Player1"
        };

        string json = JsonUtility.ToJson(testConfig);
        System.IO.File.WriteAllText(Application.streamingAssetsPath + CONFIG_PATH, json);
    }

    public Task ReadConfigFromFile()
    {
        string json = System.IO.File.ReadAllText(Application.streamingAssetsPath + CONFIG_PATH);
        CurrentConnectionInfo = JsonUtility.FromJson<ConnectionInfo>(json);
        return Task.CompletedTask;
    }
}
