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
            //�^�C�g��
        �@�@title,
            //�Z�[�t�G���A�i���Ȃ��j
            Safe,
            NomalStage,
            //�i�X��Փx���オ���Ă���
            endless,
            //�{�X��
            boss,
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
        public Vector3 spawn_pos;
        //3�E�F�[�u�Œ�
        public int[] enemies_num1;
        public int[] enemies_num2;
        public int[] enemies_num3;
        public Wave a;
        public float time_limit;//��������
    }
    public class Wave
    {
    }
}
