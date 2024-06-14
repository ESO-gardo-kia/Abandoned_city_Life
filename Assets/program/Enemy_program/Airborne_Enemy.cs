using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Airborne_Enemy : EnemyDefaultSystem
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

    [SerializeField] private Rigidbody rigidBody;

    [Header("--- 装備品 ---")]
    public GameObject wheelObject;
    public float wheelRotationSpeed;
    [SerializeField] private GameObject shotPosition;
    [SerializeField] private GameObject SHOTBORNE;
    public AudioClip shotsound;
    public float bulletDamage;//ダメージ
    public float rapidFireRate;//連射速度
    public float bulletRange;//射程
    public float bulletSpeed;//弾速
    public float diffusionChance;//拡散率
    public float rateCount = 0;
    [SerializeField] public GameObject SHOTOBJ;

    [Header("--- ステータス ---")]
    private float height;
    public float minheight;
    public float maxheight;

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
        this.transform.position += Vector3.up * Random.Range(minheight, maxheight);
        height = Random.Range(minheight, maxheight);
    }

    void FixedUpdate()
    {
        wheelObject.transform.Rotate(Vector3.up, 4);
        if (Enemy_Manager.enemiesMovePermit == true)
        {
            if (isDeath)
            {
                Deathfunction();
            }
            else if (!isDeath && !PlayerMainSystem.playerIsDeath)
            {
                float dir = Vector3.Distance(transform.position, playerObject.transform.position);
                transform.localRotation = Quaternion.RotateTowards(transform.rotation
                , Quaternion.LookRotation((playerObject.transform.position + Vector3.up * height) - transform.position), 3);
                SHOTBORNE.transform.LookAt(playerObject.transform.position);
                if (Vector3.Distance(transform.position, playerObject.transform.position) > 20)
                {
                    rigidBody.velocity = transform.forward * currentagi;
                }
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
            NormalBulletSystem normalBulletSystem = shotObj.GetComponent<NormalBulletSystem>();

            normalBulletSystem.targetTag = "Player";
            normalBulletSystem.bulletDamage = bulletDamage;
            normalBulletSystem.bulletSpeed = bulletSpeed;
            normalBulletSystem.deathDistance = bulletRange / 1.5f;
            normalBulletSystem.firstPosition = shotPosition.transform.position;
            shotObj.transform.eulerAngles = shotPosition.transform.eulerAngles;
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
        wheelObject.transform.Rotate(Vector3.up, rigidBody.velocity.magnitude * -wheelRotationSpeed);
    }
    void Deathfunction()
    {
        Debug.Log("死亡");
        enemyManager.ParentEnemyDeath(enemy_number);
        Destroy(gameObject);
    }
}
