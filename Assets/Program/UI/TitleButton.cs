using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleButton : MonoBehaviour
{
    public GameManager Manager;
    private void Start()
    {
        Manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    public void GameStart()
    {
        Manager.SceneTransitionProcess(1);
    }
}
