using Cinemachine;
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
    public GameObject idlepos;
    private GameObject[] WPanel_List;
    

    //Production_Table—p
    [SerializeField] public Gun_List gl;

    public CinemachineVirtualCamera VC;

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
        VC.Priority = 100;
        string ObjectsPass = "Canvas/Panel/ScrollView/Viewport/Content";
        int wpn = transform.Find(ObjectsPass).childCount;
        if(wpn != 0) for (int i = 0; i < wpn; i++) Destroy(transform.Find(ObjectsPass).GetChild(i).gameObject);
        for (int i = 0; i < gl.Data.Count; i++)
        {
            GameObject wp = Instantiate(WPanel, Vector3.zero, Quaternion.identity, transform.Find(ObjectsPass));
            wp.name = WPanel.name + i.ToString();
            wp.transform.Find("WeaponImage").GetComponent<Image>().sprite = gl.Data[i].sprite_id;

            Text Wtext = wp.transform.Find("WeaponData").gameObject.GetComponent<Text>();
            if(gl.Data[i].name != null)
            {
                var Guns = gl.Data[i];
                Wtext.text =
                "Name:" + Guns.name +
                "\r\ndamage:" + Guns.bullet_damage.ToString() +
                "\r\nRate:" + Guns.rapid_fire_rate.ToString() +
                "\r\nloaded_bullets:" + Guns.loaded_bullets.ToString() +
                "\r\nreload_speed:" + Guns.reload_speed.ToString() +
                "\r\nRange:" + Guns.bullet_range.ToString() +
                "\r\nSpeed:" + Guns.bullet_speed.ToString() +
                "\r\nmulti_bullet:" + Guns.multi_bullet.ToString() +
                "\r\ndiffusion:" + Guns.diffusion__chance.ToString();
            }
            Button_Equip Wbutton = wp.transform.Find("EQUIP").gameObject.GetComponent<Button_Equip>();
            Wbutton.Enum = i;
        }
    }
    public void Wepon_creating()
    {

    }
}
