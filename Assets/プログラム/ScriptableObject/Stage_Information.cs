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
            //�E�F�[�u���Ƃɏo�Ă���G��S�ē|���΃N���A
            wave,
            //�i�X��Փx���オ���Ă���
            endless,
            //�{�X��
            boss,
        }
        public int id;
        public string name;
        public int[] enemies_num;//�G�̐�
        public float time_limit;//��������
    }
}
