using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_Equip : MonoBehaviour
{
    public int Enum;
    public void OnButtonClick()
    {
        if (Player_Manager.isWeapon[Enum] == true)
        {
            Debug.Log("�����ς��܂���");
            Player_System.player_weapon_id = Enum;
            GameObject.Find("Player_System").gameObject.transform.GetComponent<Player_System>().Wepon_Reset(Enum);
        }
        else Debug.Log("���̕���͂����Ă��܂���");
    }
}
