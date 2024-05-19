using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Airborne_Enemy : MonoBehaviour
{
    public Enemy_Manager em;
    [SerializeField] public GameObject Player;
    [SerializeField] public Gun_List gunlist;
    [SerializeField] private GameObject SHOTPOS;
    [SerializeField] private GameObject SHOTBORNE;
    public GameObject wheel;

    private GameObject EnemyCanvas;
    private UnityEngine.UI.Slider HPSlider;
    [SerializeField] public GameObject TEXTOBJ;
    private GameObject TEXTPOS;
    private GameObject old_dt;
    private float old_damage;

    private Rigidbody rb;

    [Header("--- 装備品 ---")]
    public AudioClip shotsound;
    public float bullet_damage;//ダメージ
    public float rapid_fire_rate;//連射速度
    public float bullet_range;//射程
    public float bullet_speed;//弾速
    public float diffusion_chance;//拡散率
    public float rate_count = 0;
    [SerializeField] public GameObject SHOTOBJ;

    [Header("--- ステータス ---")]
    private float height;
    public float minheight;
    public float maxheight;
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
        wheel.transform.Rotate(Vector3.up, 4);
        if (Enemy_Manager.enemies_move_permit == true)
        {
            if (isdeath)
            {
                Deathfunction();
            }
            else if (!isdeath && !Player_System.player_isdeath)
            {
                float dir = Vector3.Distance(transform.position, Player.transform.position);
                transform.localRotation = Quaternion.RotateTowards(transform.rotation
                , Quaternion.LookRotation((Player.transform.position + Vector3.up * height) - transform.position), 3);
                SHOTBORNE.transform.LookAt(Player.transform.position);
                if (dir > 20)
                {
                    rb.velocity = transform.forward * currentagi;
                }
            }
            EnemyCanvas.transform.LookAt(Player.transform, Vector3.down * 180);
            HPSlider.value = currenthp;
            //銃関係
            if (rate_count >= rapid_fire_rate
                && !Player_System.player_isdeath)
            {
                NomalShot();
                rate_count = 0;
            }
            if (rate_count < rapid_fire_rate) rate_count += 0.2f;
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
        em = transform.parent.transform.parent.GetComponent<Enemy_Manager>();
        Player = em.player_system;
        TEXTPOS = transform.Find("TEXTPOS").gameObject;
        EnemyCanvas = transform.Find("EnemyCanvas").gameObject;
        HPSlider = transform.Find("EnemyCanvas/HPSlider").gameObject.GetComponent<UnityEngine.UI.Slider>();
        rb = GetComponent<Rigidbody>();
        isdeath = false;
        //ステータス反映
        //Statusは特有のステータス以外ScriptableObjectにて改変する事
        height = Random.Range(minheight,maxheight);
        transform.position += Vector3.up * height;
        Debug.Log(transform.position);
        var e_l = enemy_List.Status[0];
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
        enemy_number = 0;
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

        bs.target_tag = "Player";
        bs.damage = bullet_damage;
        bs.deathDistance = bullet_range / 1.5f;
        bs.firstpos = SHOTPOS.transform.position;
        shotObj.transform.eulerAngles = SHOTBORNE.transform.eulerAngles;
        shotObj.transform.eulerAngles += new Vector3(Random.Range(-diffusion_chance, diffusion_chance)
                            , Random.Range(-diffusion_chance, diffusion_chance)
                            , Random.Range(diffusion_chance, diffusion_chance));
        rb.velocity = bs.transform.forward * bullet_speed;
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
        Debug.Log("死亡");
        em.ParentEnemyDeath(enemy_number);
        Destroy(gameObject);
    }
}
