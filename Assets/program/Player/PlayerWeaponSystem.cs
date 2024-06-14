using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Gun_List;

public class PlayerWeaponSystem : MonoBehaviour
{
    [Header("--- 装備品 ---")]
    [SerializeField] private AudioClip shotSound;
    [SerializeField] public Gun_List gunlist;
    [SerializeField] public static int player_weapon_id = 0;
    private float rate_count = 0;
    private float loadedBullets = 0;//弾の最大値
    public float currentLoadedBullets = 0;//現在の残弾
    private float reloadSpeed = 0;//リロード完了値
    public float reload_count = 0;//リロードカウント
    private bool isReloadPossible;
    [SerializeField] public GameObject SHOTOBJ;
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
    public void NomalShot(GameObject Camera)
    {
        if (rate_count >= gunlist.Data[player_weapon_id].rapid_fire_rate && currentLoadedBullets > 0 && !isReloadPossible && Input.GetMouseButton(0))
        {
            //Debug.Log("発射しました");
            //audioSource.PlayOneShot(shotSound);
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
