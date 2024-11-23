using DG.Tweening;
using NUnit.Framework;
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Stage_Information;
//using static System.Net.Mime.MediaTypeNames;

public class GameManager : MonoBehaviour
{
    public static float playerMoney;
    public static GameManager GameManagerInstance;
    public float feedTime = 1;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Enemy_Manager enemyManager;
    [SerializeField] private SceneTransitionSystem sceneTransitionSystem;
    [SerializeField] private PlayerMainSystem playerMainSystem;
    [SerializeField] private Enemy_List enemyList;
    private string SavePath;

    public Stage_Information stageInfomation;
    public int stage_number;

    [SerializeField] private GameObject GameOverPanel;
    [SerializeField] private Text ResultInfomationText;
    [SerializeField] private GameObject ClearText;
    [SerializeField] private GameObject GameOverText;
    [SerializeField] private GameObject FeedPanel;
    [SerializeField] private GameObject stageNameText;
    private void Awake()
    {
        if(GameManagerInstance == null)
        {
            GameManagerInstance = this;
        }
        else if(GameManagerInstance != this)
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        DontDestroyOnLoad(this);
        Application.targetFrameRate = 60;
        SceneStartFunction(SceneManager.GetActiveScene().buildIndex,0);
        SavePath = Application.persistentDataPath + "/SaveData.json";
    }
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.O))
        {
            playerMoney += 10000;
        }
    }
    public void SceneTransitionProcess(int transitionSceneNumber, int stageNumber)
    {
        FeedPanel.SetActive(true);
        PlayerMainSystem.movePermit = false;
        Enemy_Manager.enemiesMovePermit = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Debug.Log(playerMainSystem.gameObject.transform.position);
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
                    SceneStartFunction(transitionSceneNumber, stageNumber);
                }))
        .Play();
    }
    private void SceneTransitionPreProcessing(int transitionSceneNumber)
    {
        audioSource.Stop();
        enemyManager.Enemy_Manager_Reset();
        playerMainSystem.Player_Reset(false);
        sceneTransitionSystem.Load_Scene(transitionSceneNumber);
        stageNameText.GetComponent<Text>().text = stageInfomation.data[transitionSceneNumber].name;
        playerMainSystem.gameObject.transform.position = stageInfomation.data[transitionSceneNumber].spawn_pos;
    }
    private void SceneStartFunction(int currentSceneNumber, int stageNumber)
    {
        FeedPanel.SetActive(false);
        ClearText.SetActive(false);
        GameOverPanel.SetActive(false);
        GameOverText.SetActive(false);
        audioSource.clip = stageInfomation.data[stageNumber].BGM;
        audioSource.Play();
        switch (currentSceneNumber)
        {
            case 0:
                Debug.Log("koko");
                PlayerMainSystem.movePermit = false;
                Enemy_Manager.enemiesMovePermit = false;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
                playerMainSystem.Player_Reset(false);
                break;
            case 1:
                PlayerMainSystem.movePermit = true;
                Enemy_Manager.enemiesMovePermit = false;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                playerMainSystem.Player_Reset(true);
                break;
            case 2:
                PlayerMainSystem.movePermit = true;
                Enemy_Manager.enemiesMovePermit = true;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                playerMainSystem.Player_Reset(true);

                enemyManager.stagetype = stageInfomation.data[stageNumber].stagetype;
                enemyManager.GetCurrentWaveEnemy(stageNumber);
                StartCoroutine(enemyManager.Enemies_Spawn_Function(stageNumber));
                break;
        }
    }
    public IEnumerator GameOver(int[] enemyKillList,float money,float totalTime ,bool isClear)
    {
        Debug.Log(enemyKillList);
        FeedPanel.SetActive(true);
        PlayerMainSystem.movePermit = false;
        Enemy_Manager.enemiesMovePermit = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        enemyManager.StopCoroutine("Enemies_Spawn_Function");
        playerMoney += money;
        int total = 0;
        foreach (int i in enemyKillList) total = i;
        playerMainSystem.resultCamera.Priority = 10;
        GameOverPanel.SetActive(true);
        if (isClear)
        {
            ClearText.SetActive(true);
        }
        else
        {
            GameOverText.SetActive(true);
        }
        ResultInfomationText.text =
        "\nBattle Time : " + Math.Round(totalTime).ToString() + "second" +
        "\nGet Money : " + money.ToString() +

        "\n\nDestroyed Enemyes : " + total.ToString();
        for (int i = 0; i < enemyList.data.Count; i++)
        {
            if (enemyKillList[i] > 0)
            {
                ResultInfomationText.text += "\n" + enemyList.data[i].name + ":" + enemyKillList[i].ToString();
            }
        }
        yield return new WaitForSeconds(5);
        if (isClear) ClearText.SetActive(false);
        else GameOverText.SetActive(false);
        playerMainSystem.resultCamera.Priority = 1;
        GameOverPanel.SetActive(false);
        SceneTransitionProcess(1,0);
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
