using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buy_Button : MonoBehaviour
{
    public int Enum;
    public Gun_List gl;
    public void OnButtonClick()
    {
        if (GameManager.Money <= gl.Data[Enum].price && !Player_Manager.isWeapon[Enum])
        {
            Debug.Log("����𔃂��܂���");
            Player_Manager.isWeapon[Enum] = true;
        }
        else Debug.Log("����������Ȃ�");
    }
}
