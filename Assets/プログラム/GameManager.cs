using DG.Tweening;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Stage_Information si;
    public Enemy_Manager em;
    private string SavePath;
    public static GameManager instance;

    private GameObject FeedPanel;
    public float start_count;//�X�e�[�W�J�n�܂ł̃J�E���g�_�E��
    public float end_count;//�X�e�[�W�I���܂ł̃J�E���g�_�E��
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

        //�J�[�\���֌W
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        //�Z�[�u�֌W
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
        Debug.Log("�Z�[�u���܂���");
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
        Debug.Log("���[�h���܂���");
    }
    public void GameStart()
    {
        DOTween.Sequence()
        .Append(FeedPanel.GetComponent<Image>().DOFade(0, 1.0f).SetDelay(1f))
        //.Append(this.transform.DOMoveX(-3, 2f).SetRelative())
        .Play();
    }
    public void GameClear()
    {

    }
}
