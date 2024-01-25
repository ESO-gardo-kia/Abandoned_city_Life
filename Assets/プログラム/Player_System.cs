using Cinemachine;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UniRx.Triggers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class Player_System : MonoBehaviour
{
    public bool bo = true;
    static public bool move_permit = true;//移動可能か否か
    static public bool player_isdeath = true;//移動可能か否か
    
    [Header("--- GetComponent ---")]
    public CinemachineBrain brain;
    [SerializeField] public Gun_List gunlist;
    private Rigidbody rb;
    [SerializeField] public GameObject CAMERA;
    private GameObject SHOTPOS;
    private GameObject old_dt;
    private float old_damage;

    private GameObject getpanel;

    private GameObject PCanvas;

    private ContactObj_System ConObj;
    private CollectObj_System ColObj;

    private GameObject ContactPanel;
    private Text ContactText;
    private Text BulletText;
    private Slider CollectGage;

    private GameObject MenuPanel;

    private GameObject GamePanel;
    private Slider HPSlider;
    private Text HPText;
    private Slider ENSlider;
    private Text ENText;

    [Header("--- ステータス ---")]
    private float exp;

    public float hp;
    public float currenthp;
    public float atk;
    public float currentatk;
    public float agi;
    public float currentagi;

    [Header("--- 基本動作 ---")]
    public float jumpforce = 6f;
    public float jump_num = 100;
    public float jump_second = 0.1f;

    private bool isJumping = false;//ジャンプ出来るか否か
    private bool isJumping_running = false;//ジャンプ処理中か否か

    public bool isPanel;//何かの画面を開いているか否か

    [Tooltip("移動速度")]
    public float walk_speed = 10;

    [Header("--- 装備品 ---")]
    private GameObject MeleeWeapon;

    public int weapon_id;
    private float rate_count = 0;
    private float loaded_bullets = 0;//弾の最大値
    private float current_loaded_bullets = 0;//現在の残弾
    private float reload_speed = 0;//リロード完了値
    private float reload_count = 0;//リロードカウント
    private bool isreload;
    [SerializeField] public GameObject SHOTOBJ;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        MeleeWeapon = transform.Find("MeleeWeapon").gameObject;
        SHOTPOS = transform.Find("SHOTPOS").gameObject;

        PCanvas = transform.Find("PCanvas").gameObject;
        
        ContactPanel = transform.Find("PCanvas/ContactPanel").gameObject;
        ContactText = ContactPanel.transform.Find("ContactText").gameObject.GetComponent<Text>();
        CollectGage = ContactPanel.transform.Find("CollectGage").gameObject.GetComponent<Slider>();

        MenuPanel = transform.Find("PCanvas/MenuPanel").gameObject;

        GamePanel = transform.Find("PCanvas/GamePanel").gameObject;
        HPSlider = GamePanel.transform.Find("HPSlider").gameObject.GetComponent<Slider>();
        HPText = GamePanel.transform.Find("HPText").gameObject.GetComponent<Text>();
        ENSlider = GamePanel.transform.Find("ENSlider").gameObject.GetComponent<Slider>();
        ENText = GamePanel.transform.Find("ENText").gameObject.GetComponent<Text>();
        BulletText = GamePanel.transform.Find("BulletText").gameObject.GetComponent<Text>();

        Player_Reset(bo);
    }
    void Update()
    {
        this.transform.eulerAngles = new Vector3(0, CAMERA.transform.eulerAngles.y, 0);
        if (Input.GetKeyDown(KeyCode.P)) this.transform.position = Vector3.zero;
        if (move_permit)
        {
            if (Input.GetKeyUp(KeyCode.Space)) isJumping_running = false;
            if (Input.GetKey(KeyCode.Space) //buttonが押されていて
            && !isJumping_running//ジャンプ処理中ではない場合で
            && isJumping)//接地している場合
            {
                Debug.Log("ジャンプ");
                StartCoroutine("JunpMove");
                isJumping = false;
            }

            Ray downray = new Ray(gameObject.transform.position,Vector3.down);
            RaycastHit hit;
            if (Physics.Raycast(downray, out hit, 10.0f))
            {
                var n = hit.point.y - gameObject.transform.position.y + 0.5f;
                if (Mathf.Round(n) == 0 && n < 0.1f && 0.1f > n && !isJumping_running) isJumping = true;
                else isJumping = false;
            }else isJumping = false;
            //Debug.DrawRay(downray.origin, downray.direction * 10, UnityEngine.Color.red, 5);


            if (Input.GetKey(KeyCode.Q))
            {


                if (ColObj != null && CollectGage.value != CollectGage.maxValue) CollectGage.value += 0.02f;
                else if (ColObj != null && CollectGage.value == CollectGage.maxValue) 
                {
                    Player_Manager.Item_Inventory[ColObj.collect_item_id] += ColObj.collect_item_num;
                    Debug.Log(Player_Manager.Item_Inventory[ColObj.collect_item_id]);
                    
                    ContactPanel.SetActive(false);
                    ContactPanel.GetComponent<Image>().color = new UnityEngine.Color(0, 0, 0, 0);
                    CollectGage.value = 0;
                    ColObj.CollectObj_function();
                    ColObj = null;
                    ContactText.text = null;
                }
            }

        }

        //以下UI関係
        HPSlider.value = currenthp;
        HPText.text = currenthp.ToString();
        BulletText.text = current_loaded_bullets.ToString();
        if (getpanel)
        {
            if (Input.GetKeyDown(KeyCode.E) && ConObj != null && !getpanel.activeSelf && !isPanel)
            {
                Canvas_Transition(getpanel, true);
            }
            else if (Input.GetKeyDown(KeyCode.E) && getpanel.activeSelf && isPanel)
            {
                Canvas_Transition(getpanel, false);
            }
        }
        if (Input.GetKeyDown(KeyCode.Tab) && !MenuPanel.activeSelf && move_permit && !isPanel)
        {
            Canvas_Transition(MenuPanel, true);
        }
        else if (Input.GetKeyDown(KeyCode.Tab) && MenuPanel.activeSelf)
        {
            Canvas_Transition(MenuPanel,false);
        }
    }
    void FixedUpdate()
    {
        if (move_permit)
        {
            //銃関係
            if (rate_count >= gunlist.Performance[weapon_id].rapid_fire_rate 
                && current_loaded_bullets > 0 
                && !isreload
                && Input.GetMouseButton(0))
            {
                Debug.Log("発射しました");
                NomalShot();
                current_loaded_bullets--;
                rate_count = 0;
            }
            if (current_loaded_bullets < loaded_bullets
                && Input.GetKey(KeyCode.R)) isreload = true;
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
            else if(rate_count < gunlist.Performance[weapon_id].rapid_fire_rate) rate_count += 0.2f;
            //物理攻撃
            if (Input.GetKey(KeyCode.R)) MeleeAttack();
            //移動処理
            float x = Input.GetAxisRaw("Horizontal"); // x方向のキー入力
            float z = Input.GetAxisRaw("Vertical"); // z方向のキー入力
            Vector3 Player_movedir = new Vector3(x, rb.velocity.y, z).normalized; // 正規化
            Player_movedir = this.transform.forward * z + this.transform.right * x;
            rb.velocity = new Vector3(Player_movedir.x * walk_speed, rb.velocity.y, Player_movedir.z * walk_speed);
        }
    }
    void OnTriggerStay(Collider other)
    {
        //if (other.gameObject.CompareTag("Floor")) isJumping = true;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Floor")) isJumping_running = false;
        if (other.gameObject.CompareTag("Collect"))
        {
            ContactPanel.SetActive(true);
            ContactPanel.GetComponent<Image>().color = new UnityEngine.Color(255, 255, 255, 50);
            ColObj = other.GetComponent<CollectObj_System>();
            ContactText.text = ColObj.collect_text;
            CollectGage.maxValue = ColObj.collect_time;
            CollectGage.value = 0;
        }
        if (other.gameObject.CompareTag("Contact"))
        {
            ContactPanel.SetActive(true);
            ContactPanel.GetComponent<Image>().color = new UnityEngine.Color(255, 255, 255, 50);
            ConObj = other.GetComponent<ContactObj_System>();
            if (ConObj.contact_text != null) ContactText.text = ConObj.contact_text;
            getpanel = ConObj.Panel;
        }
        if (move_permit)
        {
            if (other.gameObject.CompareTag("Bullet")
                && other.GetComponent<Bullet_System>().target_tag == "Player")
            {
                TakeDmage(other.GetComponent<Bullet_System>().damage,
                    other.GetComponent<Bullet_System>());
            }

        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Floor")) isJumping = false;
        if (other.gameObject.CompareTag("Collect"))
        {
            ContactPanel.SetActive(false);
            ContactPanel.GetComponent<Image>().color = new UnityEngine.Color(0, 0, 0, 0);
            ColObj = null;
            ContactText.text = null;
        }
        if (other.gameObject.CompareTag("Contact"))
        {
            if (getpanel.activeSelf) Canvas_Transition(getpanel, false);
            ContactPanel.SetActive(false);
            ContactPanel.GetComponent<Image>().color = new UnityEngine.Color(0, 0, 0, 0);
            ConObj = null;
            ContactText.text = null;
            getpanel = null;
        }
    }
    void TakeDmage(float damage,Bullet_System BS)
    {
        if (currenthp > 0) currenthp -= damage;
        if (currenthp <= 0)
        {
            player_isdeath = true;
            transform.root.GetComponent<GameManager>().Scene_Transition_Process(1);
        }
    }
    IEnumerator JunpMove()
    {
        rb.velocity = Vector3.zero;
        isJumping_running = true;
        float ju_fo = jumpforce;
        for (int i = 0; i <= jump_num && isJumping_running == true; i++)
        {
            //Debug.Log("jump処理");
            if (Input.GetKey(KeyCode.Space) && i == 0)
            {
                rb.AddForce(Vector3.up * (jumpforce), ForceMode.Impulse);
                yield return new WaitForSeconds(jump_second);
            }
            if (Input.GetKey(KeyCode.Space) && i != 0)
            {
                ju_fo = ju_fo / 1.5f;
                rb.AddForce(Vector3.up * (ju_fo), ForceMode.Impulse);
                yield return new WaitForSeconds(jump_second);
            }
        }
    }
    public void NomalShot()
    {
        
        GameObject shotObj = Instantiate(SHOTOBJ,SHOTPOS.transform.position,Quaternion.identity);
        Rigidbody rb = shotObj.GetComponent<Rigidbody>();
        Bullet_System bs = shotObj.GetComponent<Bullet_System>();
        var Guns = gunlist.Performance;

        bs.target_tag = "Enemy";
        bs.damage = Guns[1].bullet_damage;
        bs.death_time = Guns[1].bullet_range;
        rb.velocity = CAMERA.transform.forward * Guns[1].bullet_speed;
        shotObj.transform.eulerAngles = CAMERA.transform.eulerAngles;
        //shotObj.transform.eulerAngles = this.transform.eulerAngles + new Vector3(0, 0, -90);
    }
    public void MeleeAttack()
    {
        Rigidbody rb = MeleeWeapon.GetComponent<Rigidbody>();
    }
    public void Player_Reset(bool IS)
    {
        if (!IS)
        {
            move_permit = false;
            brain.enabled = false;
            GamePanel.SetActive(false);
        }
        else
        {
            move_permit = true;
            brain.enabled = true;
            GamePanel.SetActive(true);
        }
        MenuPanel.transform.localScale = Vector3.zero;
        ContactPanel.SetActive(false);
        MenuPanel.SetActive(false);
        ContactText.text = null;
        player_isdeath = false;
        currenthp = hp;
        currentatk = atk;
        currentagi = agi;
        HPSlider.maxValue = hp;
        HPSlider.value = currenthp;

        Wepon_Reset();
    }
    public void Wepon_Reset()
    {
        isreload = false;
        reload_count = 0;
        var Guns = gunlist.Performance;
        loaded_bullets = Guns[weapon_id].loaded_bullets;
        current_loaded_bullets = Guns[weapon_id].loaded_bullets;
        reload_speed = Guns[weapon_id].reload_speed;
    }
    public void Canvas_Transition(GameObject Panel,bool IS)
    {
        /*
         * trueが画面を開く時の処理
         * falseが画面を閉じる時の処理
         */
        if (IS)
        {
            brain.enabled = false;
            move_permit = false;
            isPanel = true;
            Panel.SetActive(true);
            DOTween.Sequence()
                .Append(Panel.GetComponent<RectTransform>().DOScale(Vector3.one, 0.25f)
                .SetEase(Ease.OutSine)
                .OnComplete(() => {
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.Confined;
                }))
                .Play();
        }
        else
        {
            DOTween.Sequence()
                .Append(Panel.GetComponent<RectTransform>().DOScale(Vector3.zero, 0.25f)
                .SetEase(Ease.OutSine)
              　.OnComplete(() => {
                  Cursor.visible = false;
                  Cursor.lockState = CursorLockMode.Locked;
                  brain.enabled = true;
                  move_permit = true;
                  isPanel = false;
                  Panel.SetActive(false);
                  Debug.Log("終了");
                }))
                .Play();
        }
    }
}
