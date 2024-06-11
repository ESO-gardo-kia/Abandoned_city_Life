using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Buy_Button : MonoBehaviour
{
    public int weaponNumber;
    public Gun_List gunList;
    [SerializeField] public AudioSource audioSource;
    [SerializeField] public AudioClip buySound;
    [SerializeField] public AudioClip notBuySound;
    public void OnButtonClick()
    {
        if (GameManager.playerMoney >= gunList.Data[weaponNumber].price 
            && Player_Manager.isWeapon[weaponNumber] == false)
        {
            audioSource.PlayOneShot(buySound);
            Debug.Log("武器を買いました");
            GameManager.playerMoney -= gunList.Data[weaponNumber].price;
            GameObject.Find("Player_System").GetComponent<PlayerUiSystem>().moneyText.GetComponent<Text>().text = "MONEY:" + GameManager.playerMoney.ToString();
            Player_Manager.isWeapon[weaponNumber] = true;
        }
        else
        {
            audioSource.PlayOneShot(notBuySound);
            Debug.Log("お金が足らない");
        }
    }
}
