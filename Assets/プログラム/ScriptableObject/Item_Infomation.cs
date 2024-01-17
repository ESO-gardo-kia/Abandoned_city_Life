using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(menuName = "ScriptableObject/Item_Infomation")]
public class Item_Infomation : ScriptableObject
{
    public List<item_infomation> Info;
    [System.Serializable]
    public class item_infomation
    {
        public string Name;
        public Sprite Image;
    }
}
