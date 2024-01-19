using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class throwing : MonoBehaviour
{
    [SerializeField]
    private GameObject point1;
    [SerializeField]
    private GameObject point2;

    public bool isthrowing = false;

    private Vector3 pos;
    private Vector3 pos_t;
    private Vector3 pos_l;

    private float time = 0;
    public int endTime = 40;

    private Vector3 speed = Vector3.zero;
    public float upSpeed;
    private float gravity = 0.03f;

    private void Start()
    {
        pos = point1.transform.position;
        pos_t = point1.transform.position;
        pos_l = point2.transform.position;

        transform.position = pos;

    }
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.A)) isthrowing = true;
        if (isthrowing)
        {
            var vn = ((pos_l - pos_t).y - gravity * 0.5f * endTime * endTime) / endTime; // ���������̏����xvn
            if (time < endTime)
            {
                var p = Vector3.Lerp(pos_t, pos_l, time / endTime);   //���������̍��W�����߂� (x,z���W)
                p.y = pos_t.y + vn * time + 0.5f * gravity * time * time; // ���������̍��W y
                this.transform.position = new Vector3(p.x,-p.y,p.z);
                time += 1;
            }
            else
            {
                isthrowing = false;
                time = 0;
                pos = pos_t;
                transform.position = pos;
            }
        }
        else
        {
            transform.position = pos_l;
            this.transform.position = pos;
        }
    }
}
