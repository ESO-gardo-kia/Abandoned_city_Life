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
        parabola
    }
    public Bullet_Type type;
    public string target_tag;
    public float damage;
    public float speed;
    public Vector3 dire;
    //Normal
    public float death_dis = 1;
    public Vector3 firstpos;
    //parabola
    public Vector3 target_pos; 
    void FixedUpdate()
    {
        switch(type)
        {
            case Bullet_Type.Normal:
                //rb.velocity = dire * speed;
                float dis = Vector3.Distance(firstpos,transform.position);
                if(dis >= death_dis) Destroy(gameObject);
                /*
                if (death_time <= 0) Destroy(gameObject);
                else death_time -= 0.02f;
                */
                break;
            case Bullet_Type.parabola:
                //CalculateVelocity();
                break;
        }
    }
    /// <summary>
    /// 標的に命中する射出速度の計算
    /// </summary>
    /// <param name="pointA">射出開始座標</param>
    /// <param name="pointB">標的の座標</param>
    /// <returns>射出速度</returns>
    private Vector3 CalculateVelocity(Vector3 pointA, Vector3 pointB, float angle)
    {
        // 射出角をラジアンに変換
        float rad = angle * Mathf.PI / 180;
        // 水平方向の距離x
        float x = Vector2.Distance(new Vector2(pointA.x, pointA.z), new Vector2(pointB.x, pointB.z));
        // 垂直方向の距離y
        float y = pointA.y - pointB.y;
        // 斜方投射の公式を初速度について解く
        float speed = Mathf.Sqrt(-Physics.gravity.y * Mathf.Pow(x, 2) / (2 * Mathf.Pow(Mathf.Cos(rad), 2) * (x * Mathf.Tan(rad) + y)));
        if (float.IsNaN(speed))
        {
            // 条件を満たす初速を算出できなければVector3.zeroを返す
            return Vector3.zero;
        }
        else
        {
            return (new Vector3(pointB.x - pointA.x, x * Mathf.Tan(rad), pointB.z - pointA.z).normalized * speed);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        /*
        if(target_tag == "Enemy")
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                //Debug.Log(other.gameObject.tag);
                Destroy(gameObject);
            }
            else if (!other.gameObject.CompareTag("Player"))
            {
                //Debug.Log(other.gameObject.tag);
            }
        }
        if (target_tag == "Player")
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                //Debug.Log(other.gameObject.tag);
            }
            else if (!other.gameObject.CompareTag("Player"))
            {
                //Debug.Log(other.gameObject.tag);
                Destroy(gameObject);
            }
        }
        */
    }
    /// <summary>
    /// 弾を発射する
    /// </summary>
    public void NomalShot(string targettag,int weapon_id ,GameObject SHOTPOS , GameObject SHOTOBJ,Vector3 direction)
    {
        target_tag = targettag;
        firstpos = SHOTPOS.transform.position;
        dire = direction;
        var Guns = gunlist.Data;
        damage = Guns[weapon_id].bullet_damage;
        death_dis = Guns[weapon_id].bullet_range / 1.5f;
        speed = Guns[weapon_id].bullet_speed;

        GameObject shotObj = Instantiate(SHOTOBJ, SHOTPOS.transform.position, Quaternion.identity);
        //rb.velocity = transform.forward * Guns[weapon_id].bullet_speed;
        shotObj.transform.eulerAngles = this.transform.eulerAngles;
        //shotObj.transform.eulerAngles = this.transform.eulerAngles + new Vector3(0, 0, -90);
    }
}
