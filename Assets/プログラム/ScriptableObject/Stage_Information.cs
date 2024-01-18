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
        public enum StageTipe
        {
            //ウェーブごとに出てくる敵を全て倒せばクリア
            wave,
            //段々難易度が上がっていく
            endless,
            //ボス戦
            boss,
        }
        public int id;
        public string name;
        public int[] enemies_num;//敵の数
        public float time_limit;//制限時間
    }
}
