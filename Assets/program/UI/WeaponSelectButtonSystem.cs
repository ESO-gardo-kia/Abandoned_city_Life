using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelectButtonSystem : MonoBehaviour
{
    public Gun_List gun_list;
    public WeaponEquipButton equipmentButton;
    public WeaponBuyButton buyButton;
    public Image spriteRenderer;
    public Text nameText;
    public int weaponNunber;
    public void PanelInfomationInitialaization(int i,ref WeaponEquipButton button)
    {
        equipmentButton = button;
        spriteRenderer.sprite = gun_list.Data[i].sprite_id;
        nameText.text = gun_list.Data[(int)i].name;
        weaponNunber = i;
    }
    public void EquipmentWeaponChenge()
    {
        equipmentButton.weponeNumber = weaponNunber;
    }
}
