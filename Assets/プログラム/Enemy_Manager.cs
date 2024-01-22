using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Manager : MonoBehaviour
{
    static public bool enemies_move_permit;
    [SerializeField] private GameObject Enemy_Obj;
    [SerializeField] private GameObject Player;
    public GameObject[] SPL;//SpawnPoint_List
    public int spawn_range = 20;
    private GameManager gm;
    public int all_enemies_count;
    public int current_enemies_count;
    public bool iscompletion;//今のウェーブが終わったかどうか
    void Start()
    {
        gm = transform.parent.GetComponent<GameManager>();
        SPL = new GameObject[transform.Find("SpawnPoint_List").childCount];
        for(int i = 0; i < transform.Find("SpawnPoint_List").childCount; i++)
        {
            SPL[i] = transform.Find("SpawnPoint_List").GetChild(i).gameObject;
        }
    }
    private void Update()
    {
        int[] ints = new int[1];
        if (Input.GetKey(KeyCode.Q)) Enemies_Spawn_Function(ints);
    }
    public IEnumerator Enemies_Spawn_Function(int[] num)
    {
        iscompletion = false;
        foreach (var i in num) all_enemies_count += i;
        for(int wave_num = 0; wave_num < num.Length; wave_num++)
        {
            current_enemies_count += num[wave_num];
            Debug.Log("ウェーブ"+(wave_num + 1)+"開始");
            for (int i2 = 0
                ; i2 < num[wave_num] 
                ; i2++)
            {
                Debug.Log("敵出現");
                Vector3 sp = SPL[Random.Range(0, SPL.Length)].transform.position + new Vector3(
                 Random.Range(-spawn_range, spawn_range)
               , 0f
               , Random.Range(-spawn_range, spawn_range));

                GameObject eo = Instantiate(Enemy_Obj, sp, Quaternion.identity, transform.parent = transform);
                eo.GetComponent<Enemy_System>().Player = Player;
                current_enemies_count--;
                yield return new WaitForSeconds(0.5f);
            }
            if (current_enemies_count != 0)
            { 
            }
        }


        Enemy_Manager.enemies_move_permit = true;
    }
    public void ParentEnemyDeath()
    {
        current_enemies_count--;
        all_enemies_count--;
        if (all_enemies_count == 0)
        {
            transform.parent.GetComponent<GameManager>().GameClear();
        }
    }
    public void Enemy_Manager_Reset()
    {
        /*
        for (int i = 0; i < transform.GetChild(); i++)
        {

        }
        */
        enemies_move_permit = false;
        all_enemies_count = 0;
        current_enemies_count = 0;
        iscompletion = false;
    }
}
