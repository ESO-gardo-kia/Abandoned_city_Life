using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolaBulletSystem : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidBody;
    public GameObject hitParticle;
    public string targetTag;
    public float bulletDamage;
    public float bulletSpeed;
    public float deathDistance;
    public int ClusterSplitAmount;
    public Vector3 firstPosition;
    public Vector3 targetPosition;
    public GameObject SPLITOBJ;
    void Update()
    {
        rigidBody.velocity = (transform.forward * bulletSpeed) * Time.deltaTime * 10;
        if (Vector3.Distance(firstPosition, transform.position) >= deathDistance)
        {
            BulletDestroy();
        }
    }
    public void cluster_Down(int num)
    {
        Vector3[] vec = new Vector3[num];
        for (int i = 0; i < vec.Length; i++)
        {
            GameObject shotObj = Instantiate(SPLITOBJ, transform.position, Quaternion.identity);
            Rigidbody rigitbody = shotObj.GetComponent<Rigidbody>();
            SplitBulletSystem bulletsystem = shotObj.GetComponent<SplitBulletSystem>();

            shotObj.transform.localEulerAngles = Vector3.right * 90;
            vec[i] = new Vector3(Random.Range(-20, 20)
                    , Random.Range(-90, 90)
                    , Random.Range(-90, 90));
            //shotObj.transform.LookAt(targetobj.transform);
            shotObj.transform.localEulerAngles += vec[i];

            bulletsystem.targetTag = "Player";
            bulletsystem.bulletDamage = bulletDamage;
            bulletsystem.deathDistance = 100;
            bulletsystem.firstPosition = transform.position;

            rigitbody.velocity = shotObj.transform.forward * 80;
        }
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
        cluster_Down(ClusterSplitAmount);
        Instantiate(hitParticle, transform.position, Quaternion.identity, transform.transform.parent = null);
        Destroy(gameObject);
    }
}
