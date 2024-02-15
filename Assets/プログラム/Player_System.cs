using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static ContactObj_System;

public class Player_System : MonoBehaviour
{
    public bool bo = true;
    static public bool move_permit = true;//移動可能か否か
    static public bool player_isdeath = true;//移動可能か否か
    [Space(10)]
    [SerializeField][Header("--- GetComponent ---")]
    public CinemachineBrain brain;
    [SerializeField] public Gun_List gunlist;
    private Rigidbody rb;
    [SerializeField] public GameObject CAMERA;
    [SerializeField] public CinemachineVirtualCamera CVC;
    private GameObject SHOTPOS;
    public GameObject wheel;
    private GameObject old_dt;
    private float old_damage;

    private GameObject getpanel;

    private GameObject PCanvas;

    private ContactObj_System ConObj;
    private CollectObj_System ColObj;

    private GameObject ContactPanel;
    private Text ContactText;
    private Slider CollectGage;

    private GameObject MenuPanel;
    private Slider AimSlider;

    public GameObject GamePanel;
    private Slider ReloadSlider;
    private Slider HPSlider;
    private Text HPText;
    private Slider ENSlider;
    private Text ENText;
    private Image WeaponImage;
    private Text BulletText;

    public GameObject money_text;
    [Space(10)]
    [SerializeField]
    [Header("--- サウンド ---")]
    private AudioSource AS;
    public AudioClip panel_sound;
    private AudioClip shot_sound;
    public AudioClip damage_sound;
    [Space(10)]
    [SerializeField]
    [Header("--- ステータス ---")]
    public bool isen;
    private float en_recovery;
    public float en_recovery_max;

    public float hp;
    private float currenthp;
    public float en;
    private float currenten;
    public float atk;
    private float currentatk;
    public float agi;
    private float currentagi;
    [Space(10)]
    [SerializeField]
    [Header("--- 基本動作 ---")]
    //近接攻撃の無敵時間
    private float adi_time;
    public float adi_time_max;

    public float jumpforce = 6f;
    public float jump_num = 100;
    public float jump_second = 0.1f;

    private bool isJumping = false;//ジャンプ出来るか否か
    private bool isJumping_running = false;//ジャンプ処理中か否か



    public bool isPanel;//何かの画面を開いているか否か

    [Tooltip("移動速度")]
    public float walk_speed = 10;
    public float dash_speed = 15;

    [Header("--- 装備品 ---")]
    private GameObject MeleeWeapon;

    public static int player_weapon_id;
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
        AS = GetComponent<AudioSource>();

        SHOTPOS = transform.Find("SHOTPOS").gameObject;

        PCanvas = transform.Find("PCanvas").gameObject;
        
        ContactPanel = transform.Find("PCanvas/ContactPanel").gameObject;
        ContactText = ContactPanel.transform.Find("ContactText").gameObject.GetComponent<Text>();
        CollectGage = ContactPanel.transform.Find("CollectGage").gameObject.GetComponent<Slider>();

        MenuPanel = transform.Find("PCanvas/MenuPanel").gameObject;
        AimSlider = MenuPanel.transform.Find("AimSlider").gameObject.GetComponent<Slider>();

        GamePanel = transform.Find("PCanvas/GamePanel").gameObject;
        ReloadSlider = GamePanel.transform.Find("ReloadSlider").gameObject.GetComponent<Slider>();
        HPSlider = GamePanel.transform.Find("HPSlider").gameObject.GetComponent<Slider>();
        HPText = GamePanel.transform.Find("HPText").gameObject.GetComponent<Text>();
        ENSlider = GamePanel.transform.Find("ENSlider").gameObject.GetComponent<Slider>();
        ENText = GamePanel.transform.Find("ENText").gameObject.GetComponent<Text>();
        BulletText = GamePanel.transform.Find("BulletText").gameObject.GetComponent<Text>();
        WeaponImage = GamePanel.transform.Find("WeaponImage").gameObject.GetComponent<Image>();

        money_text = transform.Find("PCanvas/money_text").gameObject;

        Player_Reset(bo);
    }
    void Update()
    {
        if(!isPanel) transform.eulerAngles = new Vector3(0, CAMERA.transform.eulerAngles.y, 0);
        if (Input.GetKeyDown(KeyCode.P)) this.transform.position = Vector3.zero;
        if (move_permit)
        {
            //if (Input.GetKeyDown(KeyCode.LeftShift)) StartCoroutine("DashMove");

            if (Input.GetKeyUp(KeyCode.Space)) isJumping_running = false;
            if (Input.GetKey(KeyCode.Space) //buttonが押されていて
            && !isJumping_running//ジャンプ処理中ではない場合で
            && isJumping)//接地している場合
            {
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
            if (rb.velocity != Vector3.zero) wheel.transform.Rotate(Vector3.up, rb.velocity.magnitude * 2);
        }

        //以下UI関係
        ReloadSlider.value = reload_count;
        HPSlider.value = currenthp;
        HPText.text = currenthp.ToString();
        ENSlider.value = currenten;
        ENText.text = currenten.ToString();
        BulletText.text = current_loaded_bullets.ToString();
        if (getpanel)
        {
            if (Input.GetKeyDown(KeyCode.E) && ConObj != null && !getpanel.activeSelf && !isPanel)
            {
                Canvas_Transition(ConObj,getpanel, true);
            }
            else if (Input.GetKeyDown(KeyCode.E) && getpanel.activeSelf && isPanel)
            {
                Canvas_Transition(ConObj,getpanel, false);
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
            if (adi_time < adi_time_max) adi_time += 0.02f;
            //銃関係
            if (rate_count >= gunlist.Data[player_weapon_id].rapid_fire_rate && current_loaded_bullets > 0 && !isreload&& Input.GetMouseButton(0))
            {
                //Debug.Log("発射しました");
                NomalShot();
                current_loaded_bullets--;
                rate_count = 0;
            }
            if ((current_loaded_bullets < loaded_bullets && Input.GetKey(KeyCode.R))
                || (current_loaded_bullets < loaded_bullets && current_loaded_bullets == 0 && Input.GetMouseButton(0)))
            {
                isreload = true;
                ReloadSlider.gameObject.SetActive(true);
            }
            if (isreload)
            {
                reload_count += 0.2f;
                if (reload_count >= reload_speed)
                {
                    ReloadSlider.gameObject.SetActive(false);
                    current_loaded_bullets = loaded_bullets;
                    reload_count = 0;
                    isreload = false;
                }
            }
            else if(rate_count < gunlist.Data[player_weapon_id].rapid_fire_rate) rate_count += 0.2f;
            //移動処理
            if (Input.GetKey(KeyCode.LeftShift)&& currenten > 0)
            {
                if (0 < currenten)
                {
                    isen = false;
                    currenten--;
                }
                float x = Input.GetAxisRaw("Horizontal"); // x方向のキー入力
                float z = Input.GetAxisRaw("Vertical"); // z方向のキー入力
                Vector3 Player_movedir = new Vector3(x, rb.velocity.y, z).normalized; // 正規化
                Player_movedir = this.transform.forward * z + this.transform.right * x;
                rb.velocity = new Vector3(Player_movedir.x * dash_speed, rb.velocity.y, Player_movedir.z * dash_speed);
            }
            else
            {
                if(ENSlider.maxValue > currenten && isen) currenten++;
                if (!isen)
                {
                    en_recovery += 0.2f;
                    if (en_recovery >= en_recovery_max)
                    {
                        en_recovery = 0;
                        isen = true;
                    }
                }
                float x = Input.GetAxisRaw("Horizontal"); // x方向のキー入力
                float z = Input.GetAxisRaw("Vertical"); // z方向のキー入力
                Vector3 Player_movedir = new Vector3(x, rb.velocity.y, z).normalized; // 正規化
                Player_movedir = this.transform.forward * z + this.transform.right * x;
                rb.velocity = new Vector3(Player_movedir.x * walk_speed, rb.velocity.y, Player_movedir.z * walk_speed);
            }
        }
    }
    void OnTriggerStay(Collider other)
    {

        if (other.gameObject.CompareTag("Saw")&& adi_time >= adi_time_max && !player_isdeath)
        {
            adi_time = 0;
            TakeDmage(other.GetComponent<Assault_Enemy>().currentatk);
        }
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
                && other.GetComponent<Bullet_System>().target_tag == "Player" && !player_isdeath)
            {
                TakeDmage(other.GetComponent<Bullet_System>().damage);
                other.GetComponent<Bullet_System>().BulletDestroy();
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
            if (Contact_Type.Production_Table == ConObj.Cont) ConObj.VC.Priority = 1;
            if (getpanel.activeSelf) Canvas_Transition(getpanel, false);
            ContactPanel.SetActive(false);
            ContactPanel.GetComponent<Image>().color = new UnityEngine.Color(0, 0, 0, 0);
            ConObj = null;
            ContactText.text = null;
            getpanel = null;
        }
    }
    void TakeDmage(float damage)
    {
        if (currenthp > 0)
        {
            AS.PlayOneShot(damage_sound);
            currenthp -= damage;
        }
        if (currenthp <= 0)
        {
            player_isdeath = true;
            GameObject.Find("Enemy_Manager").GetComponent<Enemy_Manager>().Player_Death();
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
    IEnumerator DashMove()
    {
        for(int i = 0 ; i < 3 ; i++)
        {
            rb.AddForce(transform.forward * dash_speed, ForceMode.Impulse);
            yield return new WaitForSeconds(0.01f);
        }
    }
    public void NomalShot()
    {
        AS.PlayOneShot(shot_sound);
        var Guns = gunlist.Data[player_weapon_id];
        for (int i = 0 ; i < Guns.multi_bullet ; i++)
        {
            GameObject shotObj = Instantiate(SHOTOBJ, SHOTPOS.transform.position, Quaternion.identity);
            Rigidbody rb = shotObj.GetComponent<Rigidbody>();
            Bullet_System bs = shotObj.GetComponent<Bullet_System>();

            bs.type = (Bullet_System.Bullet_Type)Guns.type;
            bs.target_tag = "Enemy";
            bs.damage = Guns.bullet_damage;
            bs.death_dis = Guns.bullet_range;
            bs.firstpos = SHOTPOS.transform.position;
            shotObj.transform.eulerAngles = CAMERA.transform.eulerAngles;
            shotObj.transform.eulerAngles += new Vector3(Random.Range(-Guns.diffusion__chance, Guns.diffusion__chance)
                                , Random.Range(-Guns.diffusion__chance, Guns.diffusion__chance)
                                , Random.Range(-Guns.diffusion__chance, Guns.diffusion__chance));
            rb.velocity = bs.transform.forward * Guns.bullet_speed;
        }
    }
    public void Player_Reset(bool IS)
    {
        Debug.Log("プレイヤー情報がリセットされました");
        if (!IS)
        {
            move_permit = false;
            brain.enabled = false;
            GamePanel.SetActive(false);
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            move_permit = true;
            brain.enabled = true;
            GamePanel.SetActive(true);
        }
        isPanel = false;
        MenuPanel.transform.localScale = Vector3.zero;
        ContactPanel.SetActive(false);
        MenuPanel.SetActive(false);
        ReloadSlider.gameObject.SetActive(false);
        ContactText.text = null;
        player_isdeath = false;
        currenthp = hp;
        currenten = en;
        currentatk = atk;
        currentagi = agi;
        HPSlider.maxValue = hp;
        HPSlider.value = currenthp;
        ENSlider.maxValue = en;
        ENSlider.value = currenten;
        transform.localRotation = new Quaternion(0,0,0,0);
        rb.velocity = Vector3.zero;
        Wepon_Reset(player_weapon_id);
        Debug.Log("プレイヤー情報がリセットしおわりました");
    }
    public void Wepon_Reset(int num)
    {
        isreload = false;
        reload_count = 0;
        var Guns = gunlist.Data[num];
        WeaponImage.sprite = Guns.sprite_id;
        shot_sound = Guns.shot_sound;
        loaded_bullets = Guns.loaded_bullets;
        current_loaded_bullets = Guns.loaded_bullets;
        reload_speed = Guns.reload_speed;
        ReloadSlider.maxValue = reload_speed;
    }
    public void Canvas_Transition(GameObject Panel, bool IS)
    {
        /*
         * trueが画面を開く時の処理
         * falseが画面を閉じる時の処理
         */
        if (IS)
        {
            AS.PlayOneShot(panel_sound);
            //brain.enabled = false;
            move_permit = false;
            isPanel = true;
            Panel.SetActive(true);
            DOTween.Sequence()
                .Append(Panel.GetComponent<RectTransform>().DOScale(Vector3.one, 0.25f)
                .SetEase(Ease.OutCirc)
                .OnComplete(() => {
                    //brain.enabled = false;
                    move_permit = false;
                    isPanel = true;
                    Panel.SetActive(true);
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.Confined;
                }))
                .Play();
        }
        else
        {
            AS.PlayOneShot(panel_sound);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            //brain.enabled = true;
            move_permit = true;
            DOTween.Sequence()
                .Append(Panel.GetComponent<RectTransform>().DOScale(Vector3.zero, 0.25f)
                .SetEase(Ease.OutCirc)
              .OnComplete(() => {
                  Cursor.visible = false;
                  Cursor.lockState = CursorLockMode.Locked;
                  //brain.enabled = true;
                  move_permit = true;
                  isPanel = false;
                  Panel.SetActive(false);
                  Debug.Log("終了");
              }))
                .Play();
        }
    }
    public void Canvas_Transition(ContactObj_System con,GameObject Panel,bool IS)
    {
        /*
         * trueが画面を開く時の処理
         * falseが画面を閉じる時の処理
         */
        if (IS)
        {
            //brain.enabled = false;
            move_permit = false;
            isPanel = true;
            AS.PlayOneShot(panel_sound);
            switch (ConObj.Cont)
            {
                case Contact_Type.Production_Table:
                    money_text.SetActive(true);
                    money_text.GetComponent<Text>().text = "MONEY:" + GameManager.Money.ToString();
                    rb.useGravity = false;
                    rb.velocity = Vector3.zero;
                    transform.position = ConObj.idlepos.transform.position;
                    transform.localEulerAngles = ConObj.idlepos.transform.localEulerAngles;
                    ConObj.VC.Priority = 100;
                    break;
                case Contact_Type.StageSelect:
                    rb.useGravity = false;
                    rb.velocity = Vector3.zero;
                    transform.position = ConObj.idlepos.transform.position;
                    transform.localEulerAngles = ConObj.idlepos.transform.localEulerAngles;
                    ConObj.VC.Priority = 100;
                    break;
            }

            con.Canvas_Open();
            DOTween.Sequence()
                .Append(Panel.GetComponent<RectTransform>().DOScale(Vector3.one, 0.25f)
                .SetEase(Ease.OutCirc)
                .OnComplete(() => {
                    //brain.enabled = false;
                    move_permit = false;
                    isPanel = true;
                    Panel.SetActive(true);
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.Confined;
                }))
                .Play();
        }
        else
        {
            AS.PlayOneShot(panel_sound);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            //brain.enabled = true;
            move_permit = true;
            DOTween.Sequence()
                .Append(Panel.GetComponent<RectTransform>().DOScale(Vector3.zero, 0.25f)
                .SetEase(Ease.OutCirc)
              　.OnComplete(() => {
                  rb.useGravity = true;
                  money_text.SetActive(false);
                  ConObj.VC.Priority = 1;
                  Cursor.visible = false;
                  Cursor.lockState = CursorLockMode.Locked;
                  //brain.enabled = true;
                  move_permit = true;
                  isPanel = false;
                  con.Canvas_Close();
                  Debug.Log("終了");
                }))
                .Play();
        }
    }
}
