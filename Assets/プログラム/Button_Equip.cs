using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_Equip : MonoBehaviour
{
    public int Enum;
    [SerializeField] public AudioSource AC;
    [SerializeField] public AudioClip ac1;
    [SerializeField] public AudioClip ac2;
    public void OnButtonClick()
    {
        if (Player_Manager.isWeapon[Enum] == true)
        {
            AC.PlayOneShot(ac1);
            Debug.Log("•Ší‚ğ•Ï‚¦‚Ü‚µ‚½");
            Player_System.player_weapon_id = Enum;
            GameObject.Find("Player_System").gameObject.transform.GetComponent<Player_System>().Wepon_Reset(Enum);
        }
        else
        {
            Debug.Log("‚»‚Ì•Ší‚Í‚à‚Á‚Ä‚¢‚Ü‚¹‚ñ");
            AC.PlayOneShot(ac2);
        }
    }
}
