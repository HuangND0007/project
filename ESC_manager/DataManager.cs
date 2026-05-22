using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;
//using System.Text.Json;


public class DataManager : MonoBehaviour
{
    private void EnvironmentSaving()
    { 
        var EnvironmentData = Environment.Instance.DataSaving();
        string json_envir = JsonMapper.ToJson(EnvironmentData);
        string path_envir = Application.streamingAssetsPath + "/environmentdata.json";
        using (StreamWriter sw = new StreamWriter(path_envir))
        {
            sw.Write(json_envir);
        }
        Debug.Log("环境数据保存成功");
    }
    private void PlayerSaving()
    {
        var  playerdata = PlayerManager.Instance.DataSaving();
        string json_player = JsonMapper.ToJson(playerdata);
        string path_player = Application.streamingAssetsPath + "/playerdata.json";
        using (StreamWriter sw = new StreamWriter(path_player))
        {
            byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(json_player);
            sw.Write(json_player);
        }
        Debug.Log("玩家数据保存成功");
    }
    private void BackpackSaving()
    {
        var backpackdata = BackpackManager.Instance.DataSaving();
        string json_backpack = JsonMapper.ToJson(backpackdata);
        string path_backpack = Application.streamingAssetsPath + "/backpackdata.json";
        using (StreamWriter sw = new StreamWriter(path_backpack))
        {
            sw.Write(json_backpack);
        }
    }


     private void DataSaving()
    {
        PlayerSaving();
        EnvironmentSaving();
        BackpackSaving();
    }
    private void DataLoading()
    { 
        PlayerLoading();
        EnveromentLoading();
        BackpackLoading();

    }
    private void PlayerLoading()
    {
        string json;
        string path_player = Application.streamingAssetsPath + "/playerdata.json";
        if (File.Exists(path_player) )
        {
        using (StreamReader sr = new StreamReader(path_player))
        {
            json = sr.ReadToEnd();
            sr.Close();
        }
        var playerdata = JsonMapper.ToObject<PlayerData>(json);
        PlayerManager.Instance.DataLoading(playerdata);
        
        Debug.Log("玩家数据加载成功");
        }
        else
        {
            PlayerManager.Instance.DataIni();
            Debug.Log("玩家数据初始化完成");
        }
    }
    private void EnveromentLoading()
    {
        string json;
        string path_envir = Application.streamingAssetsPath + "/environmentdata.json";
        if(File.Exists(path_envir))
        {
        using (StreamReader sr = new StreamReader(path_envir))
        {
            json = sr.ReadToEnd();
            sr.Close();
        }
        var Enveromentdata = JsonMapper.ToObject<EnvironmentData>(json);
        Environment.Instance.DataLoading(Enveromentdata);
        Debug.Log("环境数据加载成功"); 
        }
        else
        {
            Environment.Instance.DataIni();
            Debug.Log("环境初始化完成");
        } 
    }
    private void BackpackLoading()
    {
        string json;
        string path_backpack = Application.streamingAssetsPath + "/backpackdata.json";
        if(File.Exists(path_backpack))
        {
        using (StreamReader sr = new StreamReader(path_backpack))
        {
            json = sr.ReadToEnd();
            sr.Close();
        }
        var backpackdata = JsonMapper.ToObject<BackpackItems>(json);
        BackpackManager.Instance.DataLoading(backpackdata);
        Debug.Log("背包数据加载成功"); 
        }
        else
        {
            BackpackManager.Instance.DataIni();
            Debug.Log("背包数据初始化完成");
        }
    }


    public void ButtonOnClick()
    {
        Debug.Log("点击了退出按钮");
        DataSaving();
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
        
    private void Awake()
    {
        //DontDestroyOnLoad(transform.root.gameObject);
        DataLoading();
    }
  


}


