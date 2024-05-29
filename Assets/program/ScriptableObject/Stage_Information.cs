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
        public AudioClip BGM;
        public enum StageType
        {
            NomalStage,
            //段々難易度が上がっていく
            endless,
            //ボス戦
            boss,

            Safe
        }
        public enum TransitionScene
        {
            Title,
            Select,
            Main
        }
        public StageType stagetype;
        public TransitionScene tran_scene;
        public string name;
        public int stagenumber;
        public Vector3 spawn_pos;
        //3ウェーブ固定
        public int[] enemies_num1;
        public int[] enemies_num2;
        public int[] enemies_num3;
        public float time_limit;//制限時間
    }
}
