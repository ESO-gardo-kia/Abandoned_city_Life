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
    public bool iscompletion;//���̃E�F�[�u���I��������ǂ���
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
        //wave1[�G��ID,�G�̐�]
        iscompletion = false;
        yield return new WaitForSeconds(0f);
        foreach (var i in wave) current_enemies_count += i;// ���E�F�[�u�̓G�̑����𐔂���
        Debug.Log(wave[0]);
        for(int id = 0; id < wave.Length; id++)
        {
            for (int i = 0; i < wave[id]; i++)
            {
                //�G�𐶐�
                Debug.Log(i);
                if (!Player_System.move_permit && !enemies_move_permit) yield break;
                Spawn_Function(i);
                yield return new WaitForSeconds(0.5f);
            }
        }

        Debug.Log("�E�F�[�u" + current_wave + "�I��");
        if (current_wave == 2) Debug.Log("�S�E�F�[�u�I��");
        enemies_move_permit = true;
        current_wave++;//���̃E�F�[�u�֐�����i�߂�
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
                eo.GetComponent<Shooter_Enemy>().Player = Player;
                eo.GetComponent<Shooter_Enemy>().em = this;
                Debug.Log(eo.GetComponent<Shooter_Enemy>().em);
                eo.GetComponent<Shooter_Enemy>().Enemy_Reset();
                break;
            case 1:
                break;
            case 2:
                break;
        }
    }
    public void ParentEnemyDeath()
    {
        current_enemies_count--;
        //���݂̃E�F�[�u�̓G��0�ɂȂ����玟�̃E�F�[�u�Ɉڍs
        if (current_enemies_count == 0 && current_wave != 3) StartCoroutine(Enemies_Spawn_Function(all_wave[current_wave]));
        //���݂̃E�F�[�u�̓G��0�ɂȂ�A���S�E�F�[�u���I�����Ă���΃Q�[���N���A
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
