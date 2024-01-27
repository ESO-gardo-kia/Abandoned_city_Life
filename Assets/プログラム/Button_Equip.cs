using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_Equip : MonoBehaviour
{
    public int Enum;
    public void OnButtonClick()
    {
        Debug.Log("•Ší‚ğ•Ï‚¦‚Ü‚µ‚½");
        Debug.Log(Enum);
        Player_System.player_weapon_id = Enum;
        GameObject.Find("Player_System").gameObject.transform.GetComponent<Player_System>().Wepon_Reset(Enum);
    }
}
