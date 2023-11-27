using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_System : MonoBehaviour
{
    static public bool move_permit = true;//移動可能か否か

    [Header("--- GetComponent ---")]
    [SerializeField] public Rigidbody rb;
    [SerializeField] public GameObject camera;

    [Header("--- 基本動作 ---")]
    public float jumpforce = 6f;
    public float jump_num = 100;
    public float jump_second = 0.1f;

    public bool isJumping = false;//ジャンプ出来るか否か
    public bool isJumping_running = false;//ジャンプ処理中か否か

    //レイキャスト関係
    private Vector3 my_origin;
    private Ray right_ray;//右
    private Ray left_ray;//左

    [Tooltip("移動速度")]
    public float walk_speed = 10;
    [Tooltip("空中速度")]
    public float air_speed = 30;

    void Update()
    {
        transform.eulerAngles = camera.transform.eulerAngles;

        if (Input.GetKeyDown(KeyCode.Q)) this.transform.position = Vector3.zero;
        if (move_permit //移動許可が出されており
        && Input.GetKey(KeyCode.Space) //buttonが押されていて
        && !isJumping_running//ジャンプ処理中ではない場合で
        && isJumping)//接地している場合
        {//JunpMoveが起動できる
            Debug.Log("ジャンプ");
            StartCoroutine("JunpMove");
            isJumping = false;
        }
        if (Input.GetKeyUp(KeyCode.Space)) isJumping_running = false;

    }
    void FixedUpdate()
    {
        if (move_permit)
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            rb.AddForce(x * walk_speed, 0, z * walk_speed, ForceMode.Impulse);
            rb.velocity = new Vector3(Input.GetAxis("Horizontal"), rb.velocity.y, Input.GetAxis("Vertical"));
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Floor")) isJumping = true;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            isJumping_running = false;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Floor")) isJumping = false;
    }
    IEnumerator JunpMove()
    {
        rb.velocity = Vector3.zero;
        isJumping_running = true;
        float ju_fo = jumpforce;
        for (int i = 0; i <= jump_num && isJumping_running == true; i++)
        {
            Debug.Log("jump処理");
            if (Input.GetKey(KeyCode.Space) && i == 0)
            {
                rb.AddForce(Vector3.up * (jumpforce), ForceMode.Impulse);
                yield return new WaitForSeconds(jump_second);
            }
            if (Input.GetKey(KeyCode.Space) && i != 0)
            {
                ju_fo = ju_fo / 1.5f;
                rb.AddForce(Vector3.up * (ju_fo), ForceMode.Impulse);
                yield return new WaitForSeconds(jump_second);
            }
        }
    }
}
