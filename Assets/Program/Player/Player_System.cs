using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Player_System : MonoBehaviour
{
    public bool bo = true;
    static public bool movePermit = true;//移動可能か否か
    static public bool playerIsDeath = true;//移動可能か否か
    [Header("--- GetComponent ---")]
    [SerializeField] public Gun_List gunlist;
    [SerializeField] private PlayerUiSystem playerUiSystem;
    [SerializeField] private PlayerMoveSystem playerMoveSystem;
    [SerializeField] private CinemachineBrain cinemachinBrain;
    [SerializeField] public GameObject playerCamera;
    [SerializeField] public CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] private GameObject shotPosition;

    [Header("--- サウンド ---")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip panelSound;
    [SerializeField] private AudioClip shotSound;
    [SerializeField] private AudioClip damageSound;
    [SerializeField]
    [Header("--- ステータス ---")]
    public bool isEnergyRecovery;
    public float energyRecoveryTime;
    private float energyRecoveryCount;

    public float hp;
    public float currentHp;
    public float en;
    public float currentEn;
    public float atk;
    public float currentatk;
    public float agi;
    public float currentagi;

    [Header("--- 装備品 ---")]
    [SerializeField] public static int player_weapon_id;
    private float rate_count = 0;
    private float loadedBullets = 0;//弾の最大値
    private float currentLoadedBullets = 0;//現在の残弾
    private float reloadSpeed = 0;//リロード完了値
    private float reload_count = 0;//リロードカウント
    private bool isReloadPossible;
    [SerializeField] public GameObject SHOTOBJ;

    private void Start()
    {
        player_weapon_id = 0;
        Player_Reset(bo);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) this.transform.position = Vector3.zero;
        if (movePermit)
        {
            if (!PlayerUiSystem.isPanelOpen) transform.eulerAngles = new Vector3(0, playerCamera.transform.eulerAngles.y, 0);
            playerUiSystem.SsignmentStatsUi(reload_count,currentHp,currentEn,currentLoadedBullets);
            playerMoveSystem.IsJumpJudg();
        }
    }
    void FixedUpdate()
    {
        if (movePermit)
        {
            NomalShot();
            GunReloadSystem();
            //移動処理
            if ((Input.GetKey(KeyCode.LeftShift) && currentEn > 0) &&
                (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)))
            {
                if (0 < currentEn)
                {
                    isEnergyRecovery = false;
                    currentEn--;
                }
                playerMoveSystem.PlayerRunMove();
            }
            else
            {
                EnergyRecovery();
                playerMoveSystem.PlayerWalkMove();
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (movePermit)
        {
            if (other.gameObject.CompareTag("Bullet") && !playerIsDeath)
            {
                IsBulletTypeJuge(other);
            }
        }
    }
    private void IsBulletTypeJuge(Collider other)
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
    private void GunReloadSystem()
    {
        if ((currentLoadedBullets < loadedBullets && Input.GetKey(KeyCode.R))
            || (currentLoadedBullets < loadedBullets && currentLoadedBullets == 0 && Input.GetMouseButton(0)))
        {
            isReloadPossible = true;
            playerUiSystem.reloadSlider.gameObject.SetActive(true);
        }
        if (isReloadPossible)
        {
            reload_count += Time.deltaTime;
            if (reload_count >= reloadSpeed)
            {
                playerUiSystem.reloadSlider.gameObject.SetActive(false);
                currentLoadedBullets = loadedBullets;
                reload_count = 0;
                isReloadPossible = false;
            }
        }
    }
    private void EnergyRecovery()
    {
        if (en > currentEn && isEnergyRecovery)
        {
            currentEn++;
        }
        if (!isEnergyRecovery)
        {
            energyRecoveryCount += Time.deltaTime;
            if (energyRecoveryCount >= energyRecoveryTime)
            {
                energyRecoveryCount = 0;
                isEnergyRecovery = true;
            }
        }
    }
    public void TakeDmage(float damage)
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
    public void NomalShot()
    {
        if (rate_count >= gunlist.Data[player_weapon_id].rapid_fire_rate && currentLoadedBullets > 0 && !isReloadPossible && Input.GetMouseButton(0))
        {
            //Debug.Log("発射しました");
            audioSource.PlayOneShot(shotSound);
            var Guns = gunlist.Data[player_weapon_id];
            for (int i = 0; i < Guns.multi_bullet; i++)
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
            currentLoadedBullets--;
            rate_count = 0;
        }
        else if (rate_count < gunlist.Data[player_weapon_id].rapid_fire_rate)
        {
            rate_count += 0.2f;
        }
    }
    public void Player_Reset(bool IS)
    {
        playerIsDeath = false;

        currentHp = hp;
        currentEn = en;
        currentatk = atk;
        currentagi = agi;

        transform.localRotation = new Quaternion(0,0,0,0);
        //rigidBody.velocity = Vector3.zero;
        //rigidBody.useGravity = true;

        if (!IS)
        {
            cinemachinBrain.enabled = false;
            playerUiSystem.PlayerUiReset(IS,hp, currentHp, en, currentEn);
        }
        else
        {
            cinemachinBrain.enabled = true;
            playerUiSystem.PlayerUiReset(IS,hp, currentHp, en, currentEn);
        }

        Wepon_Reset(player_weapon_id);
        Debug.Log("プレイヤー情報リセット終了");
    }
    public void Wepon_Reset(int num)
    {
        isReloadPossible = false;
        reload_count = 0;
        var Guns = gunlist.Data[num];
        playerUiSystem.weaponImagePanel.sprite = Guns.sprite_id;
        shotSound = Guns.shot_sound;
        loadedBullets = Guns.loaded_bullets;
        currentLoadedBullets = Guns.loaded_bullets;
        reloadSpeed = Guns.reload_speed;
        playerUiSystem.reloadSlider.maxValue = reloadSpeed;
    }
}
