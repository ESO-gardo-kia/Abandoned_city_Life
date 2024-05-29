using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectSystem : MonoBehaviour
{
    public string contact_text;
    public GameObject mainCanvas;
    public GameObject idlepos;
    public Animator anime;


    public CinemachineVirtualCamera cinemachineVirtualCamera;
    private void Start()
    {
        mainCanvas = transform.Find("Canvas/Panel").gameObject;
        mainCanvas.SetActive(false);
        mainCanvas.transform.localScale = Vector3.zero;
    }
    private void OnTriggerStay(Collider collision)
    {
        if (Input.GetKeyDown(KeyCode.E) && collision.transform.CompareTag("Player"))
        {
            if (!mainCanvas.activeSelf)
            {
                Canvas_Open();
            }
            else
            {
                Canvas_Close();
            }
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.transform.CompareTag("Player") && mainCanvas.activeSelf)
        {
            Canvas_Close();
        }
    }
    public void Canvas_Open()
    {
        cinemachineVirtualCamera.Priority = 10;
        mainCanvas.SetActive(true);
        anime.SetTrigger("open");
    }
    public void Canvas_Close()
    {
        cinemachineVirtualCamera.Priority = 1;
        mainCanvas.SetActive(false);
        anime.SetTrigger("close");

    }
    public void Button_System(int num)
    {
        GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        gm.Scene_Transition_Process(num);
    }
}
