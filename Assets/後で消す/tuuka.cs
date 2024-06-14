using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tuuka : MonoBehaviour
{
    [SerializeField] GameObject player;

    Vector3 playerOldPos = Vector3.zero;
    Vector3 playerCurrentPos = Vector3.zero;
    Vector3 direction = Vector3.zero;
    float radius = 1;

    float dot0;
    float dot1;
    void Start()
    {
        direction = transform.up;
    }
    void Update()
    {
        if (Vector3Multiplication(player.transform.position, transform.position) < radius*radius)
        {
            dot0 = Vector3.Dot(playerOldPos - transform.position, direction);
            dot1 = Vector3.Dot(player.transform.position - transform.position, direction);
            if (dot0 <= 0 && dot1 > 0)
            {
                Debug.Log("’Ê‰ß");
                player.GetComponent<Renderer>().material.color = Color.red;
            }
        }
        if (player.transform.position.x < -5)
        {
            Debug.Log("–ß‚Á‚½");
            player.GetComponent<Renderer>().material.color = Color.green;
        }
    }
    float  Vector3Multiplication(Vector3 a, Vector3 b)
    {
        return a.x*b.x+a.y*b.y+a.z*b.z;
    }
}
