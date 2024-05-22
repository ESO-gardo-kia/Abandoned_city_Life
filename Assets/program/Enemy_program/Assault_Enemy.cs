using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Assault_Enemy : MonoBehaviour
{
    public Enemy_Manager enemyManager;
    [SerializeField] public GameObject Player;
    [SerializeField] public Gun_List gunlist;
    public GameObject wheel;

    private GameObject EnemyCanvas;
    private Slider HPSlider;
    [SerializeField] public GameObject TEXTOBJ;
    private GameObject TEXTPOS;
    private GameObject oldDamageText;
    private float old_damage;

    private NavMeshAgent NMA;
    private Rigidbody rb;

    [Header("--- �����i ---")]
    public AudioClip shotsound;
    public GameObject circular_saw;
    public float attack_damage;
    [Header("--- �X�e�[�^�X ---")]
    [SerializeField] private Enemy_List enemy_List;
    private int enemy_number;
    private bool isdeath;

    private string Ename;
    private float exp;

    private float hp;
    private float currenthp;
    private float atk;
    public float currentatk;
    private float agi;
    private float currentagi;

    void Start()
    {
        Enemy_Reset();
    }

    void FixedUpdate()
    {
        circular_saw.transform.Rotate(Vector3.up,5);
        if (rb.velocity != Vector3.zero) wheel.transform.Rotate(Vector3.up, rb.velocity.magnitude * 3);
        if (Enemy_Manager.enemies_move_permit == true)
        {
            if (isdeath)
            {
                Deathfunction();
            }
            else if (!isdeath && !Player_System.playerIsDeath)
            {
                NMA.destination = Player.transform.position;

                transform.localRotation = Quaternion.RotateTowards(transform.rotation
                    , Quaternion.LookRotation(Player.transform.position - transform.position)
                    , 3);
            }
            EnemyCanvas.transform.LookAt(Player.transform, Vector3.down * 180);

            HPSlider.value = currenthp;
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
                /*
                //�U���o�����Ԃ��U���Ώۂ̓G�l�~�[�̎�
                if (other.GetComponent<Attack_System>().isAttack && other.GetComponent<Attack_System>().attack_subject == Attack_System.Attack_Subject.Enemy)
                {
                    TakeDmage(other.GetComponent<Attack_System>().damage, other, other.GetComponent<Attack_System>());
                }
                */
            }
        }
    }
    public void Enemy_Reset()
    {
        //Debug.Log("�G�̏������Z�b�g����");
        enemyManager = transform.parent.transform.parent.GetComponent<Enemy_Manager>();
        Player = enemyManager.player_system;
        TEXTPOS = transform.Find("TEXTPOS").gameObject;
        EnemyCanvas = transform.Find("EnemyCanvas").gameObject;
        HPSlider = transform.Find("EnemyCanvas/HPSlider").gameObject.GetComponent<UnityEngine.UI.Slider>();
        NMA = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        isdeath = false;
        //�X�e�[�^�X���f
        //Status��ScriptableObject�ɂĉ��ς��鎖
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
        enemy_number = 1;

        NMA.speed = currentagi;
        NMA.stoppingDistance = 0;
    }
    void TakeDmage(float damage, Bullet_System BS)
    {
        if (oldDamageText == null)
        {
            GameObject damagetext = Instantiate(TEXTOBJ, TEXTPOS.transform.position, HPSlider.transform.rotation, transform.Find("EnemyCanvas"));
            Text t = damagetext.GetComponent<UnityEngine.UI.Text>();
            Damage_Text dts = damagetext.GetComponent<Damage_Text>();
            oldDamageText = damagetext;

            t.text = BS.damage.ToString();
            old_damage = BS.damage;
        }
        else if (oldDamageText != null)
        {
            old_damage += BS.damage;
            oldDamageText.GetComponent<UnityEngine.UI.Text>().text = old_damage.ToString();
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
    /*
    void TakeDmage(float damage, Collider other)
    {
        if (oldDamageText == null)
        {
            GameObject dt = Instantiate(TEXTOBJ, TEXTPOS.transform.position, HPSlider.transform.rotation, transform.Find("EnemyCanvas"));
            UnityEngine.UI.Text t = dt.GetComponent<UnityEngine.UI.Text>();
            Bullet_System bs = other.GetComponent<Bullet_System>();
            Damage_Text dts = dt.GetComponent<Damage_Text>();
            oldDamageText = dt;

            dt.transform.localEulerAngles = new Vector3(0, 180, 180);
            t.text = bs.damage.ToString();
            old_damage = bs.damage;
        }
        else if (oldDamageText != null)
        {
            old_damage += other.GetComponent<Bullet_System>().damage;
            oldDamageText.GetComponent<UnityEngine.UI.Text>().text = old_damage.ToString();
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
    */
    void Deathfunction()
    {
        Debug.Log("���S");
        enemyManager.ParentEnemyDeath(enemy_number);
        Destroy(gameObject);
    }
}
