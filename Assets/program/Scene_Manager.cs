using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Scene_Manager : MonoBehaviour
{
    private GameManager gm;
    public Stage_Information si;

    private void Start()
    {
        gm = transform.GetComponent<GameManager>();
    }
    /*
    public void SM_Title_Transfer()
    {
        SceneManager.LoadScene(si.data[0].name);
    }
    public void SM_Test_Transfer()
    {
        SceneManager.LoadScene(si.data[2].name);
    }
    public void SM_Select_Transfer()
    {
        SceneManager.LoadScene(si.data[1].name);
    }
    public void SM_Main_Transfer()
    {
        SceneManager.LoadScene(si.data[2].name);
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
    */

    public void Load_Scene(int stage_number)
    {
        if (stage_number == 0) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        else
        {
            Debug.Log("移動" + stage_number);
            SceneManager.LoadScene((int)si.data[stage_number].tran_scene);
        }
    }
}