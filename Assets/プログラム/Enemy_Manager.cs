using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Manager : MonoBehaviour
{
    public bool isEnemies_death;
    public int enemies_count;
    void Start()
    {
        enemies_count = transform.childCount;
    }
    void Update()
    {
        
    }
    public void ParentEnemyDeath()
    {
        enemies_count--;
        if (enemies_count == 0)
        {
            isEnemies_death = true;
            transform.parent.GetComponent<GameManager>().GameClear();
        }
    }
}
