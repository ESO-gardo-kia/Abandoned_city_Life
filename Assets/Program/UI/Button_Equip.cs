using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_Equip : MonoBehaviour
{
    public int weponeNumber;
    [SerializeField] public AudioSource audioSource;
    [SerializeField] public AudioClip equipSound;
    [SerializeField] public AudioClip notEquipSound;
    public void OnButtonClick()
    {
        if (Player_Manager.isWeapon[weponeNumber] == true)
        {
            audioSource.PlayOneShot(equipSound);
            Debug.Log("武器を変えました");
            PlayerWeaponSystem.player_weapon_id = weponeNumber;
            GameObject.Find("Player_System").gameObject.transform.GetComponent<PlayerWeaponSystem>().WeponChange();
        }
        else
        {
            Debug.Log("その武器はもっていません");
            audioSource.PlayOneShot(notEquipSound);
        }
    }
}
