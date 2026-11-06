using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class EnvironmentData
{
    public int currentSeason;
    public int seasonNum;
    public int year;
    public int day;
    public float time;

    public EnvironmentData(int currentSeason, int seasonNum, int year, int day, float time)
    {
        this.currentSeason = currentSeason;
        this.seasonNum = seasonNum;
        this.year = year;
        this.day = day;
        this.time = time;
    }
    public EnvironmentData() { 
    currentSeason = 0 ; seasonNum = 1; year = 0; day = 1; time = 8.00f;
    }
}
public class Environment : MonoBehaviour
{

    public static Environment Instance { get; private set; }

    private  static int[] Season = { 0, 1, 0 , -1};//春,夏,秋,冬
    private static string[] SeasonName = {  "春", "夏", "秋" ,"冬"};
    public int currentSeaon;
    private int seasoNum = 3;
    private int Year;
    public int Day;
    public float Time ;
    private float hour;
    private float minute;
    private float _light;
    private static float  starLight = 1f;


    public void YearSet()
    {
        Year++;
        Day = 1;
        Time = 8.00f;
        hour = 8;
        seasoNum = 0;
    }
   public void SkipSeason()
    {
        Day = 1;
        Time = 8.00f;
        hour = 8;
        minute = 0;
        seasoNum++;
        if (seasoNum > 3) Year++;
        seasoNum %= 4;
    }

    //季节轮替
    public void seasonTransform()
    {
        seasoNum++;
        if (seasoNum > 3) YearSet();
        seasoNum %= 4;
        currentSeaon = Season.ElementAt(seasoNum);
    }

    //传递分钟的时间变化
    public void SetTime (int min)
    {
        SeparateTime();
        minute += (float)min;
        TimeTranslate(); MerageTime();
        prinEn();
    }
    //传递小时的时间变化
    public void SetTime(float h)
    {
        SeparateTime();
        hour += (int)(h - h % 1);
        minute += 60 * (h % 1);
        TimeTranslate(); MerageTime();
        prinEn();
    }
    //更新浮点时间变量的方法
    private void MerageTime()
    {
        Time = hour + minute / 60.0f;
    }
    private void SeparateTime()
    {
        hour = (int)Time;
        minute = 60 * (Time % 1);
    }


    //分钟与小时的时间进制更新
    private void TimeTranslate()
    {
        if (minute >= 60)
        {
            minute %= 60;
            hour++;
        }
        if(hour >= 24)
        {
            hour %= 24;
            Day++;
            if(Day > 15)
            {
                Day = 1;
                seasonTransform();
                if (currentSeaon == -1)
                    Year++;
            }
        }
    }

    //一天内光照度随时间的变化
    private void setLight()
    {
        if (Time > 18 + currentSeaon || Time < 6 - currentSeaon)//夜间仅有星光
            _light = starLight;
        else                                                    //白天光照随时间变化
            _light = (9.9f + 0.2f * currentSeaon ) *Mathf.Sin(setLightseason()) * Mathf.Sin(setLightseason());
    }
    private float setLightseason()
    {
        return Mathf.Round(  Mathf.PI * (Time - 6 + currentSeaon) / (12 + currentSeaon) *100f / 100f);
    }






    /// <summary>
    /// get方法
    /// </summary>
    /// <returns></returns>
    //光照的接口
    public float getLight()
    {
        setLight();//测试用
        return _light;
    }

    public int getYear()//返回年份
    {
        return Year;
    }
    public int getSeasonNum()//返回季节编号
    {
        return seasoNum;
    }
    public string getSeason()//返回季节
    {
        return SeasonName[seasoNum];
    }
    public int getDay()//返回日期
    {
        return Day;
    }
    public float getTime()//返回时间
    {
        return Time;
    }
    
    private void prinEn()//测试用打印时间
    {
        int m = (int)(60 * (Time % 1));
        if (m >= 10)
        print( (Time - Time %1) + ":" + m + ";time:" + Time);
        else
        print((Time - Time % 1) + ":0" + m + ";time:" + Time);
        print("light:" + getLight());
    }






    /// <summary>
    /// 数据层的保存初始化方法，调用DataLoading()方法将数据初始化为默认值
    /// </summary>
    public void DataIni()
    {
        DataLoading(new EnvironmentData());
    }
    public EnvironmentData DataSaving()
    {
        return new EnvironmentData(currentSeaon, seasoNum, Year, Day, Time);
    }
    public void DataLoading(EnvironmentData data)
    {
        currentSeaon = data.currentSeason;
        seasoNum = data.seasonNum;
        Year = data.year;
        Day = data.day;
        Time = data.time;
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
