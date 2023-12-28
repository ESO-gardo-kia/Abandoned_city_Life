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
        public string name = "���O���ݒ肳��Ă��܂���";
        public Sprite sprite_id;
        public float price = 100;

        public float bullet_damage = 5;//�_���[�W
        public float bullet_range = 5;//�_���[�W
        public float bullet_speed = 100;//�e��
        public float rapid_fire_rate = 1;//�A�ˑ��x
        public float loaded_bullets = 50;//���e��
        public float reload_speed = 10;//�����[�h���x
    }
}
