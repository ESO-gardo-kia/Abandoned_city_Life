using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouse : MonoBehaviour
{
    void FixedUpdate()
    {

        float x = Input.mousePosition.x;
        float y = Input.mousePosition.y;
        Vector3 vec3 = new Vector3(x - Screen.width / 2, y - Screen.height / 2, 0);
        Debug.Log(vec3);
        /*
        // �J�[�\���ʒu���擾
        Vector3 mousePosition = Input.mousePosition;
        // �J�[�\���ʒu�����[���h���W�ɕϊ�
        Vector3 target = Camera.main.ScreenToWorldPoint(mousePosition);
        // GameObject��transform.position�ɃJ�[�\���ʒu(���[���h���W)����
        this.transform.position = target;
        */
    }
}
