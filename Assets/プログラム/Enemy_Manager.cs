using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Manager : MonoBehaviour
{
    static public bool enemies_move_permit;
    public GameManager gm;
    public int enemies_count;
    void Start()
    {
        gm = transform.parent.GetComponent<GameManager>();
        enemies_count = gm.si.data[gm.stage_number].enemies_num[0];
    }
    void Update()
    {
        
    }
    public void ParentEnemyDeath()
    {
        enemies_count--;
        if (enemies_count == 0)
        {
            transform.parent.GetComponent<GameManager>().GameClear();
        }
    }
}
