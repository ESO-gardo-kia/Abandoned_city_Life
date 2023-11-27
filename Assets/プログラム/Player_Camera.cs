using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Camera : MonoBehaviour
{
    public float view_speed = 2;
    private Vector3 past_pos;
    void Update()
    {
        if (past_pos != Input.mousePosition)
        {
            transform.eulerAngles += new Vector3(Input.mousePosition.y - past_pos.y / view_speed,
                                            Input.mousePosition.x - past_pos.x / view_speed,
                                            0);
            past_pos = Input.mousePosition;
        }


    }
}
