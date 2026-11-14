using System;
using UnityEngine;


public class SceneTransform : MonoBehaviour
{
    private AudioClip Main;
    private AudioClip Town ;
    private AudioClip Home;
    private AudioClip Ushiroyama ;
    private void UpdateUI()=>TimeUImanage.Instance.UpdateUI();   
    private void Trans(string levelName, string spawnPoint)
    {
        Player.Instance.SetMoveFalse();
        Debug.Log("等待中...");
        StartCoroutine(SceneLoader.Instance.SwitchLevelAsync(levelName, spawnPoint));
        Debug.Log("加载完成");
        SceneLoader.Instance.reIsTrans();
        Player.Instance.SetMoveTrue();
    }
    
    private void TransSet(string scene, int time ,string point = null)//统一化设置场景和时间的方法，方便调用
    {
        if (EnvironmentManageer.Instance != null)
            EnvironmentManageer.Instance.SetTime(time);
        else
            Debug.LogError("[SceneTransform] Environment 未初始化！");
        Trans(scene,point);
        switch(scene)
        {
            case "Main":
                BGMManager.Instance.PlayBGM(Main);
                break;
            case "Town":
                BGMManager.Instance.PlayBGM(Town);
                break;
            case "Home":
                BGMManager.Instance.PlayBGM(Home);
                break;
            case "Ushiroyama":
                BGMManager.Instance.PlayBGM(Ushiroyama);
                break;
        }
        //ModifyPosition(scene, x, y, z ,preScene);
        UpdateUI();
    }
    private void TransSet(string scene , float time, string point = null)//统一化设置场景和时间的方法，方便调用
    {
        EnvironmentManageer.Instance.SetTime(time);
        Trans(scene,point);
        switch(scene)
        {
            case "Main":
                BGMManager.Instance.PlayBGM(Main);
                break;
            case "Town":
                BGMManager.Instance.PlayBGM(Town);
                break;
            case "Home":
                BGMManager.Instance.PlayBGM(Home);
                break;
            case "Ushiroyama":
                BGMManager.Instance.PlayBGM(Ushiroyama);
                break;
        }
        //ModifyPosition(scene, x, y, z ,preScene);
        UpdateUI();
    }


    private void Start()
    {
     Main = Resources.Load<AudioClip>("BGMS/Easy Going Medieval Tavern - Loop");
     Town = Resources.Load<AudioClip>("BGMS/Medieval City");
     Home = Resources.Load<AudioClip>("BGMS/Easy Going Medieval Tavern - Loop");
     Ushiroyama = Resources.Load<AudioClip>("BGMS/Forest 6 - Ice Forest");
    }




    public void Home_Ushiroyama()
    {
        if (!InitialProduce.Instance.WorkJudge())
            return;
        TransSet("Ushiroyama", 50, "SpawnPoint_Ushiroyama");
    }
    public void Ushiroyama_Home()
    {
        TransSet("Home", 1f, "SpawnPoint_Ushiroyama");
    }
    public void Town_Home() 
    {
        TransSet("Home", 40 , "SpawnPoint_Town");
    }
    
    public void Home_Town() 
    {
        if (!InitialProduce.Instance.WorkJudge())
            return;
        TransSet("Town", 30 , "SpawnPoint_Home");
    }

    public void Load()//读档使用
    {
        //DataManager.Instance.DataLoading();
        string location = PlayerManager.Instance.GetLocation();
        if (string.IsNullOrEmpty(location)) TransSet("Town", 0, "enter");
        else TransSet(location, 0, "enter");
        TimeUImanage.Instance.OnEnterGameScene();
        Debug.Log("进入游戏");
    }
    public void NewGame()//新的游戏使用
    {
        DataManager.Instance.AllInitial();
        TransSet("Town", 0, "enter");
        TimeUImanage.Instance.OnEnterGameScene();
        Debug.Log("进入游戏");
    }
     
   

}
