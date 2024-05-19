using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingBulletSystem : MonoBehaviour
{
    [SerializeField] Rigidbody rigidBody;
    public GameObject hitParticle;
    public string targetTag;
    public float bulletDamage;
    public float bulletSpeed;
    public float deathDistance = 1;
    public Vector3 firstPosition;
    public bool isfollowing = true;
    public GameObject targetObj;
    void Update()
    {
        rigidBody.velocity = (transform.forward * bulletSpeed) * Time.deltaTime;
        if (isfollowing && Vector3.Distance(targetObj.transform.position, transform.position) < 4)
        {
            isfollowing = false;
        }
        if (isfollowing)
        {
            if (Vector3.Distance(firstPosition, transform.position) >= deathDistance) Destroy(gameObject);
            transform.localRotation = Quaternion.RotateTowards(transform.rotation
                , Quaternion.LookRotation((targetObj.transform.position + Vector3.up) - transform.position)
                , 10);
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
    public void BulletDestroy()
    {
        Instantiate(hitParticle, transform.position, Quaternion.identity, transform.transform.parent = null);
        Destroy(gameObject);
    }
}
