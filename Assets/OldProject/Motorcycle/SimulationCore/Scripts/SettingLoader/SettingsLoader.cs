using System.IO;
using UnityEngine;

public class SettingsLoader : MonoSinglethon<SettingsLoader>
{
    private const string FILE_NAME = "VehiclePhysicSetting.json";
    private string path;

    [SerializeField]
    private VehiclePhysicSetting _settingsPreset;

    private void Awake()
    {
        path = Path.Combine(PathFinder.ConfigsPath, FILE_NAME);
    }

    private void Start()
    {
        ReadFromStreamingAssets();
    }

    [ContextMenu("Write to json")]
    public void WriteToStreamingAssets()
    {
        using (StreamWriter writer = File.CreateText(path))
        {
            writer.Write(JsonUtility.ToJson(_settingsPreset));
        }
    }

    public void ReadFromStreamingAssets()
    {
        if (File.Exists(path))
        {
            _settingsPreset = UpdateVehiclePhysicsSettings(JsonUtility.FromJson<VehiclePhysicSettingFromJson>(File.ReadAllText(path)));
        }
        else
        {
            Debug.Log("File not found. Initializing settings from preset");
            InitializeSettingsFromPreset();
        }
    }

    public VehiclePhysicSetting Settings => _settingsPreset;

    private void InitializeSettingsFromPreset()
    {
        // Initialize settings from preset here
    }

    public VehiclePhysicSetting UpdateVehiclePhysicsSettings(VehiclePhysicSettingFromJson settingsFromJson)
    {
        _settingsPreset = new VehiclePhysicSetting()
        {
            ACCELARATION_100_Km_H = settingsFromJson.ACCELARATION_100_Km_H,
            MAX_SPEED_ACCELEARTION = settingsFromJson.MAX_SPEED_ACCELEARTION,
            MAX_SPEED = settingsFromJson.MAX_SPEED,
            MAX_STEER_ANGLE = settingsFromJson.MAX_STEER_ANGLE,
            MIN_STEER_ANGLE = settingsFromJson.MIN_STEER_ANGLE,
            SPEED_FOR_MAX_STEER_ANGLE = settingsFromJson.SPEED_FOR_MAX_STEER_ANGLE,
            SPEED_FOR_MEDIAN_STEER_ANGLE = settingsFromJson.SPEED_FOR_MEDIAN_STEER_ANGLE,
            SPEED_FOR_MIN_STEER_ANGLE = settingsFromJson.SPEED_FOR_MIN_STEER_ANGLE,
            TIME_FOR_BURNOUT = settingsFromJson.TIME_FOR_BURNOUT,
            WHILLIE = settingsFromJson.WHILLIE,
            MOTO_MASS = settingsFromJson.MOTO_MASS
        };

        WriteToStreamingAssets();
        return _settingsPreset;
    }
}
