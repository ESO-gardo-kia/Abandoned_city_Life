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
    public enum Bullet_Type
    {
        Normal,
        following,
        parabola,
        split,
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
    //following
    public bool isfollow = true;
    public GameObject targetobj;
    //parabola
    public float shot_power;
    public Vector3 target_pos;
    public GameObject SPLITOBJ;
    void FixedUpdate()
    {
        switch(type)
        {
            case Bullet_Type.Normal:
                if(Vector3.Distance(firstpos, transform.position) >= death_dis) Destroy(gameObject);
                break;
            case Bullet_Type.following:
                rb.velocity = transform.forward * speed;
                if (isfollow && Vector3.Distance(targetobj.transform.position, transform.position) < 7)
                {
                    Debug.Log("追尾解除");
                    isfollow = false;
                }
                if (isfollow)
                {
                    if (Vector3.Distance(firstpos, transform.position) >= death_dis) Destroy(gameObject);
                    transform.localRotation = Quaternion.RotateTowards(transform.rotation
                        , Quaternion.LookRotation((targetobj.transform.position + Vector3.up) - transform.position)
                        , 10);
                }

                break;
            case Bullet_Type.parabola:
                if(transform.position.y >= target_pos.y)
                {
                    Debug.Log("トウタツ");
                    cluster_Down(10);
                }
                break;
            case Bullet_Type.split:
                float dis = Vector3.Distance(firstpos, transform.position);
                if (Vector3.Distance(firstpos, transform.position) >= death_dis) Destroy(gameObject);
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
            Rigidbody rb = shotObj.GetComponent<Rigidbody>();
            Bullet_System bs = shotObj.GetComponent<Bullet_System>();

            shotObj.transform.localEulerAngles = Vector3.right * 90;
            vec[i] = new Vector3(Random.Range(-20, 20)
                    , Random.Range(-90, 90)
                    , Random.Range(-90, 90));
            //shotObj.transform.LookAt(targetobj.transform);
            shotObj.transform.localEulerAngles += vec[i];

            bs.type = Bullet_Type.split;
            bs.target_tag = "Player";
            bs.damage = damage;
            bs.shot_power = shot_power;
            bs.death_dis = 100;
            bs.firstpos = transform.position;
            bs.target_pos = target_pos;

            rb.velocity = shotObj.transform.forward * 80;
        }
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("Floor"))
        {
            GameObject par = Instantiate(hitparticle,transform.position ,Quaternion.identity,transform.transform.parent = null);
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
        GameObject par = Instantiate(hitparticle, transform.position, Quaternion.identity, transform.transform.parent = null);
        Destroy(gameObject);
    }
}
