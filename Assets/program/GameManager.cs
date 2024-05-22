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
    public static float playerMoney;
    public int feedTime = 1;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Enemy_Manager enemyManager;
    [SerializeField] private Scene_Manager sceneManager;
    [SerializeField] private Player_System playerSystem;
    private string SavePath;
    public static GameManager instance;

    public Stage_Information stageInfomation;
    public int stage_number;

    private GameObject GameOverPanel;
    private Text GameOverText;
    private GameObject FeedPanel;
    public int stageStartCount;//�X�e�[�W�J�n�܂ł̃J�E���g�_�E��
    private GameObject stageStartCountText;
    public int endCount;//�X�e�[�W�I���܂ł̃J�E���g�_�E��
    private GameObject endText;
    private void Awake()
    {
    }
    private void Start()
    {
        DontDestroyOnLoad(this);
        Application.targetFrameRate = 60;
        audioSource = GetComponent<AudioSource>();
        sceneManager = transform.GetComponent<Scene_Manager>();
        enemyManager = transform.Find("Enemy_Manager").GetComponent<Enemy_Manager>();
        playerSystem = transform.Find("Player_Manager/Player_System").GetComponent<Player_System>();
        //Stage_Information
        GameOverPanel = transform.Find("System_Canvas/GameOverPanel").gameObject;
        GameOverText = GameOverPanel.transform.Find("GameOverText").GetComponent<Text>();

        FeedPanel = transform.Find("System_Canvas/FeedPanel").gameObject;
        stageStartCountText = transform.Find("System_Canvas/Sc_Text").gameObject;
        endText = transform.Find("System_Canvas/Ec_Text").gameObject;

        GameOverPanel.SetActive(false);
        audioSource.PlayOneShot(stageInfomation.data[0].BGM);
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
        Player_System.movePermit = false;
        Enemy_Manager.enemies_move_permit = false;
        enemyManager.StopCoroutine("Enemies_Spawn_Function");
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
        .Append(FeedPanel.GetComponent<Image>().DOFade(1, 1.0f*feedTime).SetDelay(0f)//�t�F�[�h�A�E�g
        .OnComplete(() =>
        {
            audioSource.Stop();
            sceneManager.Load_Scene(sn);//�V�[���ړ�
            enemyManager.Enemy_Manager_Reset();//�G�l�~�[�}�l�[�W���[���Z�b�g
            playerSystem.Player_Reset(false);//�v���C���[�̏������Z�b�g������
            transform.Find("Player_Manager/Player_System").gameObject.transform.position = stageInfomation.data[sn].spawn_pos;
        }))
        .Append(FeedPanel.GetComponent<Image>().DOFade(0, 1.0f * feedTime).SetDelay(0.5f)//�t�F�[�h�C��
                .OnComplete(() => {
                    audioSource.PlayOneShot(stageInfomation.data[sn].BGM);
                    stageStartCountText.GetComponent<Text>().text = stageInfomation.data[sn].name;//�X�e�[�W���\��
                }))
        .Append(stageStartCountText.transform.DOScale(Vector3.one * 1, 0.5f * feedTime).SetEase(Ease.InQuart).SetDelay(0.5f)
        .OnComplete(() => {
            Player_System.movePermit = true;
            Enemy_Manager.enemies_move_permit = true;
            if(sn == 2)stageStartCountText.GetComponent<Text>().text = "Start!";
        }))
        .Append(stageStartCountText.transform.DOScale(Vector3.zero, 0.5f * feedTime).SetEase(Ease.InQuart).SetDelay(0.5f)
                .OnComplete(() => {
                    Debug.Log(stageInfomation.data[sn].name);
                    stageStartCountText.transform.localScale = Vector3.one;
                    Enemy_Manager.enemies_move_permit = true;
                    stageStartCountText.GetComponent<Text>().text = "";
                    FeedPanel.SetActive(false);
                    GameOverPanel.SetActive(false);
                    switch (stageInfomation.data[sn].tran_scene)
                    {
                        case stage_information.TransitionScene.Title://�^�C�g��
                            playerSystem.Player_Reset(false);
                            break;
                        case stage_information.TransitionScene.Select://�Z���N�g
                            playerSystem.Player_Reset(true);
                            break;
                        case stage_information.TransitionScene.Main://Main
                            playerSystem.Player_Reset(true);
                            enemyManager.stagetype = stageInfomation.data[sn].stagetype;
                            List<int[]> wave = new List<int[]>
                            {
                                stageInfomation.data[sn].enemies_num1,
                                stageInfomation.data[sn].enemies_num2,
                                stageInfomation.data[sn].enemies_num3
                            };
                            enemyManager.all_wave = wave;
                            StartCoroutine(enemyManager.Enemies_Spawn_Function(wave[0]));

                            break;
                    }
                }))
        .Play();

    }
    public void GameStart()
    {
        Player_System.movePermit = false;
        Enemy_Manager.enemies_move_permit = false;
        FeedPanel.SetActive(true);
        DOTween.Sequence()
        .Append(FeedPanel.GetComponent<Image>().DOFade(0, 1.0f).SetDelay(1f)
        .OnComplete(() => {
            Debug.Log(stageInfomation.data[0].name);
            stageStartCountText.GetComponent<Text>().text = stageInfomation.data[0].name;
        }))
        .Append(stageStartCountText.transform.DOScale(Vector3.one * 1, 0.5f).SetEase(Ease.InQuart).SetDelay(1f)
        .OnComplete(() => {
            Player_System.movePermit = true;
            Enemy_Manager.enemies_move_permit = true;
            stageStartCountText.GetComponent<Text>().text = "Start!";
        }))
        .Append(stageStartCountText.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InQuart).SetDelay(0.5f))
        .Play();
    }
    public void GameClear()
    {
        Debug.Log("�N���A");
        Player_System.movePermit = false;
        Enemy_Manager.enemies_move_permit = false;
        DOTween.Sequence()
        .Append(FeedPanel.GetComponent<Image>().DOFade(1, 1.0f).SetDelay(1f)
        .OnComplete(() =>
        {
            //sm.SM_Select_Transfer();
        }))
        .Append(FeedPanel.GetComponent<Image>().DOFade(0, 1.0f).SetDelay(1f)
        .OnComplete(() => {
            Debug.Log(stageInfomation.data[0].name);
            stageStartCountText.GetComponent<Text>().text = stageInfomation.data[0].name;
            Player_System.movePermit = true;
            Enemy_Manager.enemies_move_permit = true;
        }))
        .Play();
    }
    public IEnumerator GameOver(int[] num,string str,float mo)
    {
        //�t�F�[�h�p�l���\��
        FeedPanel.SetActive(true);
        //�v���C���[��G�̍s����~
        Player_System.movePermit = false;
        Enemy_Manager.enemies_move_permit = false;
        enemyManager.StopCoroutine("Enemies_Spawn_Function");
        //�J�[�\����\��
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        playerMoney += mo;
        int total = 0;
        foreach (int i in num) total = i;
        playerSystem.cinemachineVirtualCamera.Priority = 100;
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
        playerSystem.cinemachineVirtualCamera.Priority = 1;
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
