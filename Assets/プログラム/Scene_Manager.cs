using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Scene_Manager : MonoBehaviour
{

    public string title_scene = "Title";
    public string test_stage = "Test_Stage";
    public string Select_scene = "Slect";
    public string Main_scene = "Main";

    void Awake()
    {
        Application.targetFrameRate = 60;
    }
    public void SM_Title_Transfer()
    {
        SceneManager.LoadScene(title_scene);
    }
    public void SM_Test_Transfer()
    {
        SceneManager.LoadScene(test_stage);
    }
    public void SM_Select_Transfer()
    {
        SceneManager.LoadScene(Select_scene);
    }
    public void SM_Main_Transfer()
    {
        SceneManager.LoadScene(Main_scene);
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
}