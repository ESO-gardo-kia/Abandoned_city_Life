using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMainSystem : MonoBehaviour
{
    public bool bo = true;
    static public bool movePermit = true;//移動可能か否か
    static public bool playerIsDeath = true;//
    [Header("--- Component ---")]
    [SerializeField] private PlayerUiSystem playerUiSystem;
    [SerializeField] private PlayerMoveSystem playerMoveSystem;
    [SerializeField] private PlayerWeaponSystem playerWeaponSystem;
    [SerializeField] private CinemachineBrain cinemachinBrain;
    [SerializeField] public CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] public GameObject playerCamera;

    [Header("--- サウンド ---")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip panelSound;
    [SerializeField] private AudioClip damageSound;
    [SerializeField]
    [Header("--- ステータス ---")]
    public MainBody_List mainBodyList;
    public static int playerBodyId; 
    public float hp;
    public float currentHp;
    public float en;
    public float currentEn;

    public bool isEnergyRecovery;
    public float energyRecoveryTime;
    private float energyRecoveryCount;

    public float jumpForce = 100;
    public float walkSpeed = 10;
    public float dashSpeed = 15;

    private void Start()
    {
        Player_Reset(bo);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) this.transform.position = Vector3.zero;
        if (movePermit)
        {
            if (!PlayerUiSystem.isPanelOpen) transform.eulerAngles = new Vector3(0, playerCamera.transform.eulerAngles.y, 0);
            playerUiSystem.SsignmentStatsUi(playerWeaponSystem.reload_count,currentHp,currentEn, playerWeaponSystem.currentLoadedBullets);
            playerMoveSystem.IsJumpJudg(jumpForce);

            playerMoveSystem.WheelAnimation();

            playerWeaponSystem.BulletShotSystem(playerCamera, audioSource);
            playerWeaponSystem.GunReloadSystem(ref playerUiSystem);

            //移動処理
            if ((Input.GetKey(KeyCode.LeftShift) && currentEn > 0) &&
                (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)))
            {
                if (0 < currentEn)
                {
                    isEnergyRecovery = false;
                    currentEn--;
                }
                playerMoveSystem.PlayerMovement(dashSpeed);
            }
            else
            {
                EnergyRecovery();
                playerMoveSystem.PlayerMovement(walkSpeed);
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
    public void StatsInitialization()
    {
        var data = mainBodyList.data;
        hp = data[playerBodyId].hp;
        currentHp = data[playerBodyId].hp;
        en = data[playerBodyId].en;
        currentEn = data[playerBodyId].en;
        energyRecoveryTime = data[playerBodyId].energyRecoveryTime;
        energyRecoveryCount = 0;
        jumpForce = data[playerBodyId].jumpForce;
        walkSpeed = data[playerBodyId].walkSpeed;
        dashSpeed = data[playerBodyId].dashSpeed;
    }
    public void Player_Reset(bool IS)
    {
        playerIsDeath = false;

        currentHp = hp;
        currentEn = en;

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
        playerWeaponSystem.WeponChange();
        Debug.Log("プレイヤー情報リセット終了");
    }
}
