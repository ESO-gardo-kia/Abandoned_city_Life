using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_System : MonoBehaviour
{
    public float death_time = 1;
    public float damage;
    void FixedUpdate()
    {
        if(death_time <= 0) Destroy(gameObject);
        else death_time -= 0.02f;
    }
    private void OnCollisionEnter(Collision collision)
    {
        //if (gameObject.CompareTag("Player") == false) Destroy(gameObject);
    }
}
