using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

[System.Serializable]
public class PlayerData
{

    [Header("玩家数据信息")]
    [Tooltip("玩家名字")]
    public string Name = "初始名称";
    [Tooltip("速度")]
    public float Speed;
    [Tooltip("余额")]
    public float Money;
    [Tooltip("精力")]
    public float Vitality;
    [Tooltip("心情")]
    public float Mood;
    [Tooltip("场景")]
    public string Location;
    [Tooltip("位置")]
    public float X, Y, Z;
    public PlayerData(string name, float speed, float money, float vitality, float mood , string location ,float x , float y , float z )
    {
        Name = name;
        Speed = speed;
        Money = money;
        Vitality = vitality;
        Mood = mood;
        Location = location ;
        X =  x;
        Y =  y;
        Z =  z;
    }
    public PlayerData() { 
        Name = "初始名称";Speed = 5f;Money = 1000f;Vitality = 100f;Mood = 100f;
        Location = "Town";X = 0f; Y = 0f; Z = 0f;
    }
}


public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    [Header("玩家基础")]
    [Tooltip("玩家名字")]
    private string Name;
    [Tooltip("速度")]
    public float Speed;
    [Tooltip("余额")]
    public float Money;
    [Tooltip("精力")]
    public float Vitality;
    [Tooltip("心情")]
    public float Mood;
    [Tooltip("状态等带来的效率加成")]
    private float effience = 0;
    private float err;
    public string Location;
    public Vector3 Position;


    private void Update()
    {
        SetErr(); SetEffience();//应该不需要放在update里面
    }

    //工作效率的set和return
    private void SetEffience()///拟合函数之后再确定
    {
        effience = 150 / (Mood + Vitality);
    }
    public float GetEffience()
    {
        return effience;
    }

    //出错概率的set和return
    private void SetErr()///拟合函数之后再确定
    {
        err = 150 / (Mood + Vitality);
    }
    public float ReturnErr()
    {
        return err;
    }

    //money的增减以及获取
    public void ModifyMoney(float money)
    {
        Money += money;
    }
    public float GetMoney()
    {
        return Money;
    }



    public void DataIni()//初始数据
    {
        DataLoading(new PlayerData());
    }
    public PlayerData DataSaving()//保存数据
    {
        string currentSceneName = UnitySceneManager.GetActiveScene().name;
        Vector3 po = Player.Instance.GetPosition();
        float x = po.x;
        float y = po.y;
        float z = po.z;
        return new PlayerData(Name, Speed, Money, Vitality, Mood ,currentSceneName , x ,y ,z );
    }
    public void DataLoading(PlayerData data)//加载数据
    {
        Name = data.Name;
        Speed = data.Speed;
        Money = data.Money;
        Vitality = data.Vitality;
        Mood = data.Mood;
        Location = data.Location;
        Position = new Vector3(data.X, data.Y, data.Z);
    }
    public string GetLocation()
    {
        return Location.Equals( "MainScene") ?  "Town" : Location;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }


}