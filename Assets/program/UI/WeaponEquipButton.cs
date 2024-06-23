using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponEquipButton : MonoBehaviour
{
    public int weaponNumber;
    [SerializeField] public AudioSource audioSource;
    [SerializeField] public AudioClip equipSound;
    [SerializeField] public AudioClip notEquipSound;
    public void OnButtonClick()
    {
        if (Player_Manager.isWeapon[weaponNumber] == true)
        {
            audioSource.PlayOneShot(equipSound);
            Debug.Log("�����ς��܂���");
            PlayerWeaponSystem.player_weapon_id = weaponNumber;
            GameObject.Find("Player_System").gameObject.transform.GetComponent<PlayerWeaponSystem>().WeponChange();
        }
        else
        {
            Debug.Log("���̕���͂����Ă��܂���");
            audioSource.PlayOneShot(notEquipSound);
        }
    }
}
