using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(menuName = "ScriptableObject/GunList")]
public class Gun_List : ScriptableObject
{
    public List<GunList> Performance;
    [System.Serializable]
    public class GunList
    {
        public int id;
        public string name = "名前が設定されていません";
        public Sprite sprite_id;
        public float price = 100;

        public float bullet_damage = 5;//ダメージ
        public float bullet_range = 5;//ダメージ
        public float bullet_speed = 100;//弾速
        public float rapid_fire_rate = 1;//連射速度
        public float loaded_bullets = 50;//装弾数
        public float reload_speed = 10;//リロード速度
    }
}
