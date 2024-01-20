using Cinemachine;
using DG.Tweening;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static ContactObj_System;
//using static System.Net.Mime.MediaTypeNames;

public class GameManager : MonoBehaviour
{
    public CinemachineInputProvider InputProvider;
    private Enemy_Manager em;
    private Scene_Manager sm;
    private Player_System ps;
    private string SavePath;
    public static GameManager instance;

    public Stage_Information si;
    public int stage_number;
    private GameObject FeedPanel;
    public int start_count;//�X�e�[�W�J�n�܂ł̃J�E���g�_�E��
    private GameObject Sc_Text;
    public int end_count;//�X�e�[�W�I���܂ł̃J�E���g�_�E��
    private GameObject Ec_Text;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        Application.targetFrameRate = 60;
        sm = transform.GetComponent<Scene_Manager>();
        em = transform.Find("Enemy_Manager").GetComponent<Enemy_Manager>();
        ps = transform.Find("Player_Manager/Player_System").GetComponent<Player_System>();
        //Stage_Information
        FeedPanel = transform.Find("System_Canvas/FeedPanel").gameObject;
        Sc_Text = transform.Find("System_Canvas/Sc_Text").gameObject;
        Ec_Text = transform.Find("System_Canvas/Ec_Text").gameObject;

        //�J�[�\���֌W
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        //�Z�[�u�֌W
        SavePath = Application.persistentDataPath + "/SaveData.json";
        Application.targetFrameRate = 60;
    }
    public void Scene_Transition_Process(int sn)
    {
        //�v���C���[��G�̍s����~
        Player_System.move_permit = false;
        Enemy_Manager.enemies_move_permit = false;
        //�J�[�\����\��
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        //�t�F�[�h�p�l���\��
        FeedPanel.SetActive(true);
        DOTween.Sequence()
        /*
         * �t�F�[�h�A�E�g������������
         * �ړ��������V�[���ֈړ�
         * �v���C���[���Z�b�g
         * �X�|�[���|�C���g�Ɉړ�
         */
        .Append(FeedPanel.GetComponent<Image>().DOFade(1, 1.0f).SetDelay(1f)//�t�F�[�h�A�E�g
        .OnComplete(() =>
        {
            sm.Load_Scene(sn);//�V�[���ړ�
            ps.Player_Reset();//�v���C���[�̏������Z�b�g������
            transform.Find("Player_Manager").gameObject.transform.position = si.data[sn].spawn_pos;
        }))
        .Append(FeedPanel.GetComponent<Image>().DOFade(0, 1.0f).SetDelay(1f)//�t�F�[�h�C��
                .OnComplete(() => {
                    Sc_Text.GetComponent<Text>().text = si.data[sn].name;//�X�e�[�W���\��
                    FeedPanel.SetActive(false);
                }))
        .Append(Sc_Text.transform.DOScale(Vector3.one * 1, 0.5f).SetEase(Ease.InQuart).SetDelay(1f)
        .OnComplete(() => {
            Player_System.move_permit = true;
            Enemy_Manager.enemies_move_permit = true;
            Sc_Text.GetComponent<Text>().text = "Start!";
        }))
        .Append(Sc_Text.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InQuart).SetDelay(0.5f))
        .Play();
        switch (sn)
        {
            case 0://���v���C
                break;
            case 1://�^�C�g��
                //em.Enemies_Spawn_Function(si.data[sn].enemies_num);
                break;
            case 2://
                //em.Enemies_Spawn_Function(si.data[sn].enemies_num);
                break;
        }
    }
    public void GameStart()
    {
        Player_System.move_permit = false;
        Enemy_Manager.enemies_move_permit = false;
        FeedPanel.SetActive(true);
        DOTween.Sequence()
        .Append(FeedPanel.GetComponent<Image>().DOFade(0, 1.0f).SetDelay(1f)
        .OnComplete(() => {
            Debug.Log(si.data[0].name);
            Sc_Text.GetComponent<Text>().text = si.data[0].name;
        }))
        .Append(Sc_Text.transform.DOScale(Vector3.one * 1, 0.5f).SetEase(Ease.InQuart).SetDelay(1f)
        .OnComplete(() => {
            Player_System.move_permit = true;
            Enemy_Manager.enemies_move_permit = true;
            Sc_Text.GetComponent<Text>().text = "Start!";
        }))
        .Append(Sc_Text.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InQuart).SetDelay(0.5f))
        .Play();
    }
    public void GameClear()
    {
        Debug.Log("�N���A");
        Player_System.move_permit = false;
        Enemy_Manager.enemies_move_permit = false;
        DOTween.Sequence()
        .Append(FeedPanel.GetComponent<Image>().DOFade(1, 1.0f).SetDelay(1f)
        .OnComplete(() =>
        {
            //sm.SM_Select_Transfer();
        }))
        .Append(FeedPanel.GetComponent<Image>().DOFade(0, 1.0f).SetDelay(1f)
        .OnComplete(() => {
            Debug.Log(si.data[0].name);
            Sc_Text.GetComponent<Text>().text = si.data[0].name;
            Player_System.move_permit = true;
            Enemy_Manager.enemies_move_permit = true;
        }))
        .Play();
    }
    public void GameOver()
    {
        Debug.Log("�Q�[���I�[�o�[");
        Player_System.move_permit = false;
        Enemy_Manager.enemies_move_permit = false;
        DOTween.Sequence()
        .Append(FeedPanel.GetComponent<Image>().DOFade(1, 1.0f).SetDelay(1f)
        .OnComplete(() =>
        {
        }))
        .Append(FeedPanel.GetComponent<Image>().DOFade(0, 1.0f).SetDelay(1f)
        .OnComplete(() => {
            Debug.Log(si.data[0].name);
            Sc_Text.GetComponent<Text>().text = si.data[0].name;
            Player_System.move_permit = true;
            Enemy_Manager.enemies_move_permit = true;
        }))
        .Play();
    }
    [Serializable]
    public class SaveData
    {
        public int Id;
        public string Name;
    }
    public void DataSave()
    {
        SaveData save = new();
        save.Id = 0709;
        save.Name = "Gardo_kia";

        string saveJson = JsonUtility.ToJson(save);
        using (StreamWriter streamWriter = new(SavePath)) { streamWriter.Write(saveJson); }
        Debug.Log("�Z�[�u���܂���");
    }
    public void DataLoad()
    {
        SaveData load;

        using (StreamReader streamReader = new(SavePath))
        {
            var loadJson = streamReader.ReadToEnd();
            load = JsonUtility.FromJson<SaveData>(loadJson);
        }
        Debug.Log(load);
        Debug.Log("���[�h���܂���");
    }
}
