using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Gun_List;

public class WeaponSelectButtonSystem : MonoBehaviour
{
    public Gun_List gun_list;
    public WeaponEquipButton equipmentButton;
    public WeaponBuyButton buyButton;
    public Text weaponDescriptionText;
    public Image spriteRenderer;
    public Text nameText;
    public int weaponNunber;
    public void PanelInfomationInitialaization(int i,ref WeaponEquipButton equipmentButton, ref WeaponBuyButton buyButton, ref Text weaponDescriptionText)
    {
        this.equipmentButton = equipmentButton;
        this.buyButton = buyButton;
        this.weaponDescriptionText = weaponDescriptionText;
        spriteRenderer.sprite = gun_list.Data[i].sprite_id;
        nameText.text = gun_list.Data[(int)i].name;
        weaponNunber = i;
    }
    public void EquipmentWeaponChenge()
    {
        buyButton.weaponNumber = weaponNunber;
        equipmentButton.weaponNumber = weaponNunber;

        var Guns = gun_list.Data[weaponNunber];
        weaponDescriptionText.text =
        "Name:" + Guns.name +
        "\nDAMAGE:" + Guns.bullet_damage.ToString() +
        "Å~" + Guns.multi_bullet.ToString() + "SHOT" +
        "\nFIRE RATE:" + Guns.rapid_fire_rate.ToString() +
        "\nLOADED BULLET:" + Guns.loaded_bullets.ToString() +
        "\nDEFFUSION CHANCE:" + Guns.diffusion__chance.ToString();
    }
}
