using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;
public class EquipmentManagementSystem : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] private string contact_text;
    [SerializeField] private GameObject mainCanvas;
    [SerializeField] private GameObject weaponPanelObject;
    [SerializeField] private WeaponEquipButton weaponEquipButton;
    [SerializeField] private WeaponBuyButton weaponBuyButton;
    [SerializeField] private GameObject playerIdlePositionObject;
    [SerializeField] private Transform itemLineupPassObj;
    private GameObject[] weaponPanelList;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip panelSound;

    //Production_Tableóp
    [SerializeField] private Gun_List gunList;
    [SerializeField] private GameObject StarObj;
    [SerializeField] private float canvasOpenCoolDown;
    private float canvasOpenCoolDownCount;
    private void Start()
    {
        cinemachineVirtualCamera.Priority = 0;
    }
    private void Update()
    {
        if(canvasOpenCoolDownCount < canvasOpenCoolDown) canvasOpenCoolDownCount += Time.deltaTime;
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
            if (!mainCanvas.activeSelf)
            {
                Canvas_Transition(true);
                Gun_ReadIn();
                canvasOpenCoolDownCount = 0;
            }
            else
            {
                Canvas_Transition(false);
                canvasOpenCoolDownCount = 0;
            }
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.transform.CompareTag("Player") && mainCanvas.activeSelf)
        {
            collision.gameObject.GetComponent<PlayerUiSystem>().contactText.text = null;
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
            mainCanvas.SetActive(true);
            PlayerMainSystem.movePermit = false;
            PlayerUiSystem.isPanelOpen = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            cinemachineVirtualCamera.Priority = 10;
            DOTween.Sequence()
                .Append(mainCanvas.GetComponent<RectTransform>().DOScale(Vector3.one, 0.25f)
                .SetEase(Ease.OutCirc))
                .Play();
        }
        else
        {
            audioSource.PlayOneShot(panelSound);
            DOTween.Sequence()
                .Append(mainCanvas.GetComponent<RectTransform>().DOScale(Vector3.zero, 0.25f)
                .SetEase(Ease.OutCirc)
              .OnComplete(() => {
                  mainCanvas.SetActive(false);
                  PlayerMainSystem.movePermit = true;
                  PlayerUiSystem.isPanelOpen = false;
                  Cursor.visible = false;
                  Cursor.lockState = CursorLockMode.Locked;
                  cinemachineVirtualCamera.Priority = 0;
                  Debug.Log("èIóπ");
              }))
                .Play();
        }
    }
    public void Gun_ReadIn()
    {
        PanelReset();
        for (int i = 0; i < gunList.Data.Count; i++)
        {
            
            GameObject weaponepanel = Instantiate(weaponPanelObject, itemLineupPassObj);
            weaponepanel.GetComponent<WeaponSelectButtonSystem>().PanelInfomationInitialaization(i,ref weaponEquipButton);
            /*
            weaponepanel.name = weaponPanel.name + i.ToString();
            weaponepanel.transform.Find("WeaponImage").GetComponent<Image>().sprite = gunList.Data[i].sprite_id;

            Text Wtext = weaponepanel.transform.Find("WeaponData").gameObject.GetComponent<Text>();
            var Guns = gunList.Data[i];
            Wtext.text =
            "Name:" + Guns.name +
            "\r\nçUåÇóÕ:" + Guns.bullet_damage.ToString() +
            "\r\nòAéÀë¨ìx:" + Guns.rapid_fire_rate.ToString() +
            "\r\nëïíeêî:" + Guns.loaded_bullets.ToString() +
            "\r\nî≠éÀíeêî:" + Guns.multi_bullet.ToString() +
            "\r\nägéUìx:" + Guns.diffusion__chance.ToString();
            ButtonCreation(i, weaponepanel);
            */
        }
    }
    private void ButtonCreation(int i, GameObject panel)
    {
        WeaponEquipButton EQbutton = panel.transform.Find("EQUIP").gameObject.GetComponent<WeaponEquipButton>();
        EQbutton.weponeNumber = i;
        WeaponBuyButton BUbutton = panel.transform.Find("BUY").gameObject.GetComponent<WeaponBuyButton>();
        BUbutton.weaponNumber = i;
        BUbutton.transform.Find("Text").GetComponent<Text>().text = "ã‡äz:" + gunList.Data[i].price.ToString();
        for (int a = 1; a < gunList.Data[i].rarity; a++)
        {
            GameObject star = Instantiate(StarObj, panel.transform.Find("starlist"));
        }
    }
    private void PanelReset()
    {
        if (itemLineupPassObj.childCount != 0)
        {
            for (int i = 0; i < itemLineupPassObj.childCount; i++)
            {
                Destroy(itemLineupPassObj.GetChild(i).gameObject);
            }
        }
    }
}
