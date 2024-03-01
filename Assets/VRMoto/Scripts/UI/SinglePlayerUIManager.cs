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
    public const string SELECT_GAME_SCENE_NAME = "Scene_select_singleplayer";

    public void LoadScene(string sceneName)
    {

        if (UIFader.Instance != null)
        {
            UIFader.Instance.FadeOut(() => SceneManager.LoadScene(sceneName));
        }
        else
        {
            SceneManager.LoadScene(GAME_SCENE_NAME);
        }
    }


    public void StartSelectGameScene() => LoadScene(SELECT_GAME_SCENE_NAME);
    public void StartGame() => LoadScene(GAME_SCENE_NAME);

}
