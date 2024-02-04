using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Manager : MonoBehaviour
{
    public bool Debug_Mode;
    static public bool enemies_move_permit;
    public List<int[]> all_wave;
    [SerializeField] private GameObject Enemy_Obj;
    [SerializeField] public GameObject player_system;
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
        if(Debug_Mode) enemies_move_permit = true;
        else enemies_move_permit = false;
    }
    public IEnumerator Enemies_Spawn_Function(int[] wave)
    {
        //wave1[�G��ID,�G�̐�]
        iscompletion = false;
        yield return new WaitForSeconds(0f);
        foreach (var i in wave) current_enemies_count += i;// ���E�F�[�u�̓G�̑����𐔂���
        for(int id = 0; id < wave.Length; id++)
        {
            Debug.Log("���݂�ID1:" + id);
            for (int i = 0; i < wave[id]; i++)
            {
                //�G�𐶐�
                if (!Player_System.move_permit && !enemies_move_permit) yield break;
                Debug.Log("���݂�ID2:" + id);
                Spawn_Function(id);
                yield return new WaitForSeconds(1f);
            }
        }

        Debug.Log("�E�F�[�u" + current_wave + "�I��");
        if (current_wave == 2) Debug.Log("�S�E�F�[�u�I��");
        enemies_move_permit = true;
        current_wave++;//���̃E�F�[�u�֐�����i�߂�
    }
    public void Spawn_Function(int i)
    {
        Debug.Log("���������G��ID:"+i);
        GameObject eo = Instantiate(el.Status[i].Enemy_Model
            , SPL[Random.Range(0, SPL.Length)].transform.position + new Vector3(Random.Range(-spawn_range, spawn_range), 3f, Random.Range(-spawn_range, spawn_range))
            , Quaternion.identity, transform.Find("Enemy_ObjList"));
        switch (i)
        {
            case 0:
                Shooter_Enemy ShooterE = eo.GetComponent<Shooter_Enemy>();
                ShooterE.em = this;
                ShooterE.Enemy_Reset();
                break;
            case 1:
                ClusterCatapult_Enemy ClusterE = eo.GetComponent<ClusterCatapult_Enemy>();
                ClusterE.em = this;
                ClusterE.Enemy_Reset();
                break;
            case 2:
                FollowingShooter_Enemy FollowingE = eo.GetComponent<FollowingShooter_Enemy>();
                FollowingE.em = this;
                FollowingE.Enemy_Reset();
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
