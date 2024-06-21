using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelectButtonSystem : MonoBehaviour
{
    public Gun_List gun_list;
    public SpriteRenderer spriteRenderer;
    public Text nameText;
    public void PanelInfomationInitialaization(int i)
    {
        spriteRenderer.sprite = gun_list.Data[i].sprite_id;
        nameText.text = gun_list.Data[(int)i].name;
    }
}
