using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Gun_List;

public class Player_System : MonoBehaviour
{
    static public bool move_permit = true;//移動可能か否か

    [Header("--- GetComponent ---")]
    [SerializeField] public Gun_List gunlist;
    [SerializeField] public Rigidbody rb;
    [SerializeField] public GameObject CAMERA;
    [SerializeField] private GameObject SHOTPOS;

    [SerializeField] private GameObject PCanvas;

    [SerializeField] private ContactObj_System ConObj;
    [SerializeField] private GameObject ContactPanel;
    [SerializeField] private Text ContactText;

    [SerializeField] private GameObject MenuPanel;

    [Header("--- 基本動作 ---")]
    public float jumpforce = 6f;
    public float jump_num = 100;
    public float jump_second = 0.1f;

    private bool isJumping = false;//ジャンプ出来るか否か
    private bool isJumping_running = false;//ジャンプ処理中か否か

    [Tooltip("移動速度")]
    public float walk_speed = 10;

    [Header("--- 装備品 ---")]
    public int weapon_id;
    private float rate_count = 0;
    [SerializeField] public GameObject SHOTOBJ;

    private void Awake()
    {
        SHOTPOS = transform.Find("SHOTPOS").gameObject;
        PCanvas = transform.Find("PCanvas").gameObject;

        ContactPanel = transform.Find("PCanvas/ContactPanel").gameObject;
        ContactText = transform.Find("PCanvas/ContactPanel/ContactText").gameObject.GetComponent<Text>();

        MenuPanel = transform.Find("PCanvas/MenuPanel").gameObject;

        ContactPanel.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        MenuPanel.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        ContactText.text = null;
    }
    void Update()
    {
        this.transform.eulerAngles = new Vector3(0, CAMERA.transform.eulerAngles.y, 0);
        if (Input.GetKeyDown(KeyCode.P)) this.transform.position = Vector3.zero;
        if (move_permit //移動許可が出されており
        && Input.GetKey(KeyCode.Space) //buttonが押されていて
        && !isJumping_running//ジャンプ処理中ではない場合で
        && isJumping)//接地している場合
        {
            Debug.Log("ジャンプ");
            StartCoroutine("JunpMove");
            isJumping = false;
        }
        if (Input.GetKeyUp(KeyCode.Space)) isJumping_running = false;
        if (Input.GetKeyDown(KeyCode.Q) && ConObj != null) ConObj.Contact_function();
    }
    void FixedUpdate()
    {
        if (move_permit)
        {
            if (rate_count >= gunlist.Performance[weapon_id].rapid_fire_rate && Input.GetKey(KeyCode.E))
            {
                Debug.Log("発射しました");
                NomalShot();
                rate_count = 0;
            }
            else if(rate_count < gunlist.Performance[weapon_id].rapid_fire_rate) rate_count += 0.2f;

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
        if (other.gameObject.CompareTag("Contact"))
        {
            ContactPanel.GetComponent<Image>().color = new Color(255, 255, 255, 50);
            ConObj = other.GetComponent<ContactObj_System>();
            ContactText.text = ConObj.contact_text;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Floor")) isJumping = false;
        if (other.gameObject.CompareTag("Contact"))
        {
            ContactPanel.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            ConObj = null;
            ContactText.text = null;
        }
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
        Bullet_System bs = shotObj.GetComponent<Bullet_System>();
        var Guns = gunlist.Performance;

        bs.damage = Guns[1].bullet_damage;
        bs.death_time = Guns[1].bullet_range;
        rb.velocity = CAMERA.transform.forward * Guns[1].bullet_speed;
        shotObj.transform.eulerAngles = CAMERA.transform.eulerAngles;
        //shotObj.transform.eulerAngles = this.transform.eulerAngles + new Vector3(0, 0, -90);
    }
}
