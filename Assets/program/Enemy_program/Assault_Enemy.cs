using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Assault_Enemy : EnemyDefaultSystem
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
    [SerializeField] private GameObject wheel;
    public float wheelRotationSpeed;
    [SerializeField] private GameObject circularSaw;
    public float circularSawRotationSpeed;
    [SerializeField] private float attack_damage;
    public float rapidFireRate;//連射速度
    private float rateCount = 0;
    private bool isRateCount;
    [Header("--- ステータス ---")]
    [SerializeField] private Enemy_List enemy_List;
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
        if (Enemy_Manager.enemies_move_permit == true)
        {

            if (isDeath)
            {
                Deathfunction();
            }
            else if (!isDeath && !Player_System.playerIsDeath)
            {
                //rigidBody.velocity = Vector3.zero;
                navMeshAgent.destination = playerObject.transform.position;
                //transform.localRotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(playerObject.transform.position - transform.position), 3);
                WheelAnimation();
                enemyCanvas.transform.LookAt(playerObject.transform, Vector3.down * 180);
                hpSlider.value = currenthp;

            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (Enemy_Manager.enemies_move_permit == true)
        {
            if (other.GetComponent<Bullet_System>() != null)
            {
                TakeDamage(other.GetComponent<Bullet_System>(), other, damageTextPosition.transform.position, this.transform, hpSlider.transform.rotation, other.GetComponent<Bullet_System>().damage,
                    ref oldDamageText, ref damageTextObject, ref oldDamage, ref currenthp, ref isDeath);
            }
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (rateCount >= rapidFireRate)
            {
                playerObject.GetComponent<Player_System>().TakeDmage(attack_damage);
                rateCount = 0;
            }
        }
        if (rateCount < rapidFireRate)
        {
            rateCount += Time.deltaTime;
        }
    }
    private void WheelAnimation()
    {
        wheel.transform.Rotate(Vector3.up, navMeshAgent.velocity.magnitude * -wheelRotationSpeed);
        circularSaw.transform.Rotate(Vector3.up, circularSawRotationSpeed);
    }
    void Deathfunction()
    {
        Debug.Log("死亡");
        enemyManager.ParentEnemyDeath(enemy_number);
        Destroy(gameObject);
    }
}
