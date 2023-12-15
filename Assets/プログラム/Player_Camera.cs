using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Camera : MonoBehaviour
{
    [SerializeField] GameObject player;

    void Update()
    {
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y") *-1;

        if (Mathf.Abs(mx) > 0.001f)
        {
            // 回転軸はワールド座標のY軸
            transform.RotateAround(player.transform.position, Vector3.up, mx);
        }

        if (Mathf.Abs(my) > 0.001f)
        {
            // 回転軸はカメラ自身のX軸
            transform.RotateAround(player.transform.position, transform.right, my);
        }
    }
}
