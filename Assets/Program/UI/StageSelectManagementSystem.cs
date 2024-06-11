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
    [SerializeField] private Image stageSpritePanel;
    [SerializeField] private Text FixationRewardText;
    [SerializeField] private Text StageDescriptionText;
    [SerializeField] private Transform itemLineupPassObj;
    [SerializeField] private Animator anime;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip panelSound;

    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

    [SerializeField] private float canvasOpenCoolDown;
    private float canvasOpenCoolDownCount;
    private void Start()
    {
        mainPanel.SetActive(false);
        mainPanel.transform.localScale = Vector3.zero;
        cinemachineVirtualCamera.Priority = 0;
        anime.SetTrigger("close");
        Canvas_Transition(false);
    }
    private void Update()
    {
        if (canvasOpenCoolDownCount < canvasOpenCoolDown) canvasOpenCoolDownCount += Time.deltaTime;
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerUiSystem>().contactText.text = contact_text;
        }
    }
    private void OnTriggerStay(Collider collision)
    {
        if (Input.GetKeyDown(KeyCode.E) && collision.transform.CompareTag("Player") 
            && canvasOpenCoolDownCount >= canvasOpenCoolDown)
        {
            if (!mainPanel.activeSelf)
            {
                cinemachineVirtualCamera.Priority = 10;
                anime.SetTrigger("open");
                StageSelectButtonReadIn();
                Canvas_Transition(true);
                canvasOpenCoolDownCount = 0;
            }
            else
            {
                cinemachineVirtualCamera.Priority = 0;
                anime.SetTrigger("close");
                Canvas_Transition(false);
                canvasOpenCoolDownCount = 0;
            }
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.transform.CompareTag("Player") && mainPanel.activeSelf)
        {
            collision.gameObject.GetComponent<PlayerUiSystem>().contactText.text = null;
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
            CanvasOpenWhenPlayerProcessing();
            DOTween.Sequence()
                .Append(mainPanel.GetComponent<RectTransform>().DOScale(Vector3.one, 0.25f)
                .SetEase(Ease.OutCirc)
                .OnComplete(() =>
                {
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
              .OnComplete(() =>
              {
                  CanvasCloseWhenPlayerProcessing();
                  Debug.Log("èIóπ");
              }))
                .Play();
        }
    }
    private void CanvasOpenWhenPlayerProcessing()
    {
        mainPanel.SetActive(true);
        Player_System.movePermit = false;
        PlayerUiSystem.isPanelOpen = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
    void CanvasCloseWhenPlayerProcessing()
    {
        mainPanel.SetActive(false);
        Player_System.movePermit = true;
        PlayerUiSystem.isPanelOpen = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void StageSelectButtonReadIn()
    {
        PanelReset();
        var Data = stageInformation.data;
        for (int i = 0; i < stageInformation.data.Count; i++)
        {
            if (Data[i].stagenumber > 0)
            {
                var stageselectpanelobj = Instantiate(stageSelectPanelPrefab, itemLineupPassObj);
                StageSelectSignalButtonSystem stageSelectSignalButtonSystem = stageselectpanelobj.GetComponent<StageSelectSignalButtonSystem>();
                stageselectpanelobj.transform.Find("StageName").GetComponent<Text>().text = "MISSION " + Data[i].stagenumber + " :" + Data[i].name;
                stageSelectSignalButtonSystem.stageNumber = i;
                stageSelectSignalButtonSystem.stageStartButton = stageStartButton;
                stageSelectSignalButtonSystem.stageSpritePanel = stageSpritePanel;
                stageSelectSignalButtonSystem.fixationRewardText = FixationRewardText;
                stageSelectSignalButtonSystem.stageDescriptionText = StageDescriptionText;
            }
        }
    }
    public void Button_System(int transitionSceneNumber)
    {
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.SceneTransitionProcess(transitionSceneNumber);
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
