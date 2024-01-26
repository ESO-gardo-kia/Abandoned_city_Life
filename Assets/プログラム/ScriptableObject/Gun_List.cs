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
        public bool ispossession;
        public int id;
        public string name = null;
        public Sprite sprite_id;
        public float price;

        public float bullet_damage;//�_���[�W
        public float rapid_fire_rate;//�A�ˑ��x
        public float loaded_bullets;//���e��
        public float reload_speed;//�����[�h���x
        public float bullet_range;//�˒�
        public float bullet_speed;//�e��
    }
}
