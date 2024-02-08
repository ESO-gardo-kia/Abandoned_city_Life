using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using DG.Tweening;
using static Stage_Information.stage_information;
using static Stage_Information;
using UnityEngine.UIElements;

public class Enemy_Manager : MonoBehaviour
{
    public StageType stagetype;
    public bool Debug_Mode;
    static public bool enemies_move_permit;
    public List<int[]> all_wave;
    [SerializeField] private GameObject Enemy_Obj;
    [SerializeField] public GameObject player_system;
    [SerializeField] public Enemy_List el;
    public GameObject[] SPL;//SpawnPoint_List
    public GameObject deathparticle;

    private GameObject EMCanvas;
    private GameObject WAVEText;
    private GameObject SYSTEMText;

    public int spawn_range = 20;
    public float spawn_interval = 2;
    public int current_enemies_count;
    public bool iscompletion;//���̃E�F�[�u���I��������ǂ���
    public int current_wave = 0;
    void Start()
    {
        EMCanvas = transform.Find("EMCanvas").gameObject;
        WAVEText = EMCanvas.transform.Find("WAVEText").gameObject;
        SYSTEMText = EMCanvas.transform.Find("SYSTEMText").gameObject;

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
        Debug.Log("�Q�[���J�n");
        switch (stagetype)
        {
            case StageType.NomalStage:
                WAVEText.SetActive(true);
                Text W1 = WAVEText.GetComponent<Text>();
                if(current_wave == 0) W1.color = Color.clear;
                DOTween.Sequence()
                .Append(W1.DOFade(0, 0.5f).SetEase(Ease.InQuart)
                .OnComplete(() => {
                    Debug.Log("�N���P");
                    W1.text = "WAVE" + (1 + current_wave).ToString();
                }))
                .Append(W1.DOFade(1, 0.5f).SetEase(Ease.InQuart))
            .Play();

                //wave1[�G��ID,�G�̐�]
                iscompletion = false;
                yield return new WaitForSeconds(1f);

                foreach (var i in wave) current_enemies_count += i;// ���E�F�[�u�̓G�̑����𐔂���
                for (int id = 0; id < wave.Length; id++)
                {
                    for (int i = 0; i < wave[id]; i++)
                    {
                        if (!Player_System.move_permit && !enemies_move_permit) yield break;
                        Spawn_Function(id);
                        yield return new WaitForSeconds(spawn_interval);
                    }
                }
                Debug.Log("�E�F�[�u" + current_wave + "�I��");
                enemies_move_permit = true;
                current_wave++;//���̃E�F�[�u�֐�����i�߂�
                break;

                //----------------------------------------

            case StageType.endless:
                //wave[�G��ID,�G�̐�]
                WAVEText.SetActive(true);
                Text W2 = WAVEText.GetComponent<Text>();
                if(current_wave == 0) W2.color = Color.clear;
            DOTween.Sequence()
                .Append(W2.DOFade(0, 0.6f).SetEase(Ease.InQuart))
                .Append(W2.DOFade(1, 0.6f).SetEase(Ease.InQuart)
                .OnComplete(() => {
                    Debug.Log("�N���P");
                    W2.text = "WAVE" + (1 + current_wave).ToString();
                }))
            .Play();

                for (int i = 0; i < wave.Length; i++)
                {
                    int num = Random.Range(1, current_wave + 1);
                    wave[i] = num;
                    current_enemies_count += num;
                }

                yield return new WaitForSeconds(1f);

                for (int id = 0; id < wave.Length; id++)
                {
                    for (int i = 0; i < wave[id]; i++)
                    {
                        if (!Player_System.move_permit && !enemies_move_permit) yield break;
                        Spawn_Function(id);
                        yield return new WaitForSeconds(spawn_interval);
                    }
                }

                Debug.Log("�E�F�[�u" + current_wave + "�I��");
                enemies_move_permit = true;
                break;

                //----------------------------------------

            case StageType.boss:
                break;
        }
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
                Assault_Enemy AssaultE = eo.GetComponent<Assault_Enemy>();
                AssaultE.em = this;
                AssaultE.Enemy_Reset();
                break;
            case 2:
                ClusterCatapult_Enemy ClusterE = eo.GetComponent<ClusterCatapult_Enemy>();
                ClusterE.em = this;
                ClusterE.Enemy_Reset();
                break;
            case 3:
                FollowingShooter_Enemy FollowingE = eo.GetComponent<FollowingShooter_Enemy>();
                FollowingE.em = this;
                FollowingE.Enemy_Reset();
                break;
        }
    }
    public void ParentEnemyDeath(Vector3 my)
    {

        switch (stagetype)
        {
            case StageType.NomalStage:
                current_enemies_count--;
                GameObject par = Instantiate(deathparticle, my, Quaternion.identity, transform.transform.parent = null);
                par.transform.localScale = Vector3.one * 5;
                //���݂̃E�F�[�u�̓G��0�ɂȂ����玟�̃E�F�[�u�Ɉڍs
                if (current_enemies_count <= 0 && current_wave != 3) StartCoroutine(Enemies_Spawn_Function(all_wave[current_wave]));
                //���݂̃E�F�[�u�̓G��0�ɂȂ�A���S�E�F�[�u���I�����Ă���΃Q�[���N���A
                if (current_enemies_count <= 0 && current_wave == 3)
                {
                    transform.parent.GetComponent<GameManager>().Scene_Transition_Process(1);
                }
                break;
            case StageType.endless:
                current_enemies_count--;
                //���݂̃E�F�[�u�̓G��0�ɂȂ����玟�̃E�F�[�u�Ɉڍs
                if (current_enemies_count <= 0 && current_wave != 100) StartCoroutine(Enemies_Spawn_Function(all_wave[0]));
                //���݂̃E�F�[�u�̓G��0�ɂȂ�A���S�E�F�[�u���I�����Ă���΃Q�[���N���A
                if (current_enemies_count == 0 && current_wave == 100)
                {
                    transform.parent.GetComponent<GameManager>().Scene_Transition_Process(1);
                }
                break;
            case StageType.boss:
                break;
        }

    }
    public void Enemy_Manager_Reset()
    {
        for (int i = 0; i < transform.Find("Enemy_ObjList").childCount; i++) 
        {
            Destroy(transform.Find("Enemy_ObjList").GetChild(i).gameObject);
        }
        WAVEText.SetActive(false);
        enemies_move_permit = false;
        current_enemies_count = 0;
        current_wave = 0;
        iscompletion = false;
    }
}
