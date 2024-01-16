using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObject/Stage_Information")]
public class Stage_Information: ScriptableObject
{
    public List<stage_information> data;
    [System.Serializable]
    public class stage_information
    {
        public int id;
        public string name;
        public int enemies_num;//“G‚Ì”
        public float time_limit;//§ŒÀŠÔ
    }
}
