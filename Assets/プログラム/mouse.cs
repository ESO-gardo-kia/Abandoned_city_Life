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
        // カーソル位置を取得
        Vector3 mousePosition = Input.mousePosition;
        // カーソル位置をワールド座標に変換
        Vector3 target = Camera.main.ScreenToWorldPoint(mousePosition);
        // GameObjectのtransform.positionにカーソル位置(ワールド座標)を代入
        this.transform.position = target;
        */
    }
}
