using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
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
    public string target_tag;
    public float damage;
    public float speed;
    public Vector3 dire;
    public float death_dis = 1;
    //Normal
    public Vector3 firstpos;
    //following
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
                if (Vector3.Distance(firstpos, transform.position) >= death_dis) Destroy(gameObject);
                transform.localRotation = Quaternion.RotateTowards(transform.rotation
                    , Quaternion.LookRotation(targetobj.transform.position - transform.position)
                    , 10);
                break;
            case Bullet_Type.parabola:
                if(transform.position.y >= target_pos.y)
                {
                    Debug.Log("トウタツ");
                    cluster_Down(30);
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
        Quaternion[] vec = new Quaternion[num];
        for(int i = 0;i < vec.Length; i++)
        {
            Debug.Log("分裂");
            vec[i] = new Quaternion(Random.Range(-180, 180)
                                , Random.Range(-180, 180)
                                , Random.Range(-180, 180)
                                ,0);
            GameObject shotObj = Instantiate(SPLITOBJ, transform.position, vec[i]);
            Rigidbody rb = shotObj.GetComponent<Rigidbody>();
            Bullet_System bs = shotObj.GetComponent<Bullet_System>();

            bs.type = Bullet_Type.split;
            bs.target_tag = "Player";
            bs.damage = damage;
            bs.shot_power = shot_power;
            bs.death_dis = 100;
            bs.firstpos = transform.position;
            bs.target_pos = target_pos;

            rb.velocity = shotObj.transform.forward * Random.Range(0, 10);
        }
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            Destroy(gameObject);
        }
    }
}
