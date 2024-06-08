using DG.Tweening;
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using static Stage_Information;
//using static System.Net.Mime.MediaTypeNames;

public class GameManager : MonoBehaviour
{
    public static float playerMoney;
    public int feedTime = 1;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Enemy_Manager enemyManager;
    [SerializeField] private SceneTransitionSystem sceneManager;
    [SerializeField] private Player_System playerSystem;
    public static GameManager instance;
    private string SavePath;

    public Stage_Information stageInfomation;
    public int stage_number;

    [SerializeField] private GameObject GameOverPanel;
    [SerializeField] private Text GameOverText;
    [SerializeField] private GameObject FeedPanel;
    [SerializeField] public int stageStartCount;//ステージ開始までのカウントダウン
    [SerializeField] private GameObject stageStartCountText;
    [SerializeField] public int endCount;//ステージ終了までのカウントダウン
    [SerializeField] private GameObject endText;
    private void Awake()
    {
    }
    private void Start()
    {
        /*
        Player_System.movePermit = false;
        Enemy_Manager.enemies_move_permit = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        */
        DontDestroyOnLoad(this);
        Application.targetFrameRate = 60;
        GameOverPanel.SetActive(false);
        audioSource.PlayOneShot(stageInfomation.data[0].BGM);
        SavePath = Application.persistentDataPath + "/SaveData.json";
        Application.targetFrameRate = 60;
        playerMoney = 10000;
    }
    public void SceneTransitionProcess(int transitionSceneNumber)
    {
        FeedPanel.SetActive(true);
        Player_System.movePermit = false;
        Enemy_Manager.enemies_move_permit = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        DOTween.Sequence()
        .Append(FeedPanel.GetComponent<Image>().DOFade(1, 1.0f* feedTime).SetDelay(0f)//フェードアウト
                .OnComplete(() =>
                {
                    SceneTransitionPreprocessing(transitionSceneNumber);
                }))
        .Append(FeedPanel.GetComponent<Image>().DOFade(0, 1 * feedTime).SetDelay(0.5f)//フェードイン
                .OnComplete(() => {
                    audioSource.PlayOneShot(stageInfomation.data[transitionSceneNumber].BGM);
                    stageStartCountText.GetComponent<Text>().text = stageInfomation.data[transitionSceneNumber].name;//ステージ名表示
                }))
        .Append(stageStartCountText.transform.DOScale(Vector3.one * 1, 0.5f * feedTime).SetEase(Ease.InQuart).SetDelay(0.5f)
                .OnComplete(() => {
                    Player_System.movePermit = true;
                     Enemy_Manager.enemies_move_permit = true;
                    if(transitionSceneNumber == 2) stageStartCountText.GetComponent<Text>().text = "Start!";
                }))
        .Append(stageStartCountText.transform.DOScale(Vector3.zero, 0.5f * feedTime).SetEase(Ease.InQuart).SetDelay(0.5f)
                .OnComplete(() =>
                {
                    SceneTransitionCompletedFunction(transitionSceneNumber);
                }))
        .Play();

    }
    private void SceneTransitionPreprocessing(int transitionSceneNumber)
    {
        audioSource.Stop();
        enemyManager.Enemy_Manager_Reset();
        playerSystem.Player_Reset(false);
        sceneManager.Load_Scene(transitionSceneNumber);//シーン移動
        playerSystem.gameObject.transform.position = stageInfomation.data[transitionSceneNumber].spawn_pos;
    }
    private void SceneTransitionCompletedFunction(int transitionSceneNumber)
    {
        stageStartCountText.transform.localScale = Vector3.one;
        Enemy_Manager.enemies_move_permit = true;
        stageStartCountText.GetComponent<Text>().text = "";
        FeedPanel.SetActive(false);
        GameOverPanel.SetActive(false);
        switch (stageInfomation.data[transitionSceneNumber].tran_scene)
        {
            case stage_information.TransitionScene.Title:
                playerSystem.Player_Reset(false);
                break;
            case stage_information.TransitionScene.Select:
                playerSystem.Player_Reset(true);
                break;
            case stage_information.TransitionScene.Main:
                playerSystem.Player_Reset(true);
                enemyManager.stagetype = stageInfomation.data[transitionSceneNumber].stagetype;
                enemyManager.GetCurrentWaveEnemy(transitionSceneNumber);
                StartCoroutine(enemyManager.Enemies_Spawn_Function(transitionSceneNumber));
                break;
        }
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
        Debug.Log("クリア");
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
        //フェードパネル表示
        FeedPanel.SetActive(true);
        //プレイヤーや敵の行動停止
        Player_System.movePermit = false;
        Enemy_Manager.enemies_move_permit = false;
        enemyManager.StopCoroutine("Enemies_Spawn_Function");
        //カーソル非表示
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
        SceneTransitionProcess(1);
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
        Debug.Log("セーブしました");
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
        Debug.Log("ロードしました");
    }
}
