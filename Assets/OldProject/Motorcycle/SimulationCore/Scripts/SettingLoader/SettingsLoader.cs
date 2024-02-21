using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Mirror;

public class SettingsLoader : MonoSinglethon<SettingsLoader>
{

    string path = Application.streamingAssetsPath + "/VehiclePhysicSetting.json";

    VehiclePhysicSetting setting;

    [ContextMenu("Write to json")]
    public void WriteToStreamingAssets()
    {
        if (File.Exists(path))
        {
            File.WriteAllText(Application.streamingAssetsPath + "/VehiclePhysicSetting.json", JsonUtility.ToJson(_settings));
        }
        else
        {
            File.Create(path);
            WriteToStreamingAssets();
        }


    }

    public void ReadFromStreamingAssets()
    {
        _settings = UpdateVehiclePhysicsSettings(JsonUtility.FromJson<VehiclePhysicSettingFromJson>(File.ReadAllText(path)));
    }

    [SerializeField]
    private VehiclePhysicSetting _settings;

    public VehiclePhysicSetting Settings
    {
        get
        {
            SettingsInnit();
            _settings = UpdateVehiclePhysicsSettings(JsonUtility.FromJson<VehiclePhysicSettingFromJson>(File.ReadAllText(path)));
            return _settings;
            WriteToStreamingAssets();
        }
        set => _settings = value;
    }

    private void Awake() => SettingsInnit();

    private void OnEnable()
    {
        SettingsInnit();
    }

    private void SettingsInnit()
    {

        setting = setting == null ? new VehiclePhysicSetting() : setting;

        ReadFromStreamingAssets();
        if (_settings == null)
        {
            Debug.LogError("There is no settingsFromJson!");
        }

    }


    public VehiclePhysicSetting UpdateVehiclePhysicsSettings(VehiclePhysicSettingFromJson settingsFromJson)
    {



        setting.ACCELARATION_100_Km_H = settingsFromJson.ACCELARATION_100_Km_H;
        setting.MAX_SPEED_ACCELEARTION = settingsFromJson.MAX_SPEED_ACCELEARTION;
        setting.MAX_SPEED = settingsFromJson.MAX_SPEED;
        setting.MAX_STEER_ANGLE = settingsFromJson.MAX_STEER_ANGLE;
        setting.MIN_STEER_ANGLE = settingsFromJson.MIN_STEER_ANGLE;
        setting.SPEED_FOR_MAX_STEER_ANGLE = settingsFromJson.SPEED_FOR_MAX_STEER_ANGLE;
        setting.SPEED_FOR_MEDIAN_STEER_ANGLE = settingsFromJson.SPEED_FOR_MEDIAN_STEER_ANGLE;
        setting.SPEED_FOR_MIN_STEER_ANGLE = settingsFromJson.SPEED_FOR_MIN_STEER_ANGLE;
        setting.TIME_FOR_BURNOUT = settingsFromJson.TIME_FOR_BURNOUT;
        setting.WHILLIE = settingsFromJson.WHILLIE;
        setting.MOTO_MASS = settingsFromJson.MOTO_MASS;
        _settings = setting;
        WriteToStreamingAssets();
        return setting;
    }


}
