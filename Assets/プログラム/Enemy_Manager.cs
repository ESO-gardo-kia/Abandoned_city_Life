using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Manager : MonoBehaviour
{
    static public bool enemies_move_permit;
    [SerializeField] private GameObject Enemy_Obj;
    private GameManager gm;
    public int enemies_count;
    void Start()
    {
        gm = transform.parent.GetComponent<GameManager>();
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Q)) Enemies_Spawn_Function();
    }
    public void Enemies_Spawn_Function()
    {
        //foreach (var i in num) enemies_count += i;
        Vector3 randomPosition = GetRandomPosition();
        GameObject newObject = Instantiate(Enemy_Obj, randomPosition, Quaternion.identity);
        newObject.transform.parent = transform;
    }
    public void Enemies_Spawn_Function(int[] num)
    {
        foreach (var i in num) enemies_count += i;
        Vector3 randomPosition = GetRandomPosition();
        GameObject newObject = Instantiate(Enemy_Obj, randomPosition, Quaternion.identity);
        newObject.transform.parent = transform;
    }
    Vector3 GetRandomPosition()
    {
        Vector3 randomPosition = new Vector3(Random.Range(-100f, 100f), 0f, Random.Range(-100f, 100f));
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPosition, out hit, 1.0f, NavMesh.AllAreas)) return hit.position;
        else return GetRandomPosition();
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
