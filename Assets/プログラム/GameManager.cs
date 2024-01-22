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
    private Enemy_Manager em;
    private Scene_Manager sm;
    private Player_System ps;
    private string SavePath;
    public static GameManager instance;

    public Stage_Information si;
    public int stage_number;
    private GameObject FeedPanel;
    public int start_count;//ステージ開始までのカウントダウン
    private GameObject Sc_Text;
    public int end_count;//ステージ終了までのカウントダウン
    private GameObject Ec_Text;
    private void Awake()
    {
    }
    private void Start()
    {
        DontDestroyOnLoad(this);
        Application.targetFrameRate = 60;
        sm = transform.GetComponent<Scene_Manager>();
        em = transform.Find("Enemy_Manager").GetComponent<Enemy_Manager>();
        ps = transform.Find("Player_Manager/Player_System").GetComponent<Player_System>();
        //Stage_Information
        FeedPanel = transform.Find("System_Canvas/FeedPanel").gameObject;
        Sc_Text = transform.Find("System_Canvas/Sc_Text").gameObject;
        Ec_Text = transform.Find("System_Canvas/Ec_Text").gameObject;

        //カーソル関係
        /*
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        */
        //セーブ関係
        SavePath = Application.persistentDataPath + "/SaveData.json";
        Application.targetFrameRate = 60;
    }
    public void Scene_Transition_Process(int sn)
    {
        Debug.Log("scene変更");
        //フェードパネル表示
        FeedPanel.SetActive(true);
        //プレイヤーや敵の行動停止
        Player_System.move_permit = false;
        Enemy_Manager.enemies_move_permit = false;
        //カーソル非表示
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        DOTween.Sequence()
        /*
         * フェードアウトが完了したら
         * 移動したいシーンへ移動
         * プレイヤーリセット
         * スポーンポイントに移動
         */
        .Append(FeedPanel.GetComponent<Image>().DOFade(1, 1.0f).SetDelay(1f)//フェードアウト
        .OnComplete(() =>
        {
            sm.Load_Scene(sn);//シーン移動
            ps.Player_Reset(true);//プレイヤーの情報をリセットさせる
            em.Enemy_Manager_Reset();//エネミーマネージャーリセット
            transform.Find("Player_Manager/Player_System").gameObject.transform.position = si.data[sn].spawn_pos;
        }))
        .Append(FeedPanel.GetComponent<Image>().DOFade(0, 1.0f).SetDelay(0.5f)//フェードイン
                .OnComplete(() => {
                    Debug.Log(FeedPanel);
                    Sc_Text.GetComponent<Text>().text = si.data[sn].name;//ステージ名表示
                }))
        .Append(Sc_Text.transform.DOScale(Vector3.one * 1, 0.5f).SetEase(Ease.InQuart).SetDelay(1f)
        .OnComplete(() => {
            Player_System.move_permit = true;
            Enemy_Manager.enemies_move_permit = true;
            Sc_Text.GetComponent<Text>().text = "Start!";
        }))
        .Append(Sc_Text.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InQuart).SetDelay(0.5f)
                .OnComplete(() => {
                    Player_System.move_permit = true;
                    Enemy_Manager.enemies_move_permit = true;
                    Sc_Text.GetComponent<Text>().text = "Start!";
                    switch (sn)
                    {
                        case 0://タイトル
                            ps.Player_Reset(false);//プレイヤーの情報をリセットさせる
                            break;
                        case 1://セレクト
                            
                            //em.Enemies_Spawn_Function(si.data[sn].enemies_num);
                            break;
                        case 2://Main
                            //if(si.data[sn].name == SceneManager.GetActiveScene().name)
                            StartCoroutine(em.Enemies_Spawn_Function(si.data[sn].enemies_num));
                            break;
                    }
                }))
        .Play();
        FeedPanel.SetActive(false);
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
        Debug.Log("クリア");
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
        Debug.Log("ゲームオーバー");
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
