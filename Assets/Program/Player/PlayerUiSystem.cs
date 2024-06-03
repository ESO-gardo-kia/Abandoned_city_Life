using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class PlayerUiSystem : MonoBehaviour
{
    public void Canvas_Transition(
         GameObject Panel
        , bool IS
        , Rigidbody rigidbody
        , GameObject moneyText
        , GameObject idlepos
        , CinemachineVirtualCamera cinemachineVirtualCamera
        , AudioSource audioSource
        , AudioClip panelSound)
    {
        /*
         * trueÇ™âÊñ ÇäJÇ≠éûÇÃèàóù
         * falseÇ™âÊñ Çï¬Ç∂ÇÈéûÇÃèàóù
         */
        if (IS)
        {
            /*
            //brain.enabled = false;
            Player_System.movePermit = false;
            Player_System.isPanelOpen = true;
            audioSource.PlayOneShot(panelSound);
            switch (contactObjSystem2.Cont)
            {
                case Contact_Type.Production_Table:
                    moneyText.SetActive(true);
                    moneyText.GetComponent<Text>().text = "MONEY:" + GameManager.playerMoney.ToString();
                    NewMethod(rigidbody,idlepos,cinemachineVirtualCamera);
                    break;
                case Contact_Type.StageSelect:
                    NewMethod(rigidbody, idlepos, cinemachineVirtualCamera);
                    break;
            }

            contactObjSystem1.Canvas_Open();
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
            DOTween.Sequence()
                .Append(Panel.GetComponent<RectTransform>().DOScale(Vector3.zero, 0.25f)
                .SetEase(Ease.OutCirc)
              .OnComplete(() => {
                  rigidbody.useGravity = true;
                  moneyText.SetActive(false);
                  contactObjSystem2.VC.Priority = 1;
                  Player_System.movePermit = true;
                  Player_System.isPanelOpen = false;
                  Cursor.visible = false;
                  Cursor.lockState = CursorLockMode.Locked;
                  contactObjSystem1.Canvas_Close();
                  Debug.Log("èIóπ");
           
              }))
                .Play();
            */
        }
    }

    private void NewMethod(Rigidbody rigidbody,GameObject idlepos,CinemachineVirtualCamera cinemachineVirtualCamera)
    {
        rigidbody.useGravity = false;
        rigidbody.velocity = Vector3.zero;
        transform.position = idlepos.transform.position;
        transform.localEulerAngles = idlepos.transform.localEulerAngles;
        cinemachineVirtualCamera.Priority = 100;
            
    }
}
