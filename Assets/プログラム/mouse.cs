using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouse : MonoBehaviour
{
    void Update()
    {
        // �J�[�\���ʒu���擾
        Vector3 mousePosition = Input.mousePosition;
        // �J�[�\���ʒu�����[���h���W�ɕϊ�
        Vector3 target = Camera.main.ScreenToWorldPoint(mousePosition);
        // GameObject��transform.position�ɃJ�[�\���ʒu(���[���h���W)����
        this.transform.position = target;
    }
}
