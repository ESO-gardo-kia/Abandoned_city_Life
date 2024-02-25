using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static Enemy_List;
using static Gun_List;
using static System.Net.Mime.MediaTypeNames;

public class Enemy_Accession : MonoBehaviour
{
    public void NomalShot(GameObject SHOTOBJ, GameObject SHOTPOS, Gun_List gunList, int type)
    {

        GameObject shotObj = Instantiate(SHOTOBJ, SHOTPOS.transform.position, Quaternion.identity);
        Rigidbody rb = shotObj.GetComponent<Rigidbody>();
        Bullet_System bs = shotObj.GetComponent<Bullet_System>();
        var Guns = gunList.Data;

        bs.target_tag = "Player";
        bs.damage = Guns[type].bullet_damage;
        bs.death_dis = Guns[type].bullet_range;
        bs.firstpos = SHOTPOS.transform.position;
        rb.velocity = this.transform.forward * Guns[type].bullet_speed;
        shotObj.transform.eulerAngles = this.transform.eulerAngles;
        //shotObj.transform.eulerAngles = this.transform.eulerAngles + new Vector3(0, 0, -90);
    }
}
