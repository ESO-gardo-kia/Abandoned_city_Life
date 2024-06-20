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
    [SerializeField] private PlayerMainSystem playerSystem;
    public static GameManager instance;
    private string SavePath;

    public Stage_Information stageInfomation;
    public int stage_number;

    [SerializeField] private GameObject GameOverPanel;
    [SerializeField] private Text GameOverText;
    [SerializeField] private GameObject FeedPanel;
    [SerializeField] private GameObject stageNameText;
    private void Awake()
    {
    }
    private void Start()
    {
        DontDestroyOnLoad(this);
        SceneStartFunction(1);
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
        PlayerMainSystem.movePermit = false;
        Enemy_Manager.enemiesMovePermit = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        DOTween.Sequence()

        .Append(FeedPanel.GetComponent<Image>().DOFade(1, 1.0f* feedTime).SetDelay(0f)//フェードアウト
                .OnComplete(() =>{
                    SceneTransitionPreProcessing(transitionSceneNumber);
                }))

        .Append(FeedPanel.GetComponent<Image>().DOFade(0, 1 * feedTime).SetDelay(0.5f)//フェードイン
                .OnComplete(() => {
                }))

        .Append(stageNameText.transform.DOScale(Vector3.one * 1, 0.5f * feedTime).SetEase(Ease.InQuart).SetDelay(0.5f)
                .OnComplete(() => {
                    if(transitionSceneNumber == 2) stageNameText.GetComponent<Text>().text = "GO!";
                }))
        .Append(stageNameText.transform.DOScale(Vector3.zero, 0.5f * feedTime).SetEase(Ease.InQuart).SetDelay(0.5f)
                .OnComplete(() =>
                {
                    stageNameText.transform.localScale = Vector3.one;
                    Enemy_Manager.enemiesMovePermit = true;
                    stageNameText.GetComponent<Text>().text = "";
                    SceneStartFunction(transitionSceneNumber);
                }))
        .Play();
    }
    private void SceneTransitionPreProcessing(int transitionSceneNumber)
    {
        audioSource.Stop();
        enemyManager.Enemy_Manager_Reset();
        playerSystem.Player_Reset(false);
        sceneManager.Load_Scene(transitionSceneNumber);
        audioSource.PlayOneShot(stageInfomation.data[transitionSceneNumber].BGM);
        stageNameText.GetComponent<Text>().text = stageInfomation.data[transitionSceneNumber].name;
        playerSystem.gameObject.transform.position = stageInfomation.data[transitionSceneNumber].spawn_pos;
    }
    private void SceneStartFunction(int transitionSceneNumber)
    {
        FeedPanel.SetActive(false);
        GameOverPanel.SetActive(false);
        switch (stageInfomation.data[transitionSceneNumber].tran_scene)
        {
            case stage_information.TransitionScene.Title:
                PlayerMainSystem.movePermit = false;
                Enemy_Manager.enemiesMovePermit = false;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
                playerSystem.Player_Reset(false);
                break;
            case stage_information.TransitionScene.Select:
                PlayerMainSystem.movePermit = true;
                Enemy_Manager.enemiesMovePermit = false;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                playerSystem.Player_Reset(true);
                break;
            case stage_information.TransitionScene.Main:
                PlayerMainSystem.movePermit = true;
                Enemy_Manager.enemiesMovePermit = true;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                playerSystem.Player_Reset(true);

                enemyManager.stagetype = stageInfomation.data[transitionSceneNumber].stagetype;
                enemyManager.GetCurrentWaveEnemy(transitionSceneNumber);
                StartCoroutine(enemyManager.Enemies_Spawn_Function(transitionSceneNumber));
                break;
        }
    }
    public IEnumerator GameOver(int[] num,string str,float money)
    {
        FeedPanel.SetActive(true);
        PlayerMainSystem.movePermit = false;
        Enemy_Manager.enemiesMovePermit = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        enemyManager.StopCoroutine("Enemies_Spawn_Function");
        playerMoney += money;
        int total = 0;
        foreach (int i in num) total = i;
        playerSystem.cinemachineVirtualCamera.Priority = 100;
        GameOverPanel.SetActive(true);
        GameOverText.text =
            "Stage1 : " + str +
            "\r\nGet Money : " + money.ToString() +
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
