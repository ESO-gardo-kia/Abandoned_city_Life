using UnityEngine;
using static Gun_List;

public class PlayerWeaponSystem : MonoBehaviour
{
    [SerializeField] private AudioClip shotSound;
    [SerializeField] private Gun_List gunlist;
    [SerializeField] public static int player_weapon_id = 0;
    private float rate_count = 0;
    private float loadedBullets = 0;//弾の最大値
    [System.NonSerialized] public float currentLoadedBullets = 0;//現在の残弾
    private float reloadSpeed = 0;//リロード完了値
    [System.NonSerialized] public float reload_count = 0;//リロードカウント
    private bool isReloadPossible;
    [SerializeField] private GameObject shotPosition;
    public void GunReloadSystem(ref PlayerUiSystem playerUiSystem)
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
    public void BulletShotSystem(GameObject Camera, AudioSource audioSource)
    {
        if (rate_count >= gunlist.Data[player_weapon_id].rapid_fire_rate && currentLoadedBullets > 0 && !isReloadPossible && Input.GetMouseButton(0))
        {
            //Debug.Log("発射しました");
            audioSource.PlayOneShot(shotSound);
            var Guns = gunlist.Data[player_weapon_id];
            for (int i = 0; i < Guns.multi_bullet; i++)
            {
                GameObject shotObj = Instantiate(Guns.bullet_Object, shotPosition.transform.position, Quaternion.identity);
                Rigidbody rb = shotObj.GetComponent<Rigidbody>();
                NormalBulletSystem bs = shotObj.GetComponent<NormalBulletSystem>();

                bs.targetTag = "Enemy";
                bs.bulletDamage = Guns.bullet_damage;
                bs.bulletSpeed = Guns.bullet_speed;
                bs.deathDistance = Guns.bullet_range;
                bs.firstPosition = shotPosition.transform.position;
                shotObj.transform.eulerAngles = Camera.transform.eulerAngles;
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
    public void WeponChange()
    {
        isReloadPossible = false;
        reload_count = 0;
        var Guns = gunlist.Data[player_weapon_id];
        GetComponent<PlayerUiSystem>().weaponImagePanel.sprite = Guns.sprite_id;
        shotSound = Guns.shot_sound;
        loadedBullets = Guns.loaded_bullets;
        currentLoadedBullets = Guns.loaded_bullets;
        reloadSpeed = Guns.reload_speed;
        GetComponent<PlayerUiSystem>().reloadSlider.maxValue = reloadSpeed;
    }
}
