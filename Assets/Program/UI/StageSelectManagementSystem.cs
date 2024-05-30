using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;
using static Gun_List;

public class StageSelectManagementSystem : MonoBehaviour
{
    public string contact_text;
    [SerializeField] private Stage_Information stageInformation;
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject playerIdlepos;
    [SerializeField] private GameObject stageSelectPanelPrefab;
    [SerializeField] private StageStartButtonSystem stageStartButton;
    [SerializeField] private Text FixationRewardText;
    [SerializeField] private Text StageDescriptionText;
    [SerializeField] private Transform itemLineupPassObj;
    [SerializeField] private Animator anime;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip panelSound;

    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    private void Start()
    {
        mainPanel.SetActive(false);
        mainPanel.transform.localScale = Vector3.zero;
    }
    private void OnTriggerStay(Collider collision)
    {
        if (Input.GetKeyDown(KeyCode.E) && collision.transform.CompareTag("Player"))
        {
            if (!mainPanel.activeSelf)
            {
                cinemachineVirtualCamera.Priority = 10;
                anime.SetTrigger("open");
                StageSelectButtonReadIn();
                Canvas_Transition(true);
            }
            else
            {
                cinemachineVirtualCamera.Priority = 0;
                anime.SetTrigger("close");
                Canvas_Transition(false);
            }
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.transform.CompareTag("Player") && mainPanel.activeSelf)
        {
            cinemachineVirtualCamera.Priority = 0;
            Canvas_Transition(false);
        }
    }
    public void Canvas_Transition(bool isOpen)
    {
        /*
         * trueÇ™âÊñ ÇäJÇ≠éûÇÃèàóù
         * falseÇ™âÊñ Çï¬Ç∂ÇÈéûÇÃèàóù
         */
        if (isOpen)
        {
            audioSource.PlayOneShot(panelSound);
            mainPanel.SetActive(true);
            Player_System.movePermit = false;
            Player_System.isPanelOpen = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            DOTween.Sequence()
                .Append(mainPanel.GetComponent<RectTransform>().DOScale(Vector3.one, 0.25f)
                .SetEase(Ease.OutCirc)
                .OnComplete(() => {
                    //brain.enabled = false;
                }))
                .Play();
        }
        else
        {
            audioSource.PlayOneShot(panelSound);
            DOTween.Sequence()
                .Append(mainPanel.GetComponent<RectTransform>().DOScale(Vector3.zero, 0.25f)
                .SetEase(Ease.OutCirc)
              .OnComplete(() => {
                  mainPanel.SetActive(false);
                  Player_System.movePermit = true;
                  Player_System.isPanelOpen = false;
                  Cursor.visible = false;
                  Cursor.lockState = CursorLockMode.Locked;
                  Debug.Log("èIóπ");
              }))
                .Play();
        }
    }
    public void StageSelectButtonReadIn()
    {
        PanelReset();
        var Data = stageInformation.data;
        for (int i = 0; i < stageInformation.data.Count; i++)
        {
            if ((int)Data[i].stagetype != 3)
            {
                var stageselectpanelobj = Instantiate(stageSelectPanelPrefab, itemLineupPassObj);
                StageSelectSignalButtonSystem stageSelectSignalButtonSystem = stageselectpanelobj.GetComponent<StageSelectSignalButtonSystem>();
                stageselectpanelobj.transform.Find("StageName").GetComponent<Text>().text = "MISSION " + Data[i].stagenumber + " :" + Data[i].name;
                stageSelectSignalButtonSystem.stageNumber = i;
                stageSelectSignalButtonSystem.stageStartButton = stageStartButton;
                stageSelectSignalButtonSystem.FixationRewardText = FixationRewardText;
                stageSelectSignalButtonSystem.StageDescriptionText = StageDescriptionText;
            }
        }
    }
    public void Button_System(int transitionSceneNumber)
    {
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.Scene_Transition_Process(transitionSceneNumber);
    }
    private void PanelReset()
    {
        if (itemLineupPassObj.childCount != 0) 
            for (int i = 0; i < itemLineupPassObj.childCount; i++)
            {
                Destroy(itemLineupPassObj.GetChild(i).gameObject);
            }
    }
}
