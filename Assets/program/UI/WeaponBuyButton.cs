using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponBuyButton : MonoBehaviour
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
            GameManager.playerMoney -= gunList.Data[weaponNumber].price;
            GameObject.Find("Player_System").GetComponent<PlayerUiSystem>().moneyText.GetComponent<Text>().text = "MONEY:" + GameManager.playerMoney.ToString();
            Player_Manager.isWeapon[weaponNumber] = true;
        }
        else
        {
            audioSource.PlayOneShot(notBuySound);
        }
    }
}
