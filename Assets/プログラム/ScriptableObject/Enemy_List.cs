using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(menuName = "ScriptableObject/EnemyList")]
public class Enemy_List : ScriptableObject
{
    public List<EnemyList> Status;
    [System.Serializable]
    public class EnemyList
    {
        public string name;
        public GameObject Enemy_Model;

        public float hp = 10;
        public float atk = 5;
        public float agi = 5;
        public float exp = 1;
        public float price;
    }
}
