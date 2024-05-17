using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBulletSystem : MonoBehaviour
{
    [SerializeField] Rigidbody rigidBody;
    [SerializeField] Gun_List gunList;
    public enum BulletType
    {
        Normal,
        Following,
        Parabola,
        Split,
    }
    public BulletType bulletType;
    public GameObject hitParticle;
    public string targetTag;
    public float bulletDamage;
    public float bulletSpeed;
    public float deathDistance = 1;
    public Vector3 firstPosition;
    //Normal
    public Vector3 firstpos;
    void FixedUpdate()
    {
        if (Vector3.Distance(firstpos, transform.position) >= deathDistance)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            Instantiate(hitParticle, transform.position, Quaternion.identity, transform.transform.parent = null);
            Destroy(gameObject);
        }
        if (targetTag == "Player" && other.gameObject.CompareTag("Player"))
        {
            Instantiate(hitParticle, transform.position, Quaternion.identity, transform.transform.parent = null);
            Destroy(gameObject);
        }
        if (targetTag == "Enemy" && other.gameObject.CompareTag("Enemy"))
        {
            Instantiate(hitParticle, transform);
        }
    }
}
