using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Buy_Button : MonoBehaviour
{
    public int weaponNumber;
    public Gun_List gunList;
    [SerializeField]public AudioSource audioSource;
    [SerializeField] public AudioClip buySound;
    [SerializeField] public AudioClip notBuySound;
    public void OnButtonClick()
    {
        Debug.Log(GameManager.playerMoney);
        if (GameManager.playerMoney >= gunList.Data[weaponNumber].price && Player_Manager.isWeapon[weaponNumber] == false)
        {
            audioSource.PlayOneShot(buySound);
            Debug.Log("•Ší‚ğ”ƒ‚¢‚Ü‚µ‚½");
            GameManager.playerMoney -= gunList.Data[weaponNumber].price;
            GameObject.Find("Player_System").GetComponent<Player_System>().moneyText.GetComponent<Text>().text = "MONEY:" + GameManager.playerMoney.ToString();
            Player_Manager.isWeapon[weaponNumber] = true;
        }
        else
        {
            audioSource.PlayOneShot(notBuySound);
            Debug.Log("‚¨‹à‚ª‘«‚ç‚È‚¢");
        }
    }
}
