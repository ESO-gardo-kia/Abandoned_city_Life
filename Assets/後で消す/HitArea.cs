using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitArea : MonoBehaviour
{
    [SerializeField] GameObject hitArea;
    [SerializeField] Rigidbody body;
    private Vector3 oldposition;
    public int radius = 6;
    public bool isBouns;
    public int bounsPower;
    private void Start()
    {
    }
    void Update()
    {

        if (transform.position.x * transform.position.x + transform.position.z * transform.position.z < radius * radius)
        {
            if (isBouns)
            {
                body.AddForce((transform.position).normalized * bounsPower, ForceMode.Impulse);
            }
            else
            {
                transform.position = oldposition;
            }
        }
        oldposition = transform.position;
    }
}
