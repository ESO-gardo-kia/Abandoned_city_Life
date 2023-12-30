using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(menuName = "ScriptableObject/ItemSpriteList")]
public class ItemSprite_List : ScriptableObject
{
    public List<ItemSpriteList> Performance;
    [System.Serializable]
    public class ItemSpriteList
    {
        public Sprite[] Sprites;
    }
}
