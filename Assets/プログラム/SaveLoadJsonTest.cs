using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SaveLoadJsonTest : MonoBehaviour
{
    public string SavePath;
    public Text nyuryoku;
    
    void Start()
    {
        SavePath = Application.persistentDataPath + "/SaveData.json";
    }
    [Serializable]
    public class SaveData
    {
        public int Id;
        public string Name;
    }
    public void GameSave()
    {
        Debug.Log("セーブしました");
        SaveData save = new(); 
        save.Id = 0709;
        save.Name = "Gardo_kia";

        string saveJson = JsonUtility.ToJson(save);
        using (StreamWriter streamWriter = new(SavePath)) { streamWriter.Write(saveJson); }
    }
    public void GameLoad()
    {
        Debug.Log("ロードしました");
        SaveData load;

        using (StreamReader streamReader = new(SavePath))
        {
            var loadJson = streamReader.ReadToEnd();
            load = JsonUtility.FromJson<SaveData>(loadJson);
        }
        Debug.Log(load);
    }
}
