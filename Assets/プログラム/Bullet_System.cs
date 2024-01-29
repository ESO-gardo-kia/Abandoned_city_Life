using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet_System : MonoBehaviour
{
    public enum Bullet_Type
    {
        Normal
    }
    public Bullet_Type type;
    public Vector3 firstpos;
    public string target_tag;
    public float death_dis = 1;
    public float damage;
    void FixedUpdate()
    {
        switch(type)
        {
            case Bullet_Type.Normal:
                float dis = Vector3.Distance(firstpos,transform.position);
                if(dis >= death_dis) Destroy(gameObject);
                /*
                if (death_time <= 0) Destroy(gameObject);
                else death_time -= 0.02f;
                */
                break;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        /*
        if(target_tag == "Enemy")
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                //Debug.Log(other.gameObject.tag);
                Destroy(gameObject);
            }
            else if (!other.gameObject.CompareTag("Player"))
            {
                //Debug.Log(other.gameObject.tag);
            }
        }
        if (target_tag == "Player")
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                //Debug.Log(other.gameObject.tag);
            }
            else if (!other.gameObject.CompareTag("Player"))
            {
                //Debug.Log(other.gameObject.tag);
                Destroy(gameObject);
            }
        }
        */
    }
}
