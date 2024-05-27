using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static ContactObj_System;

public class EquipmentManagementSystem : MonoBehaviour
{
    [SerializeField] public string contact_text;
    [SerializeField] public GameObject mainCanvas;
    [SerializeField] public GameObject weaponPanel;
    [SerializeField] public GameObject playerIdlePositionObject;
    [SerializeField] public Transform itemLineupPassObj;
    private int weaponPanelnumber;
    private GameObject[] weaponPanelList;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip panelSound;

    //Production_Tableóp
    [SerializeField] public Gun_List gunList;
    [SerializeField] public GameObject StarObj;

    public CinemachineVirtualCamera cinemachineVirtualCamera;
    private void Start()
    {
        cinemachineVirtualCamera.Priority = 0;
        PanelReset();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (Input.GetKeyDown(KeyCode.E) && collision.transform.CompareTag("Player"))
        {

        }
    }
    public void Canvas_Transition(GameObject Panel, bool IS)
    {
        /*
         * trueÇ™âÊñ ÇäJÇ≠éûÇÃèàóù
         * falseÇ™âÊñ Çï¬Ç∂ÇÈéûÇÃèàóù
         */
        if (IS)
        {
            audioSource.PlayOneShot(panelSound);

            DOTween.Sequence()
                .Append(Panel.GetComponent<RectTransform>().DOScale(Vector3.one, 0.25f)
                .SetEase(Ease.OutCirc)
                .OnComplete(() => {
                    //brain.enabled = false;
                    Player_System.movePermit = false;
                    Player_System.isPanelOpen = true;
                    Panel.SetActive(true);
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.Confined;
                }))
                .Play();
        }
        else
        {
            audioSource.PlayOneShot(panelSound);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Player_System.movePermit = true;
            DOTween.Sequence()
                .Append(Panel.GetComponent<RectTransform>().DOScale(Vector3.zero, 0.25f)
                .SetEase(Ease.OutCirc)
              .OnComplete(() => {
                  rigidBody.useGravity = true;
                  moneyText.SetActive(false);
                  contactObjSystem.cinemachineVirtualCamera.Priority = 1;
                  //brain.enabled = true;
                  Player_System.movePermit = true;
                  Player_System.isPanelOpen = false;
                  Debug.Log("èIóπ");
              }))
                .Play();
        }
    }
    public void Gun_ReadIn()
    {
        cinemachineVirtualCamera.Priority = 10;
        PanelReset();
        for (int i = 0; i < gunList.Data.Count; i++)
        {
            GameObject wp = Instantiate(weaponPanel, Vector3.zero, Quaternion.identity, itemLineupPassObj);
            wp.name = weaponPanel.name + i.ToString();
            wp.transform.Find("WeaponImage").GetComponent<Image>().sprite = gunList.Data[i].sprite_id;

            Text Wtext = wp.transform.Find("WeaponData").gameObject.GetComponent<Text>();
            //if (gl.Data[i].name != null)
            var Guns = gunList.Data[i];
            Wtext.text =
            "Name:" + Guns.name +
            "\r\nçUåÇóÕ:" + Guns.bullet_damage.ToString() +
            "\r\nòAéÀë¨ìx:" + Guns.rapid_fire_rate.ToString() +
            "\r\nëïíeêî:" + Guns.loaded_bullets.ToString() +
            "\r\nî≠éÀíeêî:" + Guns.multi_bullet.ToString() +
            "\r\nägéUìx:" + Guns.diffusion__chance.ToString();

            Button_Equip EQbutton = wp.transform.Find("EQUIP").gameObject.GetComponent<Button_Equip>();
            EQbutton.Enum = i;
            Buy_Button BUbutton = wp.transform.Find("BUY").gameObject.GetComponent<Buy_Button>();
            BUbutton.Enum = i;
            BUbutton.transform.Find("Text").GetComponent<Text>().text = "ã‡äz:" + gunList.Data[i].price.ToString();
            Debug.Log("ÉåÉAìx" + Guns.rarity);
            for (int a = 1; a < Guns.rarity; a++)
            {
                GameObject star = Instantiate(StarObj, wp.transform.Find("starlist"));
            }
        }
    }

    private void PanelReset()
    {
        weaponPanelnumber = itemLineupPassObj.childCount;
        if (weaponPanelnumber != 0) for (int i = 0; i < weaponPanelnumber; i++)
            {
                Destroy(itemLineupPassObj.GetChild(i).gameObject);
            }
    }
}
