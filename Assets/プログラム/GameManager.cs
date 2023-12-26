using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public string SavePath;
    public Text fpsText;

    void Start()
    {
        //�J�[�\���֌W
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        //�Z�[�u�֌W
        SavePath = Application.persistentDataPath + "/SaveData.json";
        Application.targetFrameRate = 60;
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
}
