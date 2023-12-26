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

        public float bullet_damage;//�_���[�W
        public float bullet_speed;//�e��
        public float rapid_fire_rate;//�A�ˑ��x
        public float loaded_bullets;//���e��
        public float reload_speed;//�����[�h���x
    }
}
