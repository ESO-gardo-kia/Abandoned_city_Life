using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(menuName = "ScriptableObject/MainBoyList")]
public class MainBody_List : ScriptableObject
{
    public List<EnemyList> data;
    [System.Serializable]
    public class EnemyList
    {
        public GameObject bodyModel;
        public string bodyName;

        public float hp = 100;
        public float en = 100;
        public float energyRecoveryTime;

        public float jumpForce = 300;
        public float walkSpeed = 10;
        public float dashSpeed = 15;
    }
}
