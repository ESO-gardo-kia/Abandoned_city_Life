using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class Enemy_System : MonoBehaviour
{
    [SerializeField] public GameObject EnemyCanvas;
    [SerializeField] public GameObject TEXTOBJ;
    private GameObject TEXTPOS;

    private GameObject obj;
    private float edamage;
    
    void Start()
    {
        TEXTPOS = transform.Find("TEXTPOS").gameObject;
        EnemyCanvas = transform.Find("EnemyCanvas").gameObject;
    }

    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            if (obj == null)
            {
                GameObject dt = Instantiate(TEXTOBJ, TEXTPOS.transform.position, Quaternion.identity, transform.Find("EnemyCanvas"));
                UnityEngine.UI.Text t = dt.GetComponent<UnityEngine.UI.Text>();
                Bullet_System bs = other.GetComponent<Bullet_System>();
                obj = dt;

                Debug.Log(bs.damage);
                t.text = bs.damage.ToString();
                edamage = bs.damage;
            }
            else if(obj != null)
            {
                edamage += other.GetComponent<Bullet_System>().damage;
                obj.GetComponent<UnityEngine.UI.Text>().text = edamage.ToString();
                obj.GetComponent<Damage_Text>().TextReset();
            }
        }
    }
}
