using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMoveSystem : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private GameObject wheel;
    [Header("--- ��{���� ---")]
    [SerializeField] private float repeatedDeclineJumpForce = 1.5f;
    [SerializeField] private float jumpRepeatNumber = 100;
    [SerializeField] private float jumpRepeatSecond = 0.02f;
    private float currentJumpForce;

    private bool isJumping = false;//�W�����v�o���邩�ۂ�
    private bool isJumpingRunning = false;//�W�����v���������ۂ�

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Floor")) isJumpingRunning = false;
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Floor")) isJumping = false;
    }
    public void IsJumpJudg(float jumpForce)
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumpingRunning = false;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            Ray downray = new Ray(gameObject.transform.position, Vector3.down);
            RaycastHit hit;
            if (Physics.Raycast(downray, out hit, 10.0f))
            {
                var n = hit.point.y - gameObject.transform.position.y + 0.5f;
                if (Mathf.Round(n) == 0 && n < 0.1f && 0.1f > n && !isJumpingRunning) isJumping = true;
                else isJumping = false;
            }
            else isJumping = false;

            if (!isJumpingRunning && isJumping)
            {
                StartCoroutine(JunpMove(jumpForce));
                isJumping = false;
            }
        }

    }
    public void PlayerMovement(float speed)
    {
        float x = Input.GetAxisRaw("Horizontal"); // x�����̃L�[����
        float z = Input.GetAxisRaw("Vertical"); // z�����̃L�[����
        Vector3 Player_movedir = new Vector3(x, rigidBody.velocity.y, z); // ���K��
        Player_movedir = this.transform.forward * z + this.transform.right * x;
        Player_movedir = Player_movedir.normalized;
        rigidBody.velocity = new Vector3(Player_movedir.x * speed, rigidBody.velocity.y, Player_movedir.z * speed);
    }
    IEnumerator JunpMove(float jumpForce)
    {
        rigidBody.velocity = Vector3.zero;
        isJumpingRunning = true;
        currentJumpForce = jumpForce;
        for (int i = 0; i <= jumpRepeatNumber && isJumpingRunning == true; i++)
        {
            //Debug.Log("jump����");
            if (Input.GetKey(KeyCode.Space) && i == 0)
            {
                rigidBody.AddForce(Vector3.up * (jumpForce), ForceMode.Impulse);
                yield return new WaitForSeconds(jumpRepeatSecond);
            }
            if (Input.GetKey(KeyCode.Space) && i != 0)
            {
                currentJumpForce = currentJumpForce / repeatedDeclineJumpForce;
                rigidBody.AddForce(Vector3.up * (currentJumpForce), ForceMode.Impulse);
                yield return new WaitForSeconds(jumpRepeatSecond);
            }
        }
    }
    public void WheelAnimation()
    {
        wheel.transform.Rotate(Vector3.up, rigidBody.velocity.magnitude * 2);
    }
}
