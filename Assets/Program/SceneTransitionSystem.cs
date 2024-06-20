using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneTransitionSystem : MonoBehaviour
{
    public Scene_List sceneList;
    public Stage_Information stageInfomation;

    public void SM_Title_Transfer()
    {
        SceneManager.LoadScene(stageInfomation.data[0].name);
    }
    public void SM_Test_Transfer()
    {
        SceneManager.LoadScene(stageInfomation.data[2].name);
    }
    public void SM_Select_Transfer()
    {
        SceneManager.LoadScene(stageInfomation.data[1].name);
    }
    public void SM_Main_Transfer()
    {
        SceneManager.LoadScene(stageInfomation.data[2].name);
    }
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
        Debug.Log("移動" + stage_number);
        SceneManager.LoadScene(sceneList.data[stage_number].sceneName);
    }
}