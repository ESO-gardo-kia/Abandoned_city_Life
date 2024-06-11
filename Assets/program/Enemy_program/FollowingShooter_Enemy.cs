using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class FollowingShooter_Enemy : EnemyDefaultSystem
{
    [System.NonSerialized] public Enemy_Manager enemyManager;
    private GameObject playerObject;
    [Header("--- UI関係 ---")]
    [SerializeField] private GameObject enemyCanvas;
    [SerializeField] private Slider hpSlider;
    [SerializeField] private GameObject damageTextObject;
    [SerializeField] private GameObject damageTextPosition;
    private GameObject oldDamageText;
    private float oldDamage;

    [SerializeField] public NavMeshAgent navMeshAgent;
    [SerializeField] private Rigidbody rigidBody;

    [Header("--- 装備品 ---")]
    [SerializeField] private GameObject wheelObject;
    public float wheelRotationSpeed;
    [SerializeField] private GameObject shotPosition;
    public AudioClip shotsound;
    public float bulletDamage;//ダメージ
    public float rapidFireRate;//連射速度
    public float bulletRange;//射程
    public float bulletSpeed;//弾速
    public float diffusionChance;//拡散率
    public float rateCount = 0;
    [SerializeField] public GameObject SHOTOBJ;

    [Header("--- ステータス ---")]
    public int enemy_number;
    private bool isDeath;

    private float hp;
    private float currenthp;
    private float atk;
    private float currentatk;
    private float agi;
    private float currentagi;

    public void Start()
    {
        EnemyStatsReset(ref isDeath, enemy_number, ref hp, ref atk, ref agi, ref currenthp, ref currentatk, ref currentagi, ref hpSlider, ref enemyManager, ref playerObject);
        NavMeshAgentReset(enemy_number, currentagi, ref navMeshAgent);
    }

    void Update()
    {
        if (Enemy_Manager.enemiesMovePermit == true)
        {
            if (isDeath)
            {
                Deathfunction();
            }
            else if (!isDeath && !Player_System.playerIsDeath)
            {
                rigidBody.velocity = Vector3.zero;
                navMeshAgent.destination = playerObject.transform.position;
                transform.localRotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(playerObject.transform.position - transform.position), 3);
                WheelAnimation();
                NomalShot();
                enemyCanvas.transform.LookAt(playerObject.transform, Vector3.down * 180);
                hpSlider.value = currenthp;
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (Enemy_Manager.enemiesMovePermit == true)
        {
            if (other.GetComponent<Bullet_System>() != null)
            {
                TakeDamage(other.GetComponent<Bullet_System>(), other, damageTextPosition.transform.position, this.transform, hpSlider.transform.rotation, other.GetComponent<Bullet_System>().damage,
                    ref oldDamageText, ref damageTextObject, ref oldDamage, ref currenthp, ref isDeath);
            }
        }
    }
    public void NomalShot()
    {
        if (rateCount >= rapidFireRate)
        {
            rateCount = 0;
            GameObject shotObj = Instantiate(SHOTOBJ, shotPosition.transform.position, Quaternion.identity);
            FollowingBulletSystem followingBulletSystem = shotObj.GetComponent<FollowingBulletSystem>();

            followingBulletSystem.targetTag = "Player";
            followingBulletSystem.bulletDamage = bulletDamage;
            followingBulletSystem.deathDistance = bulletRange / 1.5f;
            followingBulletSystem.bulletSpeed = bulletSpeed;
            followingBulletSystem.firstPosition = shotPosition.transform.position;
            followingBulletSystem.targetObj = playerObject;
            shotObj.transform.eulerAngles = transform.eulerAngles;
            shotObj.transform.eulerAngles += new Vector3(Random.Range(-diffusionChance, diffusionChance)
                            , Random.Range(-diffusionChance, diffusionChance)
                            , Random.Range(diffusionChance, diffusionChance));

        }
        if (rateCount < rapidFireRate)
        {
            rateCount += Time.deltaTime;
        }
    }
    private void WheelAnimation()
    {
        wheelObject.transform.Rotate(Vector3.up, navMeshAgent.velocity.magnitude * -wheelRotationSpeed);
    }
    void Deathfunction()
    {
        enemyManager.ParentEnemyDeath(enemy_number);
        Destroy(gameObject);
    }
}
