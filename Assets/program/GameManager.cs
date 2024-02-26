using Cinemachine;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static ContactObj_System;
using static Stage_Information;
//using static System.Net.Mime.MediaTypeNames;

public class GameManager : MonoBehaviour
{
    public static float Money;
    public int num = 1;
    public static bool isDotweem;
    private AudioSource AS;
    private Enemy_Manager em;
    private Scene_Manager sm;
    private Player_System ps;
    private string SavePath;
    public static GameManager instance;

    public Stage_Information si;
    public int stage_number;

    private GameObject GameOverPanel;
    private Text GameOverText;
    private GameObject FeedPanel;
    public int start_count;//�X�e�[�W�J�n�܂ł̃J�E���g�_�E��
    private GameObject Sc_Text;
    public int end_count;//�X�e�[�W�I���܂ł̃J�E���g�_�E��
    private GameObject Ec_Text;
    private void Awake()
    {
    }
    private void Start()
    {
        Money = 10000;
        DontDestroyOnLoad(this);
        Application.targetFrameRate = 60;
        AS = GetComponent<AudioSource>();
        sm = transform.GetComponent<Scene_Manager>();
        em = transform.Find("Enemy_Manager").GetComponent<Enemy_Manager>();
        ps = transform.Find("Player_Manager/Player_System").GetComponent<Player_System>();
        //Stage_Information
        GameOverPanel = transform.Find("System_Canvas/GameOverPanel").gameObject;
        GameOverText = GameOverPanel.transform.Find("GameOverText").GetComponent<Text>();

        FeedPanel = transform.Find("System_Canvas/FeedPanel").gameObject;
        Sc_Text = transform.Find("System_Canvas/Sc_Text").gameObject;
        Ec_Text = transform.Find("System_Canvas/Ec_Text").gameObject;

        GameOverPanel.SetActive(false);
        AS.PlayOneShot(si.data[0].BGM);
        //�J�[�\���֌W
        /*
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        */
        //�Z�[�u�֌W
        SavePath = Application.persistentDataPath + "/SaveData.json";
        Application.targetFrameRate = 60;
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            //GameOver();
        }
    }
    public void Scene_Transition_Process(int sn)
    {
        //�t�F�[�h�p�l���\��
        FeedPanel.SetActive(true);
        //�v���C���[��G�̍s����~
        Player_System.move_permit = false;
        Enemy_Manager.enemies_move_permit = false;
        em.StopCoroutine("Enemies_Spawn_Function");
        //�J�[�\����\��
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        DOTween.Sequence()
        /*
         * �t�F�[�h�A�E�g������������
         * �ړ��������V�[���ֈړ�
         * �v���C���[���Z�b�g
         * �X�|�[���|�C���g�Ɉړ�
         */
        .Append(FeedPanel.GetComponent<Image>().DOFade(1, 1.0f*num).SetDelay(0f)//�t�F�[�h�A�E�g
        .OnComplete(() =>
        {
            AS.Stop();
            sm.Load_Scene(sn);//�V�[���ړ�
            em.Enemy_Manager_Reset();//�G�l�~�[�}�l�[�W���[���Z�b�g
            ps.Player_Reset(false);//�v���C���[�̏������Z�b�g������
            transform.Find("Player_Manager/Player_System").gameObject.transform.position = si.data[sn].spawn_pos;
        }))
        .Append(FeedPanel.GetComponent<Image>().DOFade(0, 1.0f * num).SetDelay(0.5f)//�t�F�[�h�C��
                .OnComplete(() => {
                    AS.PlayOneShot(si.data[sn].BGM);
                    Sc_Text.GetComponent<Text>().text = si.data[sn].name;//�X�e�[�W���\��
                }))
        .Append(Sc_Text.transform.DOScale(Vector3.one * 1, 0.5f * num).SetEase(Ease.InQuart).SetDelay(0.5f)
        .OnComplete(() => {
            Player_System.move_permit = true;
            Enemy_Manager.enemies_move_permit = true;
            if(sn == 2)Sc_Text.GetComponent<Text>().text = "Start!";
        }))
        .Append(Sc_Text.transform.DOScale(Vector3.zero, 0.5f * num).SetEase(Ease.InQuart).SetDelay(0.5f)
                .OnComplete(() => {
                    Debug.Log(si.data[sn].name);
                    Sc_Text.transform.localScale = Vector3.one;
                    Enemy_Manager.enemies_move_permit = true;
                    Sc_Text.GetComponent<Text>().text = "";
                    FeedPanel.SetActive(false);
                    GameOverPanel.SetActive(false);
                    switch (si.data[sn].tran_scene)
                    {
                        case stage_information.TransitionScene.Title://�^�C�g��
                            ps.Player_Reset(false);
                            break;
                        case stage_information.TransitionScene.Select://�Z���N�g
                            ps.Player_Reset(true);
                            break;
                        case stage_information.TransitionScene.Main://Main
                            ps.Player_Reset(true);
                            em.stagetype = si.data[sn].stagetype;
                            List<int[]> wave = new List<int[]>
                            {
                                si.data[sn].enemies_num1,
                                si.data[sn].enemies_num2,
                                si.data[sn].enemies_num3
                            };
                            em.all_wave = wave;
                            StartCoroutine(em.Enemies_Spawn_Function(wave[0]));

                            break;
                    }
                }))
        .Play();

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
    public IEnumerator GameOver(int[] num,string str,float mo)
    {
        //�t�F�[�h�p�l���\��
        FeedPanel.SetActive(true);
        //�v���C���[��G�̍s����~
        Player_System.move_permit = false;
        Enemy_Manager.enemies_move_permit = false;
        em.StopCoroutine("Enemies_Spawn_Function");
        //�J�[�\����\��
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Money += mo;
        int total = 0;
        foreach (int i in num) total = i;
        ps.CVC.Priority = 100;
        GameOverPanel.SetActive(true);
        GameOverText.text =
            "Stage1 : " + str +
            "\r\nGet Money : " + mo.ToString() +
            "\r\n\r\nDestroyed Enemyes" + 
            "\r\nShooter:" + num[0].ToString() +
            "\r\nAssault:" + num[1].ToString() +
            "\r\nClusterCatapult:" + num[2].ToString() +
            "\r\nFollowingShooter:" + num[3].ToString();
        yield return new WaitForSeconds(5);
        ps.CVC.Priority = 1;
        GameOverPanel.SetActive(false);
        Scene_Transition_Process(1);
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