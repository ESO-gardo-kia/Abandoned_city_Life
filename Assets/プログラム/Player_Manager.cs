using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Manager : MonoBehaviour
{
    static public int[] Item_Inventory = new int[10];
    [SerializeField] private Item_Infomation I_I;
    public static bool[] isWeapon; 


    private void Start()
    {
        isWeapon[0] = true;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {

        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("以下インベントリーの中身");
            for (int i = 0; i < Item_Inventory.Length; i++)
            {
                Debug.Log(I_I.Info[i].Name+Item_Inventory[i]);
            }
        }
    }
}
