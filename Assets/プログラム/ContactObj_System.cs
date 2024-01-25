using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ContactObj_System : MonoBehaviour
{
    public enum Contact_Type{None,StageSelect,Production_Table }
    public Contact_Type Cont;
    public string contact_text;
    public GameObject Panel;

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
    public void Contact_function()
    {
    }
    public void Button_System(int num)
    {
        GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        gm.Scene_Transition_Process(num);
    }
}
