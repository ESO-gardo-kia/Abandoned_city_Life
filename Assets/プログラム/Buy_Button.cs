using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Buy_Button : MonoBehaviour
{
    public int Enum;
    public Gun_List gl;
    [SerializeField]public AudioSource AC;
    [SerializeField] public AudioClip ac1;
    [SerializeField] public AudioClip ac2;
    public void OnButtonClick()
    {
        //GameManager.Money <= gl.Data[Enum].price &&
        Debug.Log(GameManager.Money);
        if (GameManager.Money >= gl.Data[Enum].price && Player_Manager.isWeapon[Enum] == false)
        {
            AC.PlayOneShot(ac1);
            Debug.Log("ïêäÌÇîÉÇ¢Ç‹ÇµÇΩ");
            GameManager.Money -= gl.Data[Enum].price;
            GameObject.Find("Player_System").GetComponent<Player_System>().money_text.GetComponent<Text>().text = "MONEY:" + GameManager.Money.ToString();
            Player_Manager.isWeapon[Enum] = true;
        }
        else
        {
            AC.PlayOneShot(ac2);
            Debug.Log("Ç®ã‡Ç™ë´ÇÁÇ»Ç¢");
        }
    }
}
