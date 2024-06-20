using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObject/SceneList")]

public class Scene_List : ScriptableObject
{
    public List<SceneList> data;
    [System.Serializable]
    public class SceneList
    {
        public string sceneName;
    }
}
