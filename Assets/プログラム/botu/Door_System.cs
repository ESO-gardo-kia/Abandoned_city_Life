using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Door_System : MonoBehaviour
{
    [SerializeField] private UnityEvent myEvent = new UnityEvent();
    public string contact_text;
    void Start()
    {
        
    }
    public void TestFunc()
    {
        myEvent.Invoke();
    }

}
