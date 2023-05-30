using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;

public class RWFile : MonoBehaviour {

    public virtual string MSG_FAILED_TO_READ_DATA {
        get => "Failed to read file data";
    }

    public virtual string MSG_FAILED_TO_WRITE_DATA {
        get => "Failed to write data into file";
    }

    public virtual string MSG_FILEPATH_NOT_FOUND {
        get => "Filepath not found";
    }

    private readonly string RESOURCES_DATA_PATH = "/Resources/Data/";

    public void WriteFileData<T>(string fileName, T dataObj) {
        string path = GetFilePath(fileName);

        try {
            // Debug.Log(JsonConvert.SerializeObject(dataObj));
            File.WriteAllText(path, JsonConvert.SerializeObject(dataObj));
        } catch (Exception e) {
            //throw new ArgumentException(MSG_FAILED_TO_WRITE_DATA);
            throw e;
        }
    }

    public T ReadFileData<T>(string fileName) {
        T data;
        if (GameManager.instance.appType == GameConfig.AppType.GAME) {
            string jsonData = Resources.Load<TextAsset>(fileName).ToString();
            data = JsonConvert.DeserializeObject<T>(jsonData);
        } else {
            string jsonData = File.ReadAllText(GetFilePath(fileName));
            data = JsonConvert.DeserializeObject<T>(jsonData);
        }

        return data;
    }

    private string GetFilePath(string fileName) {
        string filePath = Application.dataPath + RESOURCES_DATA_PATH + fileName;

        return filePath;
    }

    private void OnApplicationPause(bool pause) {
        if (pause)
            return;
    }
}