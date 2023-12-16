using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouse : MonoBehaviour
{
    public float x;//昔
    public float y;
    public Vector3 lastMousePosition;
    public GameObject Camera;
    void FixedUpdate()
    {
        float cx = Input.mousePosition.x;//今
        float cy = Input.mousePosition.y;
        if (cx != x || cy != y) Camera.transform.eulerAngles += new Vector3(cy - y * 0.1f, cx - x, 0);

        x = Input.mousePosition.x;
        y = Input.mousePosition.y;



        /*
         *         if(cx != x) vec3 = new Vector3(cx - x, 0, 0);
        if(cy != y) vec3 += new Vector3(0, cy - y, 0);
        //Vector3 vec3 = new Vector3(x - Screen.width / 2, y - Screen.height / 2, 0);
        if (cx != 0) vcx = cx - x;
        else vcx = 0;
        if (cy != 0) vcy = cy - y;
        else vcy = 0;
        // カーソル位置を取得
        Vector3 mousePosition = Input.mousePosition;
        // カーソル位置をワールド座標に変換
        Vector3 target = Camera.main.ScreenToWorldPoint(mousePosition);
        // GameObjectのtransform.positionにカーソル位置(ワールド座標)を代入
        this.transform.position = target;
                if (Mathf.Abs(Screen.width / 2 - Input.mousePosition.x) > 50 || Mathf.Abs(Screen.height / 2 - Input.mousePosition.y) > 50)
        {
            Cursor.lockState = CursorLockMode.Locked;
            lastMousePosition = Input.mousePosition;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            lastMousePosition = Input.mousePosition;
        }
        */

    }
}
