using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using static Gun_List;
using static Unity.VisualScripting.Member;

public class Bullet_System : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] Gun_List gunlist;
    public enum BulletType
    {
        Normal,
        Following,
        Parabola,
        Split,
    }
    public BulletType bulletType;
    public GameObject hitparticle;
    public string target_tag;
    public float damage;
    public float bulletSpeed;
    public float deathDistance = 1;
    //Normal
    public Vector3 firstpos;
    //following
    public bool isfollow = true;
    public GameObject targetobj;
    //parabola
    public float shot_power;
    public Vector3 target_pos;
    public GameObject SPLITOBJ;
    void FixedUpdate()
    {
        switch(bulletType)
        {
            case BulletType.Normal:
                if(Vector3.Distance(firstpos, transform.position) >= deathDistance) Destroy(gameObject);
                break;
            case BulletType.Following:
                rb.velocity = transform.forward * bulletSpeed;
                if (isfollow && Vector3.Distance(targetobj.transform.position, transform.position) < 4)
                {
                    Debug.Log("追尾解除");
                    isfollow = false;
                }
                if (isfollow)
                {
                    if (Vector3.Distance(firstpos, transform.position) >= deathDistance) Destroy(gameObject);
                    transform.localRotation = Quaternion.RotateTowards(transform.rotation
                        , Quaternion.LookRotation((targetobj.transform.position + Vector3.up) - transform.position)
                        , 10);
                }

                break;
            case BulletType.Parabola:
                if(transform.position.y >= target_pos.y)
                {
                    Debug.Log("目標地点到達");
                    cluster_Down(15);
                }
                break;
            case BulletType.Split:
                float dis = Vector3.Distance(firstpos, transform.position);
                if (Vector3.Distance(firstpos, transform.position) >= deathDistance) Destroy(gameObject);
                break;
        }
    }
    //分裂して
    public void cluster_Down(int num)
    {
        Vector3[] vec = new Vector3[num];
        for(int i = 0;i < vec.Length; i++)
        {
            GameObject shotObj = Instantiate(SPLITOBJ, transform.position, Quaternion.identity);
            Rigidbody rigitbody = shotObj.GetComponent<Rigidbody>();
            Bullet_System bulletsystem = shotObj.GetComponent<Bullet_System>();

            shotObj.transform.localEulerAngles = Vector3.right * 90;
            vec[i] = new Vector3(Random.Range(-20, 20)
                    , Random.Range(-90, 90)
                    , Random.Range(-90, 90));
            //shotObj.transform.LookAt(targetobj.transform);
            shotObj.transform.localEulerAngles += vec[i];

            bulletsystem.bulletType = BulletType.Split;
            bulletsystem.target_tag = "Player";
            bulletsystem.damage = damage;
            bulletsystem.shot_power = shot_power;
            bulletsystem.deathDistance = 100;
            bulletsystem.firstpos = transform.position;
            bulletsystem.target_pos = target_pos;

            rigitbody.velocity = shotObj.transform.forward * 80;
        }
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            Instantiate(hitparticle,transform.position ,Quaternion.identity,transform.transform.parent = null);
            Destroy(gameObject);
        }
        if(target_tag == "Player" && other.gameObject.CompareTag("Player"))
        {
            Instantiate(hitparticle, transform);
            Destroy(gameObject);
        }
        if (target_tag == "Enemy" && other.gameObject.CompareTag("Enemy"))
        {
            Instantiate(hitparticle, transform);
        }
    }
    public void BulletDestroy()
    {
        Instantiate(hitparticle, transform.position, Quaternion.identity, transform.transform.parent = null);
        Destroy(gameObject);
    }
}
