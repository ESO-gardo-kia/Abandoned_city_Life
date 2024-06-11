using Cinemachine;
using DG.Tweening;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;


public class PlayerUiSystem : MonoBehaviour
{
    public static bool isPanelOpen;
    private CollectObj_System collectObjSystem;

    [SerializeField] private GameObject contactPanel;
    [SerializeField] public Text contactText;
    [SerializeField] private Slider CollectGage;

    [SerializeField] private GameObject MenuPanel;
    [SerializeField] private Slider AimSlider;

    [SerializeField] private GameObject battleInfomationPanel;
    [SerializeField] public Slider reloadSlider;
    [SerializeField] private Slider hpSlider;
    [SerializeField] private Text hpText;
    [SerializeField] private Slider enSlider;
    [SerializeField] private Text enText;
    [SerializeField] public Image weaponImagePanel;
    [SerializeField] private Text bulletText;

    public GameObject moneyText;

    private void Start()
    {
        
    }
    void Update()
    {
        moneyText.GetComponent<Text>().text = "MONEY:" + GameManager.playerMoney.ToString();
        if (Input.GetKey(KeyCode.Q))
        {
            DropItemCollect();
        }
        if (Input.GetKeyDown(KeyCode.Tab) && !MenuPanel.activeSelf && Player_System.movePermit && !isPanelOpen)
        {
            //Canvas_Transition(MenuPanel, true);
        }
        else if (Input.GetKeyDown(KeyCode.Tab) && MenuPanel.activeSelf)
        {
            //Canvas_Transition(MenuPanel,false);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Collect"))
        {
            contactPanel.SetActive(true);
            contactPanel.GetComponent<Image>().color = new UnityEngine.Color(255, 255, 255, 50);
            collectObjSystem = other.GetComponent<CollectObj_System>();
            contactText.text = collectObjSystem.collect_text;
            CollectGage.maxValue = collectObjSystem.collect_time;
            CollectGage.value = 0;
        }
        if (other.gameObject.CompareTag("Contact"))
        {
            contactPanel.SetActive(true);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Collect"))
        {
            contactPanel.SetActive(false);
            contactPanel.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            collectObjSystem = null;
            contactText.text = null;
        }
        if (other.gameObject.CompareTag("Contact"))
        {
            //if (Contact_Type.Production_Table == contactObjSystem.contactType) contactObjSystem.cinemachineVirtualCamera.Priority = 1;
            //if (contactObjGetPanel.activeSelf) Canvas_Transition(contactObjGetPanel, false);
            contactPanel.SetActive(false);
            //contactPanel.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            contactText.text = null;
        }
    }
    public void DropItemCollect()
    {
        if (collectObjSystem != null && CollectGage.value != CollectGage.maxValue) CollectGage.value += 0.02f;
        else if (collectObjSystem != null && CollectGage.value == CollectGage.maxValue)
        {
            Player_Manager.Item_Inventory[collectObjSystem.collect_item_id] += collectObjSystem.collect_item_num;
            Debug.Log(Player_Manager.Item_Inventory[collectObjSystem.collect_item_id]);

            contactPanel.SetActive(false);
            contactPanel.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            CollectGage.value = 0;
            collectObjSystem.CollectObj_function();
            collectObjSystem = null;
            contactText.text = null;
        }
    }
    public void SsignmentStatsUi(float reload_count, float currentHp, float currentEn, float currentLoadedBullets)
    {
        reloadSlider.value = reload_count;
        hpSlider.value = currentHp;
        hpText.text = currentHp.ToString();
        enSlider.value = currentEn;
        enText.text = currentEn.ToString();
        bulletText.text = currentLoadedBullets.ToString();
    }
    public void PlayerUiReset(bool IS,float hp, float currentHp, float en, float currentEn)
    {
        if (!IS)
        {
            battleInfomationPanel.SetActive(false);
        }
        else
        {
            battleInfomationPanel.SetActive(true);
        }
        MenuPanel.transform.localScale = Vector3.zero;
        contactPanel.SetActive(false);
        MenuPanel.SetActive(false);
        reloadSlider.gameObject.SetActive(false);
        isPanelOpen = false;
        contactText.text = null;

        hpSlider.maxValue = hp;
        hpSlider.value = currentHp;
        enSlider.maxValue = en;
        enSlider.value = currentEn;
    }
    public void Canvas_Transition(
         GameObject Panel
        , bool IS
        , Rigidbody rigidbody
        , GameObject moneyText
        , GameObject idlepos
        , CinemachineVirtualCamera cinemachineVirtualCamera
        , AudioSource audioSource
        , AudioClip panelSound)
    {
        /*
         * trueÇ™âÊñ ÇäJÇ≠éûÇÃèàóù
         * falseÇ™âÊñ Çï¬Ç∂ÇÈéûÇÃèàóù
         */
        if (IS)
        {
            /*
            //brain.enabled = false;
            Player_System.movePermit = false;
            Player_System.isPanelOpen = true;
            audioSource.PlayOneShot(panelSound);
            switch (contactObjSystem2.Cont)
            {
                case Contact_Type.Production_Table:
                    moneyText.SetActive(true);
                    moneyText.GetComponent<Text>().text = "MONEY:" + GameManager.playerMoney.ToString();
                    NewMethod(rigidbody,idlepos,cinemachineVirtualCamera);
                    break;
                case Contact_Type.StageSelect:
                    NewMethod(rigidbody, idlepos, cinemachineVirtualCamera);
                    break;
            }

            contactObjSystem1.Canvas_Open();
            DOTween.Sequence()
                .Append(Panel.GetComponent<RectTransform>().DOScale(Vector3.one, 0.25f)
                .SetEase(Ease.OutCirc)
                .OnComplete(() => {
                    //brain.enabled = false;
                    Player_System.movePermit = false;
                    Player_System.isPanelOpen = true;
                    Panel.SetActive(true);
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.Confined;
                }))
                .Play();
        }
        else
        {
            audioSource.PlayOneShot(panelSound);
            DOTween.Sequence()
                .Append(Panel.GetComponent<RectTransform>().DOScale(Vector3.zero, 0.25f)
                .SetEase(Ease.OutCirc)
              .OnComplete(() => {
                  rigidbody.useGravity = true;
                  moneyText.SetActive(false);
                  contactObjSystem2.VC.Priority = 1;
                  Player_System.movePermit = true;
                  Player_System.isPanelOpen = false;
                  Cursor.visible = false;
                  Cursor.lockState = CursorLockMode.Locked;
                  contactObjSystem1.Canvas_Close();
                  Debug.Log("èIóπ");
           
              }))
                .Play();
            */
        }
    }

    private void NewMethod(Rigidbody rigidbody,GameObject idlepos,CinemachineVirtualCamera cinemachineVirtualCamera)
    {
        rigidbody.useGravity = false;
        rigidbody.velocity = Vector3.zero;
        transform.position = idlepos.transform.position;
        transform.localEulerAngles = idlepos.transform.localEulerAngles;
        cinemachineVirtualCamera.Priority = 100;
            
    }
}
