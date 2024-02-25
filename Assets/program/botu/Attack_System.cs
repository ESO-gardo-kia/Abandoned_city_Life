using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_System : MonoBehaviour
{
    public enum Attack_Subject
    {
        Player,
        Enemy,
    }
    public Attack_Subject attack_subject = Attack_Subject.Player;
    public bool isAttack = false;
    public float damage;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Attack_On()
    {
        isAttack = true;
    }
    void Attack_Off()
    {
        isAttack = false;
    }
}
