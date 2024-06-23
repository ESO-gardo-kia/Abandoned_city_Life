using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
        public StageType stagetype;
        public int transitionSceneNumber;
        public string name;
        public int fixationReward;
        public string stageDescription;
        public int stagenumber;
        public Sprite stageSprite;
        public Vector3 spawn_pos;
        //3ウェーブ固定
        public int[] enemies_num1;
        public int[] enemies_num2;
        public int[] enemies_num3;
        public float time_limit;//制限時間
    }
}
