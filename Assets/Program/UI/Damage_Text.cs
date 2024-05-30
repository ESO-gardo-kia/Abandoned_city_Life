using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage_Text : MonoBehaviour
{
    public float death_time = 1;
    void Start()
    {
        
    }
    void FixedUpdate()
    {
        if (death_time <= 0) Destroy(gameObject);
        else death_time -= 0.02f;
    }
    public void TextReset()
    {
        death_time = 1;
    }
}
