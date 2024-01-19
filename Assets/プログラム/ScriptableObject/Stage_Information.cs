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
            //�^�C�g��
        �@�@title,
            //�Z�[�t�G���A�i���Ȃ��j
            Safe,
            //�E�F�[�u���Ƃɏo�Ă���G��S�ē|���΃N���A
            wave,
            //�i�X��Փx���オ���Ă���
            endless,
            //�{�X��
            boss,
        }
        public StageTipe stagetipe;
        public string name;
        public Vector3 spawn_pos;
        public int[] enemies_num;//�G�̐�
        public float time_limit;//��������
    }
}
