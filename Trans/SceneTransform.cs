using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneTransform : MonoBehaviour
{

    private void UpdateUI()=>TimeUImanage.Instance.UpdateUI();   
    private void Trans(string levelName, string spawnPoint)
    {
        Debug.Log("等待中...");
        StartCoroutine(SceneLoader.Instance.SwitchLevelAsync(levelName, spawnPoint));
        Debug.Log("加载完成");
        SceneLoader.Instance.reIsTrans();
    }
    
    private void TransSet(string scene, int time ,string point = null)//统一化设置场景和时间的方法，方便调用
    {
        Environment.Instance.setTime(time);
        Trans(scene,point);
        //ModifyPosition(scene, x, y, z ,preScene);
        UpdateUI();
    }
    private void TransSet(string scene , float time, string point = null)//统一化设置场景和时间的方法，方便调用
    {
        Environment.Instance.setTime(time);
        Trans(scene,point);
        //ModifyPosition(scene, x, y, z ,preScene);
        UpdateUI();
    }
    private void ModifyPosition(string scene,float x , float y ,float z , string preScene) 
        => Player.Instance.ModifyPosition(scene , x, y, z , preScene);
    





    public void Town_Ushiroyama()=>TransSet("Ushiroyama", 50,"SpawnPoint_Town");
    
    public void Ushiroyama_Town() => TransSet("Town", 1f , "SpawnPoint_Ushiroyama");
    
    public void Town_Home() => TransSet("Home", 40 , "SpawnPoint_Town");
    
    public void Home_Town() => TransSet("Town", 30 , "SpawnPoint_Home");
    

    public void Load()//后续修改完善
    {
        string location = PlayerManager.Instance.GetLocation();
        if (string.IsNullOrEmpty(location)) TransSet("Town", 0, "enter");
        else TransSet(location, 0 , "enter"); 
        TimeUImanage.Instance.OnEnterGameScene();
        Debug.Log("进入游戏");
    }

    /// <summary>
    /// 废弃
    /// </summary>
    public void Loading()//进入游戏场景时的加载方法，主要用于进入场景时
    {
        string location = PlayerManager.Instance.GetLocation();
        Vector3 position = new Vector3(PlayerManager.Instance.Position.x, PlayerManager.Instance.Position.y, PlayerManager.Instance.Position.z);

        TimeUImanage.Instance.OnEnterGameScene();
        ModifyPosition(location, position.x, position.y, position.z, "MainScene");
    }

}
