using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveSystem : MonoBehaviour
{
    /*
    // Start is called before the first frame update
    private void IsJumpJudg()
    {
        if (Input.GetKeyUp(KeyCode.Space)) isJumpingRunning = false;
        if (Input.GetKey(KeyCode.Space) && !isJumpingRunning && isJumping)
        {
            StartCoroutine("JunpMove");
            isJumping = false;
        }
        Ray downray = new Ray(gameObject.transform.position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(downray, out hit, 10.0f))
        {
            var n = hit.point.y - gameObject.transform.position.y + 0.5f;
            if (Mathf.Round(n) == 0 && n < 0.1f && 0.1f > n && !isJumpingRunning) isJumping = true;
            else isJumping = false;
        }
        else isJumping = false;
    }
    private void PlayerMove(float speed, ref Rigidbody rigidBody)
    {
        float x = Input.GetAxisRaw("Horizontal"); // x方向のキー入力
        float z = Input.GetAxisRaw("Vertical"); // z方向のキー入力
        Vector3 Player_movedir = new Vector3(x, rigidBody.velocity.y, z); // 正規化
        Player_movedir = this.transform.forward * z + this.transform.right * x;
        Player_movedir = Player_movedir.normalized;
        rigidBody.velocity = new Vector3(Player_movedir.x * speed, rigidBody.velocity.y, Player_movedir.z * speed);
    }
    IEnumerator JunpMove(ref Rigidbody rigidBody,)
    {
        rigidBody.velocity = Vector3.zero;
        isJumpingRunning = true;
        float ju_fo = jumpForce;
        for (int i = 0; i <= jumpNum && isJumpingRunning == true; i++)
        {
            //Debug.Log("jump処理");
            if (Input.GetKey(KeyCode.Space) && i == 0)
            {
                rigidBody.AddForce(Vector3.up * (jumpForce), ForceMode.Impulse);
                yield return new WaitForSeconds(jumpRepeatSecond);
            }
            if (Input.GetKey(KeyCode.Space) && i != 0)
            {
                ju_fo = ju_fo / 1.5f;
                rigidBody.AddForce(Vector3.up * (ju_fo), ForceMode.Impulse);
                yield return new WaitForSeconds(jumpRepeatSecond);
            }
        }
    }
    */
}
