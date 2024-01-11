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
    private GameObject old_dt;
    private float old_damage;

    public NavMeshAgent navMeshAgent;
    [SerializeField] public Enemy_List enemy_List;
    public int enemy_number;
    public bool isdeath;

    public string name;
    public float exp;

    public float hp;
    public float currenthp;
    public float atk;
    public float currentatk;
    public float agi;
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
            
        }
        else
        {
            navMeshAgent.destination = Player.transform.position;
        }
        
        EnemyCanvas.transform.LookAt(Player.transform);
        EnemyCanvas.transform.eulerAngles += Vector3.down * 180;

        HPSlider.value = currenthp;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            GameObject dt = Instantiate(TEXTOBJ, TEXTPOS.transform.position, EnemyCanvas.transform.rotation, transform.Find("EnemyCanvas"));
            UnityEngine.UI.Text t = dt.GetComponent<UnityEngine.UI.Text>();
            Bullet_System bs = other.GetComponent<Bullet_System>();
            Damage_Text dts = dt.GetComponent<Damage_Text>();
            if (old_dt == null)
            {
                old_dt = dt;

                t.text = bs.damage.ToString();
                old_damage = bs.damage;
            }
            else if(old_dt != null)
            {
                old_damage += other.GetComponent<Bullet_System>().damage;
                old_dt.GetComponent<UnityEngine.UI.Text>().text = old_damage.ToString();
                old_dt.GetComponent<Damage_Text>().TextReset();
            }
            TakeDmage(bs.damage);
        }
    }
    void TakeDmage(float damage)
    {
        if (currenthp > 0)
        {
            currenthp -= damage;
        }
    }
}
