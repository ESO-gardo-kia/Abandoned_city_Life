using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugUiSystem : MonoBehaviour
{
    public Text mouseMoveNum;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        mouseMoveNum.text = "X"+Input.GetAxis("Mouse X").ToString() +
            "Y"+Input.GetAxis("Mouse Y").ToString();
    }
}
