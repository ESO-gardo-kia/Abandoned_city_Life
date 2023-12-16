using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_System : MonoBehaviour
{
    static public bool move_permit = true;//移動可能か否か

    [Header("--- GetComponent ---")]
    [SerializeField] public Rigidbody rb;
    [SerializeField] public GameObject CAMERA;
    [SerializeField] public GameObject POINTER;

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
    public float airwalk_speed = 30;

    public Vector3 aOrigin;
    public Vector3 bOrigin;

    void Update()
    {
        /*
        Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        POINTER.transform.position = target;
        aOrigin = POINTER.transform.position;
        //transform.eulerAngles = new Vector3(transform.eulerAngles.x,CAMERA.transform.eulerAngles.y, transform.eulerAngles.z) ;
        
        if (aOrigin != POINTER.transform.position)
        {
            float x = POINTER.transform.position.x - aOrigin.x;
            float y = POINTER.transform.position.y - aOrigin.y;
            //Debug.Log(x+"　"+y);
            CAMERA.transform.eulerAngles = new Vector3(x, y, transform.eulerAngles.z);
        }
        */
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
        //Debug.Log(POINTER.transform.position);
    }
    void FixedUpdate()
    {
        if (move_permit)
        {
            float x = Input.GetAxisRaw("Horizontal"); // x方向のキー入力
            float z = Input.GetAxisRaw("Vertical"); // z方向のキー入力
            Vector3 Player_movedir = new Vector3(x, rb.velocity.y, z).normalized; // 正規化
            Player_movedir = CAMERA.transform.forward * z + CAMERA.transform.right * x;
            rb.velocity = new Vector3(Player_movedir.x * walk_speed, rb.velocity.y, Player_movedir.z * walk_speed);
            //rb.velocity = Player_movedir * walk_speed;
            /*
            rb.AddForce(x * walk_speed, 0, z * walk_speed, ForceMode.Impulse);
            rb.velocity = new Vector3(Input.GetAxis("Horizontal"), rb.velocity.y, Input.GetAxis("Vertical"));
            */
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Floor")) isJumping = true;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Floor")) isJumping_running = false;
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
