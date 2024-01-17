using DG.Tweening;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
//using static System.Net.Mime.MediaTypeNames;

public class GameManager : MonoBehaviour
{
    public Stage_Information si;
    public Enemy_Manager em;
    private string SavePath;
    public static GameManager instance;

    private GameObject FeedPanel;
    public float start_count;//ステージ開始までのカウントダウン
    private GameObject Sc_Text;
    public float end_count;//ステージ終了までのカウントダウン
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
        //Stage_Information
        em.enemies_count = si.data[0].enemies_num;
        FeedPanel = transform.Find("System_Canvas/FeedPanel").gameObject;
        Sc_Text = transform.Find("System_Canvas/Sc_Text").gameObject;
        Ec_Text = transform.Find("System_Canvas/Ec_Text").gameObject;

        //カーソル関係
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        //セーブ関係
        SavePath = Application.persistentDataPath + "/SaveData.json";
        Application.targetFrameRate = 60;

        GameStart();
    }
    void Start()
    {

    }
    [Serializable]
    public class SaveData
    {
        public int Id;
        public string Name;
    }
    void Update()
    {

    }
    public void GameSave()
    {
        SaveData save = new();
        save.Id = 0709;
        save.Name = "Gardo_kia";

        string saveJson = JsonUtility.ToJson(save);
        using (StreamWriter streamWriter = new(SavePath)) { streamWriter.Write(saveJson); }
        Debug.Log("セーブしました");
    }
    public void GameLoad()
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
    public void GameStart()
    {
        FeedPanel.SetActive(true);
        DOTween.Sequence()
        .Append(FeedPanel.GetComponent<Image>().DOFade(0, 1.0f).SetDelay(1f))
        .Append(Sc_Text.GetComponent<Text>().DOCounter(5,0,5.0f).SetEase(Ease.Linear).SetDelay(0.5f))
        .Play();
    }
    public void GameClear()
    {

    }
}
