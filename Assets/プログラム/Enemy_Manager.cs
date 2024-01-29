using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Manager : MonoBehaviour
{
    static public bool enemies_move_permit;
    public List<int[]> all_wave;
    [SerializeField] private GameObject Enemy_Obj;
    [SerializeField] private GameObject Player;
    [SerializeField] public Enemy_List el;
    public GameObject[] SPL;//SpawnPoint_List
    public int spawn_range = 20;
    private GameManager gm;
    public int current_enemies_count;
    public bool iscompletion;//今のウェーブが終わったかどうか
    public int current_wave = 0;
    void Start()
    {
        gm = transform.parent.GetComponent<GameManager>();
        SPL = new GameObject[transform.Find("SpawnPoint_List").childCount];
        for(int i = 0; i < transform.Find("SpawnPoint_List").childCount; i++)
        {
            SPL[i] = transform.Find("SpawnPoint_List").GetChild(i).gameObject;
        }
    }
    public IEnumerator Enemies_Spawn_Function(int[] wave)
    {
        //wave1[敵のID,敵の数]
        iscompletion = false;
        yield return new WaitForSeconds(1f);
        foreach (var i in wave) current_enemies_count += i;// 現ウェーブの敵の総数を数える
        Debug.Log(current_wave);
        for (int i = 0; i < wave.Length; i++)
        {
            //敵を生成
            if (!Player_System.move_permit && !enemies_move_permit) yield break;
            Spawn_Function(i);
            yield return new WaitForSeconds(0.5f);
        }
        Debug.Log("ウェーブ" + current_wave + "終了");
        if (current_wave == 2) Debug.Log("全ウェーブ終了");
        enemies_move_permit = true;
        current_wave++;//次のウェーブへ数字を進める

        /*
        for(int enemyID = 0; enemyID < wave1.Length; enemyID++)
        {
            if (!Player_System.move_permit&&!enemies_move_permit) yield break;
            current_enemies_count += wave1[enemyID];
            Debug.Log((enemyID + 1)+"出現");

            for (int i2 = 0; i2 < wave1[enemyID] ; i2++)
            {
                Debug.Log("敵出現");

                Vector3 sp = SPL[Random.Range(0, SPL.Length)].transform.position + new Vector3(
                 Random.Range(-spawn_range, spawn_range)
               , 0f
               , Random.Range(-spawn_range, spawn_range));

                GameObject eo = Instantiate(el.Status[enemyID].Enemy_Model, sp, Quaternion.identity, transform.Find("Enemy_ObjList"));
                switch (enemyID)
                {
                    case 0:
                        eo.GetComponent<Enemy_System>().Player = Player;
                        eo.GetComponent<Enemy_System>().em = this;
                        Debug.Log(eo.GetComponent<Enemy_System>().em);
                        eo.GetComponent<Enemy_System>().Enemy_Reset();
                        break; 
                    case 1:
                        eo.GetComponent<EnemyType2>().Player = Player;
                        eo.GetComponent<EnemyType2>().em = this;
                        Debug.Log(eo.GetComponent<EnemyType2>().em);
                        eo.GetComponent<EnemyType2>().Enemy_Reset();
                        break;
                    case 2:
                        //eo.GetComponent<Enemy_System>().Player = Player;
                        break;
                }
                yield return new WaitForSeconds(0.5f);
            }
        }
        Enemy_Manager.enemies_move_permit = true;
        */
    }
    public void Spawn_Function(int i)
    {
        Vector3 sp = SPL[Random.Range(0, SPL.Length)].transform.position + new Vector3(
                        Random.Range(-spawn_range, spawn_range)
                        , 0f
                        , Random.Range(-spawn_range, spawn_range));

        GameObject eo = Instantiate(el.Status[i].Enemy_Model, sp, Quaternion.identity, transform.Find("Enemy_ObjList"));
        switch (i)
        {
            case 0:
                eo.GetComponent<Enemy_System>().Player = Player;
                eo.GetComponent<Enemy_System>().em = this;
                Debug.Log(eo.GetComponent<Enemy_System>().em);
                eo.GetComponent<Enemy_System>().Enemy_Reset();
                break;
            case 1:
                eo.GetComponent<EnemyType2>().Player = Player;
                eo.GetComponent<EnemyType2>().em = this;
                Debug.Log(eo.GetComponent<EnemyType2>().em);
                eo.GetComponent<EnemyType2>().Enemy_Reset();
                break;
            case 2:
                break;
        }
    }
    public void ParentEnemyDeath()
    {
        current_enemies_count--;
        //現在のウェーブの敵が0になったら次のウェーブに移行
        if (current_enemies_count == 0 && current_wave != 3) StartCoroutine(Enemies_Spawn_Function(all_wave[current_wave]));
        //現在のウェーブの敵が0になり、かつ全ウェーブが終了していればゲームクリア
        if (current_enemies_count == 0 && current_wave == 3) transform.parent.GetComponent<GameManager>().
                Scene_Transition_Process(1);
    }
    public void Enemy_Manager_Reset()
    {
        for (int i = 0; i < transform.Find("Enemy_ObjList").childCount; i++) 
        {
            Destroy(transform.Find("Enemy_ObjList").GetChild(i).gameObject);
        }
        enemies_move_permit = false;
        current_enemies_count = 0;
        iscompletion = false;
    }
}
