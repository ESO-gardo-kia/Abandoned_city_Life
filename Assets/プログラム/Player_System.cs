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
    [SerializeField] public GameObject SHOTPOS;
    [SerializeField] public GameObject SHOTOBJ;

    [Header("--- 基本動作 ---")]
    public float jumpforce = 6f;
    public float jump_num = 100;
    public float jump_second = 0.1f;

    private bool isJumping = false;//ジャンプ出来るか否か
    private bool isJumping_running = false;//ジャンプ処理中か否か

    [Tooltip("移動速度")]
    public float walk_speed = 10;

    //仮
    public float bullet_speed = 5;
    public float num = 0;
    public float max_num = 10;

    void Update()
    {
        this.transform.eulerAngles = new Vector3(0, CAMERA.transform.eulerAngles.y, 0);
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
            if (num >= max_num && Input.GetKey(KeyCode.E))
            {
                NomalShot();
                num = 0;
            }
            else if(num < max_num) num += 0.2f;

            float x = Input.GetAxisRaw("Horizontal"); // x方向のキー入力
            float z = Input.GetAxisRaw("Vertical"); // z方向のキー入力
            Vector3 Player_movedir = new Vector3(x, rb.velocity.y, z).normalized; // 正規化
            Player_movedir = this.transform.forward * z + this.transform.right * x;
            rb.velocity = new Vector3(Player_movedir.x * walk_speed, rb.velocity.y, Player_movedir.z * walk_speed);
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
    public void NomalShot()
    {
        GameObject shotObj = Instantiate(SHOTOBJ,SHOTPOS.transform.position,Quaternion.identity);
        Rigidbody rb = shotObj.GetComponent<Rigidbody>();
        rb.velocity = CAMERA.transform.forward * bullet_speed;
        shotObj.transform.eulerAngles = CAMERA.transform.eulerAngles;
        //shotObj.transform.eulerAngles = this.transform.eulerAngles + new Vector3(0, 0, -90);
    }
}
