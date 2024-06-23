using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneTransitionSystem : MonoBehaviour
{
    public Scene_List sceneList;
    public Stage_Information stageInfomation;

    public void SM_Replay_Transfer()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void SM_EndTransfer()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
        Application.Quit();//ゲームプレイ終了
#endif
    }

    public void Load_Scene(int stage_number)
    {
        SceneManager.LoadScene(sceneList.data[stage_number].sceneName);
    }
}