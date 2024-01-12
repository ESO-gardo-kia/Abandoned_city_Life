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
    [SerializeField] public GameObject Player;
    public GameObject Enemy_Obj;

    private GameObject EnemyCanvas;
    private UnityEngine.UI.Slider HPSlider;
    [SerializeField] public GameObject TEXTOBJ;
    private GameObject TEXTPOS;
    public GameObject old_dt;
    public float old_damage;

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
        Enemy_Obj = transform.Find("Enemy_Obj").gameObject;
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
        //EnemyCanvas.transform.eulerAngles += Vector3.down * 180;
        //EnemyCanvas.transform.eulerAngles += Vector3.right * 90;

        HPSlider.value = currenthp;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            if (old_dt == null)
            {
                GameObject dt = Instantiate(TEXTOBJ, TEXTPOS.transform.position, HPSlider.transform.rotation, transform.Find("EnemyCanvas"));
                UnityEngine.UI.Text t = dt.GetComponent<UnityEngine.UI.Text>();
                Bullet_System bs = other.GetComponent<Bullet_System>();
                Damage_Text dts = dt.GetComponent<Damage_Text>();
                Debug.Log("‹N“®1");
                old_dt = dt;

                t.text = bs.damage.ToString();
                old_damage = bs.damage;
            }
            else if(old_dt != null)
            {
                Debug.Log("‹N“®2");
                old_damage += other.GetComponent<Bullet_System>().damage;
                old_dt.GetComponent<UnityEngine.UI.Text>().text = old_damage.ToString();
                old_dt.GetComponent<Damage_Text>().TextReset();
            }
            TakeDmage(other.GetComponent<Bullet_System>().damage);
        }
    }
    void TakeDmage(float damage)
    {
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
    void Deathfunction()
    {
        Destroy(gameObject);
    }
}
