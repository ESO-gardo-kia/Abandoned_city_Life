using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parabola : MonoBehaviour
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
    public float shot_power;
    public Vector3 target_pos;
    public GameObject SPLITOBJ;
    void Update()
    {
        if (transform.position.y >= target_pos.y)
        {
            Debug.Log("目標地点到達");
            cluster_Down(15);
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
