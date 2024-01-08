using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(menuName = "ScriptableObject/EnemyList")]
public class Enemy_List : MonoBehaviour
{
    public List<EnemyList> Performance;
    [System.Serializable]
    public class EnemyList
    {
        public int id;
        public string name;
        public Sprite Enemy_Model;

        public float hp = 10;
        public float atk = 5;
        public float agi = 5;
        public float exp = 1;
    }
}
