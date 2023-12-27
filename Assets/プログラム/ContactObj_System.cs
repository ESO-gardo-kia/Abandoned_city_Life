using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ContactObj_System : MonoBehaviour
{
    public enum Condition{Test,Title,Select,Main,Replay,End}
    public Condition Cond;
    public string title_scene = "Title";
    public string test_stage = "Test_Stage";
    public string Select_scene = "Slect";
    public string Main_scene = "Main";
    public string contact_text;

    public void Contact_function()
    {
        switch (Cond)
        {
            case Condition.Test:
                SceneManager.LoadScene(test_stage);
                break;
            case Condition.Title:
                SceneManager.LoadScene(title_scene);
                break;
            case Condition.Select:
                SceneManager.LoadScene(Select_scene);
                break;
            case Condition.Main:
                SceneManager.LoadScene(Main_scene);
                break;
            case Condition.Replay:
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;
            case Condition.End:
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
        Application.Quit();//ゲームプレイ終了
#endif
                break;
        }
    }
}
