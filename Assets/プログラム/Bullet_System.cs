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
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log(other.gameObject.tag);
        }
        else if (!other.gameObject.CompareTag("Player"))
        {
            Debug.Log(other.gameObject.tag);
            //Destroy(gameObject);
        }
    }
}
