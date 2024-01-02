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
    void Start()
    {
        TEXTPOS = transform.Find("TEXTPOS").gameObject;
    }

    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            GameObject dt = Instantiate(TEXTOBJ, TEXTPOS.transform.position, Quaternion.identity,transform.Find("EnemyCanvas"));
            UnityEngine.UI.Text t = dt.GetComponent<UnityEngine.UI.Text>();
            Bullet_System bs = other.GetComponent<Bullet_System>();

            t.text = bs.damage.ToString();
        }
    }
}
