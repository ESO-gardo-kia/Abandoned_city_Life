using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static Enemy_List;
using static System.Net.Mime.MediaTypeNames;

public class Enemy_System : MonoBehaviour
{
    [SerializeField] private Enemy_Manager em;
    [SerializeField] public GameObject Player;
    private GameObject Enemy_Obj;
    private GameObject Attack_Obj;

    private GameObject EnemyCanvas;
    private UnityEngine.UI.Slider HPSlider;
    [SerializeField] public GameObject TEXTOBJ;
    private GameObject TEXTPOS;
    private GameObject old_dt;
    private float old_damage;

    public NavMeshAgent navMeshAgent;
    [SerializeField] private Enemy_List enemy_List;
    private int enemy_number;
    private bool isdeath;

    private string name;
    private float exp;

    private float hp;
    public float currenthp;
    private float atk;
    public float currentatk;
    private float agi;
    public float currentagi;

    void Start()
    {
        em = transform.parent.GetComponent<Enemy_Manager>();
        Player = GameObject.Find("Player");
        Enemy_Obj = transform.Find("Enemy_Obj").gameObject;
        Attack_Obj = transform.Find("Enemy_Obj/Attack_Obj").gameObject;
        TEXTPOS = transform.Find("TEXTPOS").gameObject;
        EnemyCanvas = transform.Find("EnemyCanvas").gameObject;
        HPSlider = transform.Find("EnemyCanvas/HPSlider").gameObject.GetComponent<UnityEngine.UI.Slider>();
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        isdeath = true;
        var e_l = enemy_List.Status[1];
        name = e_l.name;
        exp = e_l.exp;
        hp = e_l.hp;
        atk = e_l.atk;
        agi =  e_l.agi;
        currenthp = hp;
        currentatk = atk;
        currentagi = agi;
        HPSlider.maxValue = hp;
    }

    void FixedUpdate()
    {
        if (!isdeath)
        {
            Deathfunction();
        }
        else
        {
            navMeshAgent.destination = Player.transform.position;
        }
        
        EnemyCanvas.transform.LookAt(Player.transform, Vector3.down * 180);

        HPSlider.value = currenthp;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            TakeDmage(other.GetComponent<Bullet_System>().damage,other, other.GetComponent<Bullet_System>());
        }
        if (other.gameObject.CompareTag("Attack_Obj"))
        {
            //攻撃出来る状態かつ攻撃対象はエネミーの時
            if (other.GetComponent<Attack_System>().isAttack && other.GetComponent<Attack_System>().attack_subject == Attack_System.Attack_Subject.Enemy)
            {
                TakeDmage(other.GetComponent<Attack_System>().damage, other, other.GetComponent<Attack_System>());
            }
            
        }
    }
    void TakeDmage(float damage ,Collider other , Bullet_System BS)
    {
        if (old_dt == null)
        {
            GameObject dt = Instantiate(TEXTOBJ, TEXTPOS.transform.position, HPSlider.transform.rotation, transform.Find("EnemyCanvas"));
            UnityEngine.UI.Text t = dt.GetComponent<UnityEngine.UI.Text>();
            //Bullet_System bs = other.GetComponent<Bullet_System>();
            Damage_Text dts = dt.GetComponent<Damage_Text>();
            old_dt = dt;

            t.text = BS.damage.ToString();
            old_damage = BS.damage;
        }
        else if (old_dt != null)
        {
            old_damage += BS.damage;
            old_dt.GetComponent<UnityEngine.UI.Text>().text = old_damage.ToString();
            old_dt.GetComponent<Damage_Text>().TextReset();
        }
        if (currenthp > 0)
        {
            Debug.Log(damage);
            currenthp -= damage;
        }
        if(currenthp <= 0)
        {
            isdeath = false;
        }
    }
    void TakeDmage(float damage, Collider other, Attack_System AS)
    {
        if (old_dt == null)
        {
            GameObject dt = Instantiate(TEXTOBJ, TEXTPOS.transform.position, HPSlider.transform.rotation, transform.Find("EnemyCanvas"));
            UnityEngine.UI.Text t = dt.GetComponent<UnityEngine.UI.Text>();
            Bullet_System bs = other.GetComponent<Bullet_System>();
            Damage_Text dts = dt.GetComponent<Damage_Text>();
            old_dt = dt;

            t.text = bs.damage.ToString();
            old_damage = bs.damage;
        }
        else if (old_dt != null)
        {
            old_damage += other.GetComponent<Bullet_System>().damage;
            old_dt.GetComponent<UnityEngine.UI.Text>().text = old_damage.ToString();
            old_dt.GetComponent<Damage_Text>().TextReset();
        }
        if (currenthp > 0)
        {
            Debug.Log(damage);
            currenthp -= damage;
        }
        if (currenthp <= 0)
        {
            isdeath = false;
        }
    }
    void Deathfunction()
    {
        em.ParentEnemyDeath();
        Destroy(gameObject);
    }
}
