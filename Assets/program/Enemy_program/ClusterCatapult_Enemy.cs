using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using static Gun_List;

public class ClusterCatapult_Enemy : EnemyDefaultSystem
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
    public AudioClip shotSound;
    public float bulletDamage;//ダメージ
    public float bulletSpeed;//弾速
    public int ClusterSplitAmount;//弾速
    public float rapidFireRate;//連射速度
    private float rateCount = 0;
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
            else if (!isDeath && !PlayerMainSystem.playerIsDeath)
            {
                rigidBody.velocity = Vector3.zero;
                navMeshAgent.destination = playerObject.transform.position;
                transform.localRotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(playerObject.transform.position - transform.position), 3);
                //WheelAnimation();
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
            //AS.PlayOneShot(shotsound);
            GameObject shotObj = Instantiate(SHOTOBJ, shotPosition.transform.position, Quaternion.identity);
            ParabolaBulletSystem bs = shotObj.GetComponent<ParabolaBulletSystem>();

            bs.targetTag = "Player";
            bs.bulletDamage = bulletDamage;
            bs.bulletSpeed = bulletSpeed;
            bs.deathDistance = Vector3.Distance(shotPosition.transform.position, playerObject.transform.position + Vector3.up * 3);
            bs.ClusterSplitAmount = ClusterSplitAmount;
            bs.firstPosition = shotPosition.transform.position;
            bs.targetPosition = playerObject.transform.position + Vector3.up * 3;
            shotObj.transform.LookAt(playerObject.transform.position + Vector3.up * 30);
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
        Debug.Log("死亡");
        enemyManager.ParentEnemyDeath(enemy_number);
        Destroy(gameObject);
    }
}
