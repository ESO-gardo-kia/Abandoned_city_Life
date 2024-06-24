using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(menuName = "ScriptableObject/GunList")]
public class Gun_List : ScriptableObject
{
    public List<GunList> Data;
    [System.Serializable]
    public class GunList
    {
        public AudioClip shotSound;
        public AudioClip reloadSound;
        public bool ispossession;
        public int id;
        public string name = null;
        public Sprite sprite_id;
        public float price;

        public float bullet_damage;//ダメージ
        public float rapid_fire_rate;//連射速度
        public float loaded_bullets;//装弾数
        public float reload_speed;//リロード速度
        public float bullet_range;//射程
        public float bullet_speed;//弾速
        public float diffusion__chance;//拡散率
        public float multi_bullet;//
        public GameObject bullet_Object;
        public float rarity;
    }
}
