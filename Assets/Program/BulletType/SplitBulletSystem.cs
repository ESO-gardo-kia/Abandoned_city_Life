using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitBulletSystem : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidBody;
    public GameObject hitParticle;
    public string targetTag;
    public float bulletDamage;
    public float bulletSpeed;
    public float deathDistance;
    public Vector3 firstPosition;
    void Update()
    {
        rigidBody.velocity = (transform.forward * bulletSpeed) * Time.deltaTime * 10;
        if (Vector3.Distance(firstPosition, transform.position) >= deathDistance) Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Floor"))
        {
            BulletDestroy();
        }
        if (targetTag == "Player" && other.gameObject.CompareTag("Player"))
        {
            BulletDestroy();
        }
        if (targetTag == "Enemy" && other.gameObject.CompareTag("Enemy"))
        {
            BulletDestroy();
        }
    }
    public void BulletDestroy()
    {
        Instantiate(hitParticle, transform.position, Quaternion.identity, transform.transform.parent = null);
        Destroy(gameObject);
    }
}
