using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBulletSystem : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] Gun_List gunlist;
    public enum Bullet_Type
    {
        Normal,
        Following,
        Parabola,
        Split,
    }
    public Bullet_Type type;
    public GameObject hitparticle;
    public string target_tag;
    public float damage;
    public float speed;
    public Vector3 dire;
    public float death_dis = 1;
    //Normal
    public Vector3 firstpos;
    void FixedUpdate()
    {
        if (Vector3.Distance(firstpos, transform.position) >= death_dis)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Floor"))
        {
            Instantiate(hitparticle, transform.position, Quaternion.identity, transform.transform.parent = null);
            Destroy(gameObject);
        }
        if (target_tag == "Player" && other.gameObject.CompareTag("Player"))
        {
            Instantiate(hitparticle, transform.position, Quaternion.identity, transform.transform.parent = null);
            Destroy(gameObject);
        }
        if (target_tag == "Enemy" && other.gameObject.CompareTag("Enemy"))
        {
            Instantiate(hitparticle, transform);
        }
    }
}
