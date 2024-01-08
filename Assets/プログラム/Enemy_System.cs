using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class Enemy_System : MonoBehaviour
{
    [SerializeField] public GameObject Player;
    public GameObject Enemy_Obj;

    private GameObject EnemyCanvas;
    private Slider HPSlider;
    [SerializeField] public GameObject TEXTOBJ;
    private GameObject TEXTPOS;
    private GameObject old_dt;
    private float old_damage;

    private NavMeshAgent navMeshAgent;


    void Start()
    {
        Enemy_Obj = transform.Find("Enemy_Obj").gameObject;
        TEXTPOS = transform.Find("TEXTPOS").gameObject;
        EnemyCanvas = transform.Find("EnemyCanvas").gameObject;
        HPSlider = transform.Find("EnemyCanvas/HPSlider").gameObject.GetComponent<Slider>();

        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void FixedUpdate()
    {
        navMeshAgent.destination = Player.transform.position;
        EnemyCanvas.transform.LookAt(Player.transform);
        EnemyCanvas.transform.eulerAngles += Vector3.down * 180;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            if (old_dt == null)
            {
                GameObject dt = Instantiate(TEXTOBJ, TEXTPOS.transform.position, EnemyCanvas.transform.rotation, transform.Find("EnemyCanvas"));
                UnityEngine.UI.Text t = dt.GetComponent<UnityEngine.UI.Text>();
                Bullet_System bs = other.GetComponent<Bullet_System>();
                Damage_Text dts = dt.GetComponent<Damage_Text>();
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
        }
    }
}
