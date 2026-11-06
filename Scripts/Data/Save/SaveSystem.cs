using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using static SaveData;

// SaveSystem.cs
public class SaveSystem : MonoBehaviour
{

    private const string SAVE_PATH = "save.json";
    private List<ISavable> savableObjects;

    private void Awake()
    {
        // 自动收集 Resources 中所有实现了 ISavable 的 SO
        savableObjects = Resources.LoadAll<ScriptableObject>("")
            .OfType<ISavable>()
            .ToList();
    }
    public void SaveAll()
    {
        var saveData = new GameSaveData
        {
            Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };

        foreach (var obj in savableObjects)
        {
            if (obj is ISavable savable)
            {
                string json = JsonUtility.ToJson(obj);
                saveData.DataBlocks[savable.GetSaveId()] = json;
            }
        }

        string finalJson = JsonUtility.ToJson(saveData);
        File.WriteAllText(GetPath(), finalJson);
    }

    public void LoadAll()
    {
        string path = GetPath();
        if (!File.Exists(path)) return;

        string finalJson = File.ReadAllText(path);
        var saveData = JsonUtility.FromJson<GameSaveData>(finalJson);

        foreach (var obj in savableObjects)
        {
            if (obj is ISavable savable && 
                saveData.DataBlocks.TryGetValue(savable.GetSaveId(), out string json))
            {
                JsonUtility.FromJsonOverwrite(json, obj);
            }
        }
    }

    private string GetPath() => 
        Path.Combine(Application.persistentDataPath, SAVE_PATH);

}