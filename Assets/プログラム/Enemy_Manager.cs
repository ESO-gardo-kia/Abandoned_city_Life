using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Manager : MonoBehaviour
{
    static public bool enemies_move_permit;
    private GameManager gm;
    public int enemies_count;
    void Start()
    {
        gm = transform.parent.GetComponent<GameManager>();
    }
    public void Manager_Setting(int num)
    {
        enemies_count = gm.si.data[gm.stage_number].enemies_num[num];
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
