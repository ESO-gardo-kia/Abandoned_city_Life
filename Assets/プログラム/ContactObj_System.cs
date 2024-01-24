using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ContactObj_System : MonoBehaviour
{
    public enum Contact_Type{StageSelect,Wepon_by}
    public Contact_Type Cont;
    public string contact_text;

    public void Contact_function()
    {
        switch (Cont)
        {
            case Contact_Type.StageSelect:
                
                break;
            case Contact_Type.Wepon_by:

                break;
        }
    }
}
