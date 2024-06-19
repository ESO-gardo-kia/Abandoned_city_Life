using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Shooter_Enemy: EnemyDefaultSystem
{
    [System.NonSerialized] public Enemy_Manager enemyManager;
    private GameObject playerObject;
    [Header("--- UI�֌W ---")]
    [SerializeField] private GameObject enemyCanvas;
    [SerializeField] private Slider hpSlider;
    [SerializeField] private GameObject damageTextObject;
    [SerializeField] private GameObject damageTextPosition;
    private GameObject oldDamageText;
     private float oldDamage;

    [SerializeField] public NavMeshAgent navMeshAgent;
    [SerializeField] private Rigidbody rigidBody;

    [Header("--- �����i ---")]
    [SerializeField] private GameObject wheelObject;
    public float wheelRotationSpeed;
    [SerializeField] private GameObject shotPosition;
    public AudioClip shotSound;
    public float bulletDamage;//�_���[�W
    public float rapidFireRate;//�A�ˑ��x
    public float bulletRange;//�˒�
    public float bulletSpeed;//�e��
    public float diffusionChance;//�g�U��
    private float rateCount = 0;
    [SerializeField] public GameObject SHOTOBJ;

    [Header("--- �X�e�[�^�X ---")]
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
            if(other.CompareTag("Bullet"))
            {
                IsBulletTypeJuge(other, damageTextPosition.transform.position, transform, hpSlider.transform.rotation,
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
            NormalBulletSystem normalBulletSystem = shotObj.GetComponent<NormalBulletSystem>();

            normalBulletSystem.targetTag = "Player";
            normalBulletSystem.bulletDamage = bulletDamage;
            normalBulletSystem.bulletSpeed = bulletSpeed;
            normalBulletSystem.deathDistance = bulletRange / 1.5f;
            normalBulletSystem.firstPosition = shotPosition.transform.position;
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
        Debug.Log("���S");
        enemyManager.ParentEnemyDeath(enemy_number);
        Destroy(gameObject);
    }
}