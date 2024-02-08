using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buy_Button : MonoBehaviour
{
    public int Enum;
    public Gun_List gl;
    public void OnButtonClick()
    {
        //GameManager.Money <= gl.Data[Enum].price &&
        Debug.Log(GameManager.Money);
        if (GameManager.Money >= gl.Data[Enum].price && Player_Manager.isWeapon[Enum] == false)
        {
            Debug.Log("•Ší‚ğ”ƒ‚¢‚Ü‚µ‚½");
            GameManager.Money -= gl.Data[Enum].price;
            Player_Manager.isWeapon[Enum] = true;
        }
        else Debug.Log("‚¨‹à‚ª‘«‚ç‚È‚¢");
    }
}
