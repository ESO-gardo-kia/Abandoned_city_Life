using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using DG.Tweening;
using static Stage_Information.stage_information;

public class Enemy_Manager : MonoBehaviour
{
    public StageType stagetype;
    [SerializeField] private Stage_Information stageInformation;
    [SerializeField] private Enemy_List enemyList;
    public bool Debug_Mode;
    static public bool enemiesMovePermit;
    public float battleDuration;
    public List<int[]> normalWaveEnemyList;
    [SerializeField] private GameManager gm;
    [SerializeField] private GameObject Enemy_Obj;
    [SerializeField] public GameObject player_system;
    [SerializeField] public Enemy_List el;
    public GameObject[] SPL;//SpawnPoint_List
    public GameObject deathparticle;

    [SerializeField] private GameObject EMCanvas;
    [SerializeField] private GameObject WAVEText;
    [SerializeField] private GameObject SYSTEMText;

    public int spawn_range = 20;
    public float spawn_interval = 2;
    public int current_enemies_count;
    public int currentWave = 0;

    public int[] destroyEnemy;
    public float getMoney;
    public int endlessWave;
    void Start()
    {
        GetSpawnPoint();
        destroyEnemy = new int[enemyList.data.Count];
        if (Debug_Mode) enemiesMovePermit = true;
        else enemiesMovePermit = false;

        void GetSpawnPoint()
        {
            SPL = new GameObject[transform.Find("SpawnPoint_List").childCount];
            for (int i = 0; i < transform.Find("SpawnPoint_List").childCount; i++)
            {
                SPL[i] = transform.Find("SpawnPoint_List").GetChild(i).gameObject;
            }
        }
    }
    private void Update()
    {
        if (enemiesMovePermit)
        {
            battleDuration += Time.deltaTime;
        }
    }

    public IEnumerator Enemies_Spawn_Function(int transitionSceneNumber)
    {
        StartWaveUiFanction();
        switch (stagetype)
        {
            case StageType.NomalStage:
                yield return new WaitForSeconds(1f);
                //Debug.Log(currentWave + "ウェーブ目開始");
                //Debug.Log(normalWaveEnemyList[currentWave].Length);
                for (int spawnEnemyId = 0; spawnEnemyId < normalWaveEnemyList[currentWave].Length; spawnEnemyId++)
                {
                    for (int i = 0; i < normalWaveEnemyList[currentWave][spawnEnemyId]; i++)
                    {
                        Spawn_Function(spawnEnemyId);
                        yield return new WaitForSeconds(spawn_interval);
                    }
                }
                currentWave++;
                break;
            case StageType.endless:
                yield return new WaitForSeconds(1f);
                for (int spawnEnemyId = 0; spawnEnemyId < enemyList.data.Count; spawnEnemyId++)
                {
                    for (int i = 0; i < Random.Range(1, currentWave + 1); i++)
                    {
                        Spawn_Function(spawnEnemyId);
                        yield return new WaitForSeconds(spawn_interval);
                    }
                }
                currentWave++;
                break;
            case StageType.boss:
                break;
        }
    }
    private void StartWaveUiFanction()
    {
        WAVEText.SetActive(true);
        Text waveText = WAVEText.GetComponent<Text>();
        if (currentWave == 0) waveText.color = Color.clear;

        DOTween.Sequence()
        .Append(waveText.DOFade(0, 0.5f).SetEase(Ease.InQuart)
        .OnComplete(() =>
        {
            waveText.text = "WAVE" + (1 + currentWave).ToString();
        }))
        .Append(waveText.DOFade(1, 0.5f).SetEase(Ease.InQuart))
    .Play();
    }
    public void GetCurrentWaveEnemy(int transitionSceneNumber)
    {
        //normalWaveEnemyList.Clear();
        normalWaveEnemyList = new List<int[]>
        {
            stageInformation.data[transitionSceneNumber].enemies_num1,
            stageInformation.data[transitionSceneNumber].enemies_num2,
            stageInformation.data[transitionSceneNumber].enemies_num3
        };
        /*
        Debug.Log(stageInformation.data[2].enemies_num1.Length);
        Debug.Log(stageInformation.data[2].enemies_num2.Length);
        Debug.Log(stageInformation.data[2].enemies_num3.Length);
        Debug.Log(normalWaveEnemyList[1][0]);
        Debug.Log(stageInformation.data[transitionSceneNumber].enemies_num2.Length);
        Debug.Log(stageInformation.data[transitionSceneNumber].enemies_num3.Length);
        */
    }
    public void Spawn_Function(int enemyId)
    {
        current_enemies_count++;
        GameObject enemyObject = Instantiate(el.data[enemyId].Enemy_Model
            , SPL[Random.Range(0, SPL.Length)].transform.position + new Vector3(Random.Range(-spawn_range, spawn_range), 3f, Random.Range(-spawn_range, spawn_range))
            , Quaternion.identity, transform.Find("Enemy_ObjList"));
        switch (enemyId)
        {
            case 0:
                Shooter_Enemy ShooterE = enemyObject.GetComponent<Shooter_Enemy>();
                ShooterE.enemyManager = this;
                ShooterE.Start();
                break;
            case 1:
                Assault_Enemy AssaultE = enemyObject.GetComponent<Assault_Enemy>();
                AssaultE.enemyManager = this;
                AssaultE.Start();
                break;
            case 2:
                ClusterCatapult_Enemy ClusterE = enemyObject.GetComponent<ClusterCatapult_Enemy>();
                ClusterE.enemyManager = this;
                ClusterE.Start();
                break;
            case 3:
                Airborne_Enemy AirborneE = enemyObject.GetComponent<Airborne_Enemy>();
                AirborneE.enemyManager = this;
                AirborneE.Start();
                break;
            case 4:
                FollowingShooter_Enemy FollowingE = enemyObject.GetComponent<FollowingShooter_Enemy>();
                FollowingE.enemyManager = this;
                FollowingE.Start();
                break;
        }
    }
    public void ParentEnemyDeath(int enemyId)
    {
        destroyEnemy[enemyId]++;
        current_enemies_count--;
        getMoney += enemyList.data[enemyId].price;
        switch (stagetype)
        {
            case StageType.NomalStage:
                //現在のウェーブの敵が0になったら次のウェーブに移行
                if (current_enemies_count <= 0 && currentWave != 3)
                {
                    StartCoroutine(Enemies_Spawn_Function(enemyId));
                }
                //現在のウェーブの敵が0になり、かつ全ウェーブが終了していればゲームクリア
                if (current_enemies_count <= 0 && currentWave >= 3)
                {
                    StartCoroutine(gm.GameOver(destroyEnemy, getMoney, battleDuration, true));

                }
                break;
            case StageType.endless:
                if (current_enemies_count <= 0 && currentWave != 100)
                {
                    StartCoroutine(Enemies_Spawn_Function(enemyId));
                }
                if (current_enemies_count == 0 && currentWave == 100)
                {
                    StartCoroutine(gm.GameOver(destroyEnemy, getMoney, battleDuration, true));
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
        enemiesMovePermit = false;
        endlessWave = 0;
        current_enemies_count = 0;
        currentWave = 0;
    }
    public void Player_Death()
    {
        StartCoroutine(gm.GameOver(destroyEnemy, getMoney,battleDuration, false));
    }
}
