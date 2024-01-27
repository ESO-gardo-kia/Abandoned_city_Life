using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ContactObj_System : MonoBehaviour
{
    public enum Contact_Type{None,StageSelect,Production_Table }
    public Contact_Type Cont;
    public string contact_text;
    public GameObject Panel;
    public GameObject WPanel;
    private GameObject[] WPanel_List;

    //Production_Table—p
    [SerializeField] public Gun_List gl;

    private void Start()
    {
        switch (Cont)
        {
            case Contact_Type.None:

                break;
            case Contact_Type.StageSelect:
                Panel = transform.Find("Canvas/Panel").gameObject;
                Panel.SetActive(false);
                Panel.transform.localScale = Vector3.zero;
                break;
            case Contact_Type.Production_Table:
                Panel = transform.Find("Canvas/Panel").gameObject;
                Panel.SetActive(false);
                Panel.transform.localScale = Vector3.zero;
                break;
        }
    }
    public void Canvas_Open()
    {
        Panel.SetActive(true);
        switch (Cont)
        {
            case Contact_Type.None:

                break;
            case Contact_Type.StageSelect:
                break;
            case Contact_Type.Production_Table:
                Gun_ReadIn();
                break;
        }
    }
    public void Canvas_Close()
    {
        Panel.SetActive(false);
    }
    public void Button_System(int num)
    {
        GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        gm.Scene_Transition_Process(num);
    }
    public void Gun_ReadIn()
    {
        string ObjectsPass = "Canvas/Panel/ScrollView/Viewport/Content";
        int wpn = transform.Find(ObjectsPass).childCount;
        if(wpn != 0) for (int i = 0; i < wpn; i++) Destroy(transform.Find(ObjectsPass).GetChild(i).gameObject);
        for (int i = 0; i < gl.Data.Count; i++)
        {
            GameObject wp = Instantiate(WPanel, Vector3.zero, Quaternion.identity, transform.Find(ObjectsPass));
            wp.name = WPanel.name + i.ToString();

            Text Wtext = wp.transform.Find("WeaponData").gameObject.GetComponent<Text>();
            if(gl.Data[i].name != null) Wtext.text = "" +
                "Name:"+ gl.Data[i].name +
                "\r\nDamage:" +gl.Data[i].bullet_damage.ToString() +
                "\r\nRate:" + gl.Data[i].rapid_fire_rate.ToString() +
                "\r\nloaded_bullets:" + gl.Data[i].loaded_bullets.ToString() +
                "\r\nreload_speed:" + gl.Data[i].reload_speed.ToString() +
                "\r\nRange:" + gl.Data[i].bullet_range.ToString() +
                "\r\nSpeed:" + gl.Data[i].bullet_speed.ToString();

            Button_Equip Wbutton = wp.transform.Find("EQUIP").gameObject.GetComponent<Button_Equip>();
            Wbutton.Enum = i;
        }
    }
    public void Wepon_creating()
    {

    }
}
