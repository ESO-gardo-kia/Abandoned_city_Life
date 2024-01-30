using UnityEngine;
using UnityEngine.AI;

public class Shooter_Enemy: MonoBehaviour
{
    public Enemy_Manager em;
    [SerializeField] public GameObject Player;
    [SerializeField] public Gun_List gunlist;
    private GameObject SHOTPOS;
    private GameObject Enemy_Obj;
    private GameObject Attack_Obj;

    private GameObject EnemyCanvas;
    private UnityEngine.UI.Slider HPSlider;
    [SerializeField] public GameObject TEXTOBJ;
    private GameObject TEXTPOS;
    private GameObject old_dt;
    private float old_damage;

    private NavMeshAgent NMA;

    [Header("--- 装備品 ---")]
    public int weapon_id;
    private float rate_count = 0;
    [SerializeField] public GameObject SHOTOBJ;

    [Header("--- ステータス ---")]
    [SerializeField] private Enemy_List enemy_List;
    private int enemy_number;
    private bool isdeath;

    private string Ename;
    private float exp;

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

    void FixedUpdate()
    {
        if (Enemy_Manager.enemies_move_permit == true)
        {
            if (isdeath)
            {
                Deathfunction();
            }
            else if (!isdeath && !Player_System.player_isdeath)
            {
                NMA.destination = Player.transform.position;

                transform.localRotation = Quaternion.RotateTowards(transform.rotation
                    , Quaternion.LookRotation(Player.transform.position - transform.position)
                    , 10);
            }
            EnemyCanvas.transform.LookAt(Player.transform, Vector3.down * 180);

            HPSlider.value = currenthp;
            //銃関係
            if (rate_count >= gunlist.Data[weapon_id].rapid_fire_rate
                && NMA.destination != null)
            {
                Debug.Log("発射しました");
                NomalShot();
                rate_count = 0;
            }
            /*
            if (current_loaded_bullets <= 0) isreload = true;
            if (isreload)
            {
                reload_count += 0.2f;
                if (reload_count >= reload_speed)
                {
                    current_loaded_bullets = loaded_bullets;
                    reload_count = 0;
                    isreload = false;
                }
            }
            */
            if (rate_count < gunlist.Data[weapon_id].rapid_fire_rate) rate_count += 0.2f;
            //if(!old_dt) old_dt.transform.localEulerAngles = new Vector3(0, 180, 180);

        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (Enemy_Manager.enemies_move_permit == true)
        {
            if (other.gameObject.CompareTag("Bullet")
                && other.GetComponent<Bullet_System>().target_tag == "Enemy")
            {
                TakeDmage(other.GetComponent<Bullet_System>().damage, other.GetComponent<Bullet_System>());
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
        Debug.Log("敵の情報をリセットする");
        em = transform.parent.transform.parent.GetComponent<Enemy_Manager>();
        if (Player == null) Player = GameObject.Find("Player");
        Enemy_Obj = transform.Find("Enemy_Obj").gameObject;
        Attack_Obj = transform.Find("Enemy_Obj/Attack_Obj").gameObject;
        SHOTPOS = transform.Find("SHOTPOS").gameObject;
        TEXTPOS = transform.Find("TEXTPOS").gameObject;
        EnemyCanvas = transform.Find("EnemyCanvas").gameObject;
        HPSlider = transform.Find("EnemyCanvas/HPSlider").gameObject.GetComponent<UnityEngine.UI.Slider>();
        NMA = this.GetComponent<NavMeshAgent>();
        isdeath = false;
        //ステータス反映
        //StatusはScriptableObjectにて改変する事
        var e_l = enemy_List.Status[1];
        Ename = e_l.name;
        exp = e_l.exp;
        hp = e_l.hp;
        atk = e_l.atk;
        agi = e_l.agi;
        currenthp = hp;
        currentatk = atk;
        currentagi = agi;
        HPSlider.maxValue = hp;
        HPSlider.value = currenthp;

        NMA.speed = agi;
        NMA.stoppingDistance = gunlist.Data[weapon_id].bullet_range / 1.5f;
    }
    void TakeDmage(float damage, Bullet_System BS)
    {
        if (old_dt == null)
        {
            GameObject dt = Instantiate(TEXTOBJ, TEXTPOS.transform.position, HPSlider.transform.rotation, transform.Find("EnemyCanvas"));
            UnityEngine.UI.Text t = dt.GetComponent<UnityEngine.UI.Text>();
            Damage_Text dts = dt.GetComponent<Damage_Text>();
            old_dt = dt;

            t.text = BS.damage.ToString();
            old_damage = BS.damage;
        }
        else if (old_dt != null)
        {
            old_damage += BS.damage;
            old_dt.GetComponent<UnityEngine.UI.Text>().text = old_damage.ToString();
            old_dt.GetComponent<Damage_Text>().TextReset();
            old_dt.transform.localEulerAngles = new Vector3(0, 180, 180);
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
        GameObject shotObj = Instantiate(SHOTOBJ, SHOTPOS.transform.position, Quaternion.identity);
        Rigidbody rb = shotObj.GetComponent<Rigidbody>();
        Bullet_System bs = shotObj.GetComponent<Bullet_System>();
        var Guns = gunlist.Data;

        bs.target_tag = "Player";
        bs.damage = Guns[weapon_id].bullet_damage;
        bs.death_dis = Guns[weapon_id].bullet_range / 1.5f;
        bs.firstpos = SHOTPOS.transform.position;
        rb.velocity = this.transform.forward * Guns[weapon_id].bullet_speed;
        shotObj.transform.eulerAngles = this.transform.eulerAngles;
        //shotObj.transform.eulerAngles = this.transform.eulerAngles + new Vector3(0, 0, -90);
    }
    void TakeDmage(float damage, Collider other, Attack_System AS)
    {
        if (old_dt == null)
        {
            GameObject dt = Instantiate(TEXTOBJ, TEXTPOS.transform.position, HPSlider.transform.rotation, transform.Find("EnemyCanvas"));
            UnityEngine.UI.Text t = dt.GetComponent<UnityEngine.UI.Text>();
            Bullet_System bs = other.GetComponent<Bullet_System>();
            Damage_Text dts = dt.GetComponent<Damage_Text>();
            old_dt = dt;

            dt.transform.localEulerAngles = new Vector3(0, 180, 180);
            t.text = bs.damage.ToString();
            old_damage = bs.damage;
        }
        else if (old_dt != null)
        {
            old_damage += other.GetComponent<Bullet_System>().damage;
            old_dt.GetComponent<UnityEngine.UI.Text>().text = old_damage.ToString();
            old_dt.GetComponent<Damage_Text>().TextReset();

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
        em.ParentEnemyDeath();
        Destroy(gameObject);
    }
}
