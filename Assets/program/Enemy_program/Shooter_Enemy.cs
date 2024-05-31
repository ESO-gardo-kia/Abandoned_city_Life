using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Shooter_Enemy: MonoBehaviour
{
    [System.NonSerialized] public Enemy_Manager enemyManager;
    private GameObject playerObject;
    [SerializeField] private Gun_List gunList;
    [SerializeField] private GameObject shotPosition;
    [SerializeField] private GameObject wheel;

    [SerializeField] private GameObject enemyCanvas;
    [SerializeField] private Slider hpSlider;
    [SerializeField] public GameObject damageTextObject;
    [SerializeField] private GameObject damageTextPosition;
    private GameObject oldDamageText;
     private float oldDamage;

    private NavMeshAgent navMeshAgent;
    private Rigidbody rigitBody;

    [Header("--- 装備品 ---")]
    public AudioClip shotSound;
    public float bulletDamage;//ダメージ
    public float rapidFireRate;//連射速度
    public float bulletRange;//射程
    public float bulletSpeed;//弾速
    public float diffusionChance;//拡散率
    private float rateCount = 0;
    [SerializeField] public GameObject SHOTOBJ;

    [Header("--- ステータス ---")]
    [SerializeField] private Enemy_List enemy_List;
    private int enemy_number;
    private bool isdeath;

    private float hp;
    private float currenthp;
    private float atk;
    private float currentatk;
    private float agi;
    private float currentagi;

    void Start()
    {
        Enemy_Reset();
    }

    void Update()
    {
        WheelAnimation();
        if (Enemy_Manager.enemies_move_permit == true)
        {
            hpSlider.value = currenthp;
            if (isdeath)
            {
                Deathfunction();
            }
            else if (!isdeath && !Player_System.playerIsDeath)
            {
                navMeshAgent.destination = playerObject.transform.position;
                transform.localRotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(playerObject.transform.position - transform.position), 3);
            }
            enemyCanvas.transform.LookAt(playerObject.transform, Vector3.down * 180);
            //銃関係
            if (rateCount >= rapidFireRate&& navMeshAgent.destination != null)
            {
                NomalShot();
            }
            if (rateCount < rapidFireRate) rateCount += Time.deltaTime;
        }
    }

    private void WheelAnimation()
    {
        if (rigitBody.velocity != Vector3.zero) wheel.transform.Rotate(Vector3.up, rigitBody.velocity.magnitude * 3);
    }

    void OnTriggerEnter(Collider other)
    {
        if (Enemy_Manager.enemies_move_permit == true)
        {
            if (other.gameObject.CompareTag("Bullet")
                && other.GetComponent<Bullet_System>().target_tag == "Enemy")
            {
                TakeDmage(other.GetComponent<Bullet_System>().damage, other.GetComponent<Bullet_System>());
                other.GetComponent<Bullet_System>().BulletDestroy();
            }
            if (other.gameObject.CompareTag("Attack_Obj"))
            {
                //攻撃出来る状態かつ攻撃対象はエネミーの時
                if (other.GetComponent<Attack_System>().isAttack && other.GetComponent<Attack_System>().attack_subject == Attack_System.Attack_Subject.Enemy)
                {
                    TakeDmage(other.GetComponent<Attack_System>().damage, other, other.GetComponent<Attack_System>());
                }
            }
        }
    }
    public void Enemy_Reset()
    {
        //Debug.Log("敵の情報をリセットする");
        if(enemyManager == null)enemyManager = transform.parent.transform.parent.GetComponent<Enemy_Manager>();
        if (playerObject == null) playerObject = enemyManager.player_system;
        navMeshAgent = GetComponent<NavMeshAgent>();
        rigitBody = GetComponent<Rigidbody>();
        isdeath = false;
        //ステータス反映
        //StatusはScriptableObjectにて改変する事
        var e_l = enemy_List.Status[0];
        hp = enemy_List.Status[0].hp;
        atk = enemy_List.Status[0].atk;
        agi = enemy_List.Status[0].agi;
        currenthp = hp;
        currentatk = atk;
        currentagi = agi;
        hpSlider.maxValue = hp;
        hpSlider.value = currenthp;
        enemy_number = 0;

        navMeshAgent.speed = currentagi;
        navMeshAgent.stoppingDistance = 20;
    }
    void TakeDmage(float damage, Bullet_System BS)
    {
        if (oldDamageText == null)
        {
            GameObject dt = Instantiate(damageTextObject, damageTextPosition.transform.position, hpSlider.transform.rotation, transform.Find("EnemyCanvas"));
            Text t = dt.GetComponent<UnityEngine.UI.Text>();
            Damage_Text dts = dt.GetComponent<Damage_Text>();
            oldDamageText = dt;

            t.text = BS.damage.ToString();
            oldDamage = BS.damage;
        }
        else if (oldDamageText != null)
        {
            oldDamage += BS.damage;
            oldDamageText.GetComponent<UnityEngine.UI.Text>().text = oldDamage.ToString();
            oldDamageText.GetComponent<Damage_Text>().TextReset();
            oldDamageText.transform.localEulerAngles = new Vector3(0, 180, 180);
        }
        if (currenthp > 0)
        {
            Debug.Log(damage);
            currenthp -= damage;
        }
        if (currenthp <= 0)
        {
            isdeath = true;
        }
    }
    public void NomalShot()
    {
        GameObject shotObj = Instantiate(SHOTOBJ, shotPosition.transform.position, Quaternion.identity);
        Rigidbody rb = shotObj.GetComponent<Rigidbody>();
        NormalBulletSystem normalBulletSystem = shotObj.GetComponent<NormalBulletSystem>();

        normalBulletSystem.targetTag = "Player";
        normalBulletSystem.bulletDamage = bulletDamage;
        normalBulletSystem.deathDistance = bulletRange / 1.5f;
        normalBulletSystem.firstpos = shotPosition.transform.position;
        shotObj.transform.eulerAngles = transform.eulerAngles;
        shotObj.transform.eulerAngles += new Vector3(Random.Range(-diffusionChance, diffusionChance)
                            , Random.Range(-diffusionChance, diffusionChance)
                            , Random.Range(diffusionChance, diffusionChance));
        rb.velocity = normalBulletSystem.transform.forward * bulletSpeed;
        rateCount = 0;
        //shotObj.transform.eulerAngles = this.transform.eulerAngles + new Vector3(0, 0, -90);
    }
    void TakeDmage(float damage, Collider other, Attack_System AS)
    {
        if (oldDamageText == null)
        {
            GameObject dt = Instantiate(damageTextObject, damageTextPosition.transform.position, hpSlider.transform.rotation, transform.Find("EnemyCanvas"));
            UnityEngine.UI.Text t = dt.GetComponent<UnityEngine.UI.Text>();
            Bullet_System bs = other.GetComponent<Bullet_System>();
            Damage_Text dts = dt.GetComponent<Damage_Text>();
            oldDamageText = dt;

            dt.transform.localEulerAngles = new Vector3(0, 180, 180);
            t.text = bs.damage.ToString();
            oldDamage = bs.damage;
        }
        else if (oldDamageText != null)
        {
            oldDamage += other.GetComponent<Bullet_System>().damage;
            oldDamageText.GetComponent<UnityEngine.UI.Text>().text = oldDamage.ToString();
            oldDamageText.GetComponent<Damage_Text>().TextReset();

        }
        if (currenthp > 0)
        {
            Debug.Log(damage);
            currenthp -= damage;
        }
        if (currenthp <= 0)
        {
            isdeath = true;
        }
    }
    void Deathfunction()
    {
        Debug.Log("死亡");
        enemyManager.ParentEnemyDeath(enemy_number);
        Destroy(gameObject);
    }
}
