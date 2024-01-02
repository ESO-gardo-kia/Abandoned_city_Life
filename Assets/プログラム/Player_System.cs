using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player_System : MonoBehaviour
{
    static public bool move_permit = true;//移動可能か否か
    
    [Header("--- GetComponent ---")]
    [SerializeField] public Gun_List gunlist;
    [SerializeField] public Rigidbody rb;
    [SerializeField] public GameObject CAMERA;
    private GameObject SHOTPOS;

    private GameObject PCanvas;

    private ContactObj_System ConObj;
    private CollectObj_System ColObj;
    private GameObject ContactPanel;
    private GameObject MenuPanel;
    private GameObject GamePanel;
    private Text ContactText;
    private Slider CollectGage;

    [Header("--- 基本動作 ---")]
    public float jumpforce = 6f;
    public float jump_num = 100;
    public float jump_second = 0.1f;

    public bool isJumping = false;//ジャンプ出来るか否か
    private bool isJumping_running = false;//ジャンプ処理中か否か

    [Tooltip("移動速度")]
    public float walk_speed = 10;

    [Header("--- 装備品 ---")]
    public int weapon_id;
    private float rate_count = 0;
    private float collect_count = 0;
    [SerializeField] public GameObject SHOTOBJ;

    private void Awake()
    {
        SHOTPOS = transform.Find("SHOTPOS").gameObject;
        PCanvas = transform.Find("PCanvas").gameObject;

        ContactPanel = transform.Find("PCanvas/ContactPanel").gameObject;
        ContactText = transform.Find("PCanvas/ContactPanel/ContactText").gameObject.GetComponent<Text>();
        CollectGage = transform.Find("PCanvas/ContactPanel/CollectGage").gameObject.GetComponent<Slider>();

        MenuPanel = transform.Find("PCanvas/MenuPanel").gameObject;

        GamePanel = transform.Find("PCanvas/GamePanel").gameObject;

        ContactPanel.GetComponent<Image>().color = new UnityEngine.Color(0, 0, 0, 0);
        MenuPanel.GetComponent<Image>().color = new UnityEngine.Color(0, 0, 0, 0);
        ContactPanel.SetActive(false);
        ContactText.text = null;
    }
    void Update()
    {
        this.transform.eulerAngles = new Vector3(0, CAMERA.transform.eulerAngles.y, 0);
        if (Input.GetKeyDown(KeyCode.P)) this.transform.position = Vector3.zero;
        if (move_permit)
        {
            if (Input.GetKeyUp(KeyCode.Space)) isJumping_running = false;
            if (Input.GetKey(KeyCode.Space) //buttonが押されていて
            && !isJumping_running//ジャンプ処理中ではない場合で
            && isJumping)//接地している場合
            {
                Debug.Log("ジャンプ");
                StartCoroutine("JunpMove");
                isJumping = false;
            }

            Ray downray = new Ray(gameObject.transform.position,Vector3.down);
            RaycastHit hit;
            if (Physics.Raycast(downray, out hit, 10.0f))
            {
                var n = hit.point.y - gameObject.transform.position.y + 0.5f;
                if (Mathf.Round(n) == 0 && n < 0.1f && 0.1f > n && !isJumping_running) isJumping = true;
            }
            Debug.DrawRay(downray.origin, downray.direction * 10, UnityEngine.Color.red, 5);


            if (Input.GetKey(KeyCode.Q))
            {
                if (ConObj != null) ConObj.Contact_function();
                if (ColObj != null && CollectGage.value != CollectGage.maxValue)
                {
                    CollectGage.value += 0.02f;
                }
                else if (ColObj != null && CollectGage.value == CollectGage.maxValue) 
                {
                    Player_Manager.Item_Inventory[ColObj.collect_item_id] += ColObj.collect_item_num;
                    Debug.Log(Player_Manager.Item_Inventory[ColObj.collect_item_id]);
                    
                    ContactPanel.SetActive(false);
                    ContactPanel.GetComponent<Image>().color = new UnityEngine.Color(0, 0, 0, 0);
                    CollectGage.value = 0;
                    ColObj.CollectObj_function();
                    ColObj = null;
                    ContactText.text = null;
                }
            }
        }
    }
    void FixedUpdate()
    {
        if (move_permit)
        {
            if (rate_count >= gunlist.Performance[weapon_id].rapid_fire_rate && Input.GetKey(KeyCode.Tab))
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
        //if (other.gameObject.CompareTag("Floor")) isJumping = true;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Floor")) isJumping_running = false;
        if (other.gameObject.CompareTag("Collect"))
        {
            ContactPanel.SetActive(true);
            ContactPanel.GetComponent<Image>().color = new UnityEngine.Color(255, 255, 255, 50);
            ColObj = other.GetComponent<CollectObj_System>();
            ContactText.text = ColObj.collect_text;
            CollectGage.maxValue = ColObj.collect_time;
            CollectGage.value = 0;
        }
        if (other.gameObject.CompareTag("Contact"))
        {
            ContactPanel.SetActive(true);
            ContactPanel.GetComponent<Image>().color = new UnityEngine.Color(255, 255, 255, 50);
            ConObj = other.GetComponent<ContactObj_System>();
            ContactText.text = ConObj.contact_text;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Floor")) isJumping = false;
        if (other.gameObject.CompareTag("Collect"))
        {
            ContactPanel.SetActive(false);
            ContactPanel.GetComponent<Image>().color = new UnityEngine.Color(0, 0, 0, 0);
            ColObj = null;
            ContactText.text = null;
        }
        if (other.gameObject.CompareTag("Contact"))
        {
            ContactPanel.SetActive(false);
            ContactPanel.GetComponent<Image>().color = new UnityEngine.Color(0, 0, 0, 0);
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
            //Debug.Log("jump処理");
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
