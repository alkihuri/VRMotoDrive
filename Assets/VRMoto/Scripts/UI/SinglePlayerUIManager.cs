using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Unity.VisualScripting;

public class SinglePlayerUIManager : MonoBehaviour
{
    public const string GAME_SCENE_NAME = "VR_MotorCycleTestSceneVR_RallyTrack";

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }


    public void StartGame()
    {
        if (UIFader.Instance != null)
        {
            UIFader.Instance.FadeOut(() => LoadScene(GAME_SCENE_NAME));
        }
        else
        {
            LoadScene(GAME_SCENE_NAME);
        }
    }

}
