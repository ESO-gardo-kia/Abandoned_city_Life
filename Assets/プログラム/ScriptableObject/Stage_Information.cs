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
        public enum StageType
        {
            //タイトル
        　　title,
            //セーフエリア（戦わない）
            Safe,
            //段々難易度が上がっていく
            endless,
            //ボス戦
            boss,
        }
        public StageType stagetype;
        public string name;
        public Vector3 spawn_pos;
        public int[] enemies_num;//敵の数
        public float time_limit;//制限時間
    }
}
