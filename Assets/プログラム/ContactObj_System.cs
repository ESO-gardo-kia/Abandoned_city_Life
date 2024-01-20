using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ContactObj_System : MonoBehaviour
{
    public enum Condition{Door,Clear,Title,Select,Main,Replay,End}
    public Condition Cond;
    public string contact_text;

    public void Contact_function()
    {
        switch (Cond)
        {
            case Condition.Door:
                
                break;
            case Condition.Clear:
                
                break;
            case Condition.Title:
                
                break;
            case Condition.Select:
                
                break;
            case Condition.Main:
                
                break;
            case Condition.Replay:
                
                break;
        }
    }
}
