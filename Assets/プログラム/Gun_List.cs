using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObject/GunList")]
public class Gun_List : ScriptableObject
{
    public List<GunList> Items;
    [System.Serializable]
    public class GunList
    {
        public int id;
        public string name;
        public int sprite_id;
        public float price;

        public float bullet_damage;//ダメージ
        public float bullet_speed;//弾速
        public float rapid_fire_rate;//連射速度
        public float loaded_bullets;//装弾数
        public float reload_speed;//リロード速度
    }
}
