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
    static public bool movePermit = true;//移動可能か否か
    static public bool playerIsDeath = true;//移動可能か否か
    [Space(10)]
    [Header("--- GetComponent ---")]
    [SerializeField] private CinemachineBrain cinemachinBrain;
    [SerializeField] public Gun_List gunlist;
    private Rigidbody rigidBody;
    [SerializeField] public GameObject playerCamera;
    [SerializeField] public CinemachineVirtualCamera cinemachineVirtualCamera;
    private GameObject shotPosition;
    public GameObject wheel;

    private GameObject contactObjGetPanel;

    private GameObject playerCanvas;

    private ContactObj_System contactObjSystem;
    private CollectObj_System collectObjSystem;

    private GameObject contactPanel;
    private Text contactText;
    [SerializeField] private Slider CollectGage;

    private GameObject MenuPanel;
    private Slider AimSlider;

    [SerializeField] private GameObject battleInfomationPanel;
    [SerializeField] private Slider reloadSlider;
    [SerializeField] private Slider hpSlider;
    [SerializeField] private Text hpText;
    [SerializeField] private Slider enSlider;
    [SerializeField] private Text enText;
    private Image weaponImage;
    [SerializeField] private Text bulletText;

    public GameObject moneyText;
    [Space(10)]
    [Header("--- サウンド ---")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip panelSound;
    [SerializeField] private AudioClip shotSound;
    [SerializeField] private AudioClip damageSound;
    [Space(10)]
    [SerializeField]
    [Header("--- ステータス ---")]
    public bool isEnergyRecovery;
    public float energyRecoveryTime;
    private float energyRecoveryCount;

    public float hp;
    private float currentHp;
    public float en;
    private float currentEn;
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

    public float jumpForce = 6f;
    public float jumpNum = 100;
    public float jumpRepeatSecond = 0.1f;

    private bool isJumping = false;//ジャンプ出来るか否か
    private bool isJumpingRunning = false;//ジャンプ処理中か否か

    public static bool isPanelOpen;

    [Tooltip("移動速度")]
    public float walkSpeed = 10;
    public float dashSpeed = 15;

    [Header("--- 装備品 ---")]
    private GameObject MeleeWeapon;

    public static int player_weapon_id;
    private float rate_count = 0;
    private float loaded_bullets = 0;//弾の最大値
    private float current_loaded_bullets = 0;//現在の残弾
    private float reload_speed = 0;//リロード完了値
    private float reload_count = 0;//リロードカウント
    private bool isReloadPossible;
    [SerializeField] public GameObject SHOTOBJ;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        shotPosition = transform.Find("SHOTPOS").gameObject;

        playerCanvas = transform.Find("PCanvas").gameObject;
        
        contactPanel = transform.Find("PCanvas/ContactPanel").gameObject;
        contactText = contactPanel.transform.Find("ContactText").gameObject.GetComponent<Text>();
        CollectGage = contactPanel.transform.Find("CollectGage").gameObject.GetComponent<Slider>();

        MenuPanel = transform.Find("PCanvas/MenuPanel").gameObject;
        AimSlider = MenuPanel.transform.Find("AimSlider").gameObject.GetComponent<Slider>();

        battleInfomationPanel = transform.Find("PCanvas/GamePanel").gameObject;
        reloadSlider = battleInfomationPanel.transform.Find("ReloadSlider").gameObject.GetComponent<Slider>();
        hpSlider = battleInfomationPanel.transform.Find("HPSlider").gameObject.GetComponent<Slider>();
        hpText = battleInfomationPanel.transform.Find("HPText").gameObject.GetComponent<Text>();
        enSlider = battleInfomationPanel.transform.Find("ENSlider").gameObject.GetComponent<Slider>();
        enText = battleInfomationPanel.transform.Find("ENText").gameObject.GetComponent<Text>();
        bulletText = battleInfomationPanel.transform.Find("BulletText").gameObject.GetComponent<Text>();
        weaponImage = battleInfomationPanel.transform.Find("WeaponImage").gameObject.GetComponent<Image>();

        moneyText = transform.Find("PCanvas/money_text").gameObject;

        Player_Reset(bo);
    }
    void Update()
    {
        moneyText.SetActive(true);
        moneyText.GetComponent<Text>().text = "MONEY:" + GameManager.playerMoney.ToString();
        if (!isPanelOpen) transform.eulerAngles = new Vector3(0, playerCamera.transform.eulerAngles.y, 0);
        if (Input.GetKeyDown(KeyCode.P)) this.transform.position = Vector3.zero;
        if (movePermit)
        {
            //if (Input.GetKeyDown(KeyCode.LeftShift)) StartCoroutine("DashMove");

            if (Input.GetKeyUp(KeyCode.Space)) isJumpingRunning = false;
            if (Input.GetKey(KeyCode.Space) //buttonが押されていて
            && !isJumpingRunning//ジャンプ処理中ではない場合で
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
                if (Mathf.Round(n) == 0 && n < 0.1f && 0.1f > n && !isJumpingRunning) isJumping = true;
                else isJumping = false;
            }else isJumping = false;
            //Debug.DrawRay(downray.origin, downray.direction * 10, UnityEngine.Color.red, 5);


            if (Input.GetKey(KeyCode.Q))
            {


                if (collectObjSystem != null && CollectGage.value != CollectGage.maxValue) CollectGage.value += 0.02f;
                else if (collectObjSystem != null && CollectGage.value == CollectGage.maxValue) 
                {
                    Player_Manager.Item_Inventory[collectObjSystem.collect_item_id] += collectObjSystem.collect_item_num;
                    Debug.Log(Player_Manager.Item_Inventory[collectObjSystem.collect_item_id]);
                    
                    contactPanel.SetActive(false);
                    contactPanel.GetComponent<Image>().color = new UnityEngine.Color(0, 0, 0, 0);
                    CollectGage.value = 0;
                    collectObjSystem.CollectObj_function();
                    collectObjSystem = null;
                    contactText.text = null;
                }
            }
            if (rigidBody.velocity != Vector3.zero) wheel.transform.Rotate(Vector3.up, rigidBody.velocity.magnitude * 2);
        }

        //以下UI関係
        reloadSlider.value = reload_count;
        hpSlider.value = currentHp;
        hpText.text = currentHp.ToString();
        enSlider.value = currentEn;
        enText.text = currentEn.ToString();
        bulletText.text = current_loaded_bullets.ToString();
        if (contactObjGetPanel)
        {
            /*
            if (Input.GetKeyDown(KeyCode.E) && contactObjSystem != null && !contactObjGetPanel.activeSelf && !isPanelOpen)
            {
                Canvas_Transition(contactObjSystem,contactObjGetPanel, true);
            }
            else if (Input.GetKeyDown(KeyCode.E) && contactObjGetPanel.activeSelf && isPanelOpen)
            {
                Canvas_Transition(contactObjSystem,contactObjGetPanel, false);
            }
            */
        }
        if (Input.GetKeyDown(KeyCode.Tab) && !MenuPanel.activeSelf && movePermit && !isPanelOpen)
        {
            //Canvas_Transition(MenuPanel, true);
        }
        else if (Input.GetKeyDown(KeyCode.Tab) && MenuPanel.activeSelf)
        {
            //Canvas_Transition(MenuPanel,false);
        }
    }
    void FixedUpdate()
    {
        if (movePermit)
        {
            if (adi_time < adi_time_max) adi_time += 0.02f;
            //銃関係
            if (rate_count >= gunlist.Data[player_weapon_id].rapid_fire_rate && current_loaded_bullets > 0 && !isReloadPossible&& Input.GetMouseButton(0))
            {
                //Debug.Log("発射しました");
                NomalShot();
                current_loaded_bullets--;
                rate_count = 0;
            }
            if ((current_loaded_bullets < loaded_bullets && Input.GetKey(KeyCode.R))
                || (current_loaded_bullets < loaded_bullets && current_loaded_bullets == 0 && Input.GetMouseButton(0)))
            {
                isReloadPossible = true;
                reloadSlider.gameObject.SetActive(true);
            }
            if (isReloadPossible)
            {
                reload_count += 0.2f;
                if (reload_count >= reload_speed)
                {
                    reloadSlider.gameObject.SetActive(false);
                    current_loaded_bullets = loaded_bullets;
                    reload_count = 0;
                    isReloadPossible = false;
                }
            }
            else if(rate_count < gunlist.Data[player_weapon_id].rapid_fire_rate) rate_count += 0.2f;
            //移動処理
            if (Input.GetKey(KeyCode.LeftShift)&& currentEn > 0)
            {
                if (0 < currentEn)
                {
                    isEnergyRecovery = false;
                    currentEn--;
                }
                PlayerMove(dashSpeed);
            }
            else
            {
                if(enSlider.maxValue > currentEn && isEnergyRecovery) currentEn++;
                if (!isEnergyRecovery)
                {
                    energyRecoveryCount += 0.2f;
                    if (energyRecoveryCount >= energyRecoveryTime)
                    {
                        energyRecoveryCount = 0;
                        isEnergyRecovery = true;
                    }
                }
                PlayerMove(walkSpeed);
            }
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Saw")&& adi_time >= adi_time_max && !playerIsDeath)
        {
            adi_time = 0;
            TakeDmage(other.GetComponent<Assault_Enemy>().currentatk);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Floor")) isJumpingRunning = false;
        if (other.gameObject.CompareTag("Collect"))
        {
            contactPanel.SetActive(true);
            contactPanel.GetComponent<Image>().color = new UnityEngine.Color(255, 255, 255, 50);
            collectObjSystem = other.GetComponent<CollectObj_System>();
            contactText.text = collectObjSystem.collect_text;
            CollectGage.maxValue = collectObjSystem.collect_time;
            CollectGage.value = 0;
        }
        if (other.gameObject.CompareTag("Contact"))
        {
            /*
            contactPanel.SetActive(true);
            contactPanel.GetComponent<Image>().color = new UnityEngine.Color(255, 255, 255, 50);
            contactObjSystem = other.GetComponent<ContactObj_System>();
            if (contactObjSystem.contact_text != null) contactText.text = contactObjSystem.contact_text;
            contactObjGetPanel = contactObjSystem.Panel;
            */
        }
        if (movePermit)
        {
            if (other.gameObject.CompareTag("Bullet") && !playerIsDeath)
            {
                if (other.GetComponent<NormalBulletSystem>() != null && other.GetComponent<NormalBulletSystem>().targetTag == "Player")
                {
                    TakeDmage(other.GetComponent<NormalBulletSystem>().bulletDamage);
                    other.GetComponent<NormalBulletSystem>().BulletDestroy();
                }
                else if (other.GetComponent<FollowingBulletSystem>() != null && other.GetComponent<FollowingBulletSystem>().targetTag == "Player")
                {
                    TakeDmage(other.GetComponent<FollowingBulletSystem>().bulletDamage);
                    other.GetComponent<FollowingBulletSystem>().BulletDestroy();
                }
                else if (other.GetComponent<ParabolaBulletSystem>() != null && other.GetComponent<ParabolaBulletSystem>().targetTag == "Player")
                {
                    TakeDmage(other.GetComponent<ParabolaBulletSystem>().bulletDamage);
                    other.GetComponent<ParabolaBulletSystem>().BulletDestroy();
                }
                else if (other.GetComponent<SplitBulletSystem>() != null && other.GetComponent<SplitBulletSystem>().targetTag == "Player")
                {
                    TakeDmage(other.GetComponent<SplitBulletSystem>().bulletDamage);
                    other.GetComponent<SplitBulletSystem>().BulletDestroy();
                }
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Floor")) isJumping = false;
        if (other.gameObject.CompareTag("Collect"))
        {
            contactPanel.SetActive(false);
            contactPanel.GetComponent<Image>().color = new UnityEngine.Color(0, 0, 0, 0);
            collectObjSystem = null;
            contactText.text = null;
        }
        if (other.gameObject.CompareTag("Contact"))
        {
            //if (Contact_Type.Production_Table == contactObjSystem.contactType) contactObjSystem.cinemachineVirtualCamera.Priority = 1;
            //if (contactObjGetPanel.activeSelf) Canvas_Transition(contactObjGetPanel, false);
            contactPanel.SetActive(false);
            contactPanel.GetComponent<Image>().color = new UnityEngine.Color(0, 0, 0, 0);
            contactObjSystem = null;
            contactText.text = null;
            contactObjGetPanel = null;
        }
    }
    void TakeDmage(float damage)
    {
        if (currentHp > 0)
        {
            audioSource.PlayOneShot(damageSound);
            currentHp -= damage;
        }
        if (currentHp <= 0)
        {
            playerIsDeath = true;
            GameObject.Find("Enemy_Manager").GetComponent<Enemy_Manager>().Player_Death();
        }
    }
    private void PlayerMove(float speed)
    {
        float x = Input.GetAxisRaw("Horizontal"); // x方向のキー入力
        float z = Input.GetAxisRaw("Vertical"); // z方向のキー入力
        Vector3 Player_movedir = new Vector3(x, rigidBody.velocity.y, z); // 正規化
        Player_movedir = this.transform.forward * z + this.transform.right * x;
        Player_movedir = Player_movedir.normalized;
        rigidBody.velocity = new Vector3(Player_movedir.x * speed, rigidBody.velocity.y, Player_movedir.z * speed);
    }
    IEnumerator JunpMove()
    {
        rigidBody.velocity = Vector3.zero;
        isJumpingRunning = true;
        float ju_fo = jumpForce;
        for (int i = 0; i <= jumpNum && isJumpingRunning == true; i++)
        {
            //Debug.Log("jump処理");
            if (Input.GetKey(KeyCode.Space) && i == 0)
            {
                rigidBody.AddForce(Vector3.up * (jumpForce), ForceMode.Impulse);
                yield return new WaitForSeconds(jumpRepeatSecond);
            }
            if (Input.GetKey(KeyCode.Space) && i != 0)
            {
                ju_fo = ju_fo / 1.5f;
                rigidBody.AddForce(Vector3.up * (ju_fo), ForceMode.Impulse);
                yield return new WaitForSeconds(jumpRepeatSecond);
            }
        }
    }
    public void NomalShot()
    {
        audioSource.PlayOneShot(shotSound);
        var Guns = gunlist.Data[player_weapon_id];
        for (int i = 0 ; i < Guns.multi_bullet ; i++)
        {
            GameObject shotObj = Instantiate(SHOTOBJ, shotPosition.transform.position, Quaternion.identity);
            Rigidbody rb = shotObj.GetComponent<Rigidbody>();
            Bullet_System bs = shotObj.GetComponent<Bullet_System>();

            bs.bulletType = (Bullet_System.BulletType)Guns.type;
            bs.target_tag = "Enemy";
            bs.damage = Guns.bullet_damage;
            bs.deathDistance = Guns.bullet_range;
            bs.firstpos = shotPosition.transform.position;
            shotObj.transform.eulerAngles = playerCamera.transform.eulerAngles;
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
            movePermit = false;
            cinemachinBrain.enabled = false;
            battleInfomationPanel.SetActive(false);
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            movePermit = true;
            cinemachinBrain.enabled = true;
            battleInfomationPanel.SetActive(true);
        }
        isPanelOpen = false;
        MenuPanel.transform.localScale = Vector3.zero;
        contactPanel.SetActive(false);
        MenuPanel.SetActive(false);
        reloadSlider.gameObject.SetActive(false);
        contactText.text = null;
        playerIsDeath = false;
        currentHp = hp;
        currentEn = en;
        currentatk = atk;
        currentagi = agi;
        hpSlider.maxValue = hp;
        hpSlider.value = currentHp;
        enSlider.maxValue = en;
        enSlider.value = currentEn;
        transform.localRotation = new Quaternion(0,0,0,0);
        rigidBody.velocity = Vector3.zero;
        rigidBody.useGravity = true;
        Wepon_Reset(player_weapon_id);
        Debug.Log("プレイヤー情報がリセットしおわりました");
    }
    public void Wepon_Reset(int num)
    {
        isReloadPossible = false;
        reload_count = 0;
        var Guns = gunlist.Data[num];
        weaponImage.sprite = Guns.sprite_id;
        shotSound = Guns.shot_sound;
        loaded_bullets = Guns.loaded_bullets;
        current_loaded_bullets = Guns.loaded_bullets;
        reload_speed = Guns.reload_speed;
        reloadSlider.maxValue = reload_speed;
    }
    /*
    public void Canvas_Transition(GameObject Panel, bool IS)
    {
        if (IS)
        {
            audioSource.PlayOneShot(panelSound);
            //brain.enabled = false;
            movePermit = false;
            isPanelOpen = true;
            Panel.SetActive(true);
            DOTween.Sequence()
                .Append(Panel.GetComponent<RectTransform>().DOScale(Vector3.one, 0.25f)
                .SetEase(Ease.OutCirc)
                .OnComplete(() => {
                    movePermit = false;
                    isPanelOpen = true;
                    Panel.SetActive(true);
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.Confined;
                }))
                .Play();
        }
        else
        {
            audioSource.PlayOneShot(panelSound);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            //brain.enabled = true;
            movePermit = true;
            DOTween.Sequence()
                .Append(Panel.GetComponent<RectTransform>().DOScale(Vector3.zero, 0.25f)
                .SetEase(Ease.OutCirc)
              .OnComplete(() => {
                  Cursor.visible = false;
                  Cursor.lockState = CursorLockMode.Locked;
                  movePermit = true;
                  isPanelOpen = false;
                  Panel.SetActive(false);
              }))
                .Play();
        }
    }
    public void Canvas_Transition(ContactObj_System con,GameObject Panel,bool IS)
    {
        if (IS)
        {
            movePermit = false;
            isPanelOpen = true;
            audioSource.PlayOneShot(panelSound);
            switch (contactObjSystem.contactType)
            {
                case Contact_Type.Production_Table:
                    moneyText.SetActive(true);
                    moneyText.GetComponent<Text>().text = "MONEY:" + GameManager.playerMoney.ToString();
                    PlayerMoveStop();
                    break;
                case Contact_Type.StageSelect:
                    PlayerMoveStop();
                    break;
            }

            con.Canvas_Open();
            DOTween.Sequence()
                .Append(Panel.GetComponent<RectTransform>().DOScale(Vector3.one, 0.25f)
                .SetEase(Ease.OutCirc)
                .OnComplete(() => {
                    //brain.enabled = false;
                    movePermit = false;
                    isPanelOpen = true;
                    Panel.SetActive(true);
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.Confined;
                }))
                .Play();
        }
        else
        {
            audioSource.PlayOneShot(panelSound);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            movePermit = true;
            DOTween.Sequence()
                .Append(Panel.GetComponent<RectTransform>().DOScale(Vector3.zero, 0.25f)
                .SetEase(Ease.OutCirc)
              　.OnComplete(() => {
                  rigidBody.useGravity = true;
                  moneyText.SetActive(false);
                  contactObjSystem.cinemachineVirtualCamera.Priority = 1;
                  //brain.enabled = true;
                  movePermit = true;
                  isPanelOpen = false;
                  con.Canvas_Close();
                  Debug.Log("終了");
                }))
                .Play();
        }
    }
*/
    private void PlayerMoveStop()
    {
        movePermit = false;
        rigidBody.velocity = Vector3.zero;
        transform.position = contactObjSystem.idlepos.transform.position;
        transform.localEulerAngles = contactObjSystem.idlepos.transform.localEulerAngles;
    }
}
