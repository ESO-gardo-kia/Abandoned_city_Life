using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Manager : MonoBehaviour
{
    static public int[] Item_Inventory = new int[10];
    [SerializeField] private Item_Infomation itemInfomation;
    public static bool[] isWeapon; 


    private void Start()
    {
        isWeapon = new bool[100];
        for (int i = 1; i > 100; i++)
        {
            isWeapon[i] = false;
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("以下インベントリーの中身");
            for (int i = 0; i < Item_Inventory.Length; i++)
            {
                Debug.Log(itemInfomation.Info[i].Name+Item_Inventory[i]);
            }
        }
    }
}
