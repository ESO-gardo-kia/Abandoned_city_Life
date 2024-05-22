using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Buy_Button : MonoBehaviour
{
    public int Enum;
    public Gun_List gunList;
    [SerializeField]public AudioSource audioSource;
    [SerializeField] public AudioClip buySound;
    [SerializeField] public AudioClip notBuySound;
    public void OnButtonClick()
    {
        //GameManager.Money <= gl.Data[Enum].price &&
        Debug.Log(GameManager.playerMoney);
        if (GameManager.playerMoney >= gunList.Data[Enum].price && Player_Manager.isWeapon[Enum] == false)
        {
            audioSource.PlayOneShot(buySound);
            Debug.Log("ïêäÌÇîÉÇ¢Ç‹ÇµÇΩ");
            GameManager.playerMoney -= gunList.Data[Enum].price;
            GameObject.Find("Player_System").GetComponent<Player_System>().moneyText.GetComponent<Text>().text = "MONEY:" + GameManager.playerMoney.ToString();
            Player_Manager.isWeapon[Enum] = true;
        }
        else
        {
            audioSource.PlayOneShot(notBuySound);
            Debug.Log("Ç®ã‡Ç™ë´ÇÁÇ»Ç¢");
        }
    }
}
