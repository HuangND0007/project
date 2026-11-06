using UnityEngine;

[System.Serializable]
public struct AgricultureData
{
    public float Cooperation;
    public float ProductiveValue;
    public float Destory;
    public float Nutrition;
    public bool Ripe;
    public float Output;
    public float SeededNum;
    public float Area;//农田面积
    public int[] RipeNum;//成长度

    public AgricultureData(float cooperation = 1f,
        float productiveValue = 0, float destory = 0, float nutrition = 20, bool ripe = false, float output = 0,
        float seededNum = 0, float area = 0, int[] ripeNum = null)
    {
        Cooperation = cooperation;
        ProductiveValue = productiveValue;
        Destory = destory;
        Nutrition = nutrition;
        Ripe = ripe;
        Output = output;
        SeededNum = seededNum;
        Area = area; 
        if (ripeNum != null)
            RipeNum = ripeNum;
        else RipeNum = new int[] { 0, 0, 0, 0, 0, 0 };
    }
}

[System.Serializable]
public struct FisheryData
{
    public float Coperation;
    public float ProductiveValue;
    public float Destory;
    public float Nutrition;
    public bool Ripe;
    public float Output;
    public float FishNum;
    public float Area;
    public int[] RipeNum;

    public FisheryData(float cooperation = 1f,
       float productiveValue = 0, float destory = 0, float nutrition = 20, bool ripe = false, float output = 0,
        float FishNum = 20, float area = 0, int[] ripeNum = null)
    {
        Coperation = cooperation;
        ProductiveValue = productiveValue;
        Destory = destory;
        Nutrition = nutrition;
        Ripe = ripe;
        Output = output;
        this.FishNum = FishNum;
        Area = area;
        if (ripeNum != null)
            RipeNum = ripeNum;
        else RipeNum = new int[] { 0, 0, 0, 0, 0, 0 };
    }
}

[System.Serializable]
public struct HerdingData
{
    public float Input;
    public float ProductiveValue;
    public float Destory;
    public float Nutrition;
    public bool Ripe;
    public float Output;

    public HerdingData(float input = 0, float productiveValue = 0,
        float destory = 0, float nutrition = 20, bool ripe = false, float output = 0)
    {
        Input = input;
        ProductiveValue = productiveValue;
        Destory = destory;
        Nutrition = nutrition;
        Ripe = ripe;
        Output = output;
    }
}
[SerializeField]
public struct UshiroyamaData 
{
    public float Froest;//林木存量百分比
    public float Destory;//破坏度百分比
    public int Mushroom ;//蘑菇存量
    public float ProductiveValue;
    public float Cooperation;

    public UshiroyamaData(float froest = 1, float destory = 0, int mushroom = 100 , float productiveValue = 0 ,float cooperation = 1f)
    {
        Froest = froest;
        Destory = destory;
        Mushroom = mushroom;
        ProductiveValue = productiveValue;
        Cooperation = cooperation;
    }

}
  
public class ProduceSaving : MonoBehaviour
{
    public static ProduceSaving Instance { get; private set; }

    [Header("农业生产信息")]
    public AgricultureData agricultureData;
    [Header("渔业生产信息")]
    public FisheryData fishingData;
    [Header("畜牧业生产信息")]
    public HerdingData herdingData;
    [Header("后山生产信息")]
    public UshiroyamaData ushiroyamaData;



    //耦合度依托啊(恼!
    private void UshDestoryChange()//用于更新破坏度,计算破坏度变化
    {
        var ush = ushiroyamaData;
        if (ush.Destory >= 1) return;//破坏不可逆

        ush.Destory += ((1 - ush.Froest) * 0.25f);
        //破坏度的数值规范
        if (ush.Destory < 0) ush.Destory = 0;
        else if (ush.Destory > 1)
        {
            ush.Destory = 100;
            DataEvents.Instance.SetDestoryForetry();
        }
    }
    public void UshCooperationSet(int choice)
    {if (choice == 1)
        {
            ushiroyamaData.Cooperation = 1.1f;
            DataEvents.Instance.Refuse("Foretry");
        }
        else
        {
            ushiroyamaData.Cooperation = 1.5f;
            DataEvents.Instance.Receive("Foretry");
        }
    }
    public void Grow()
    {
        var ush = ushiroyamaData;
        var income = 30 * (100 - ush.Mushroom) + ush.Froest * 1000 * 20;
        DataEvents.Instance.Income(income);
        ush.ProductiveValue += income;
        UshDestoryChange();
        AgricultureProduce.Instance.Grow();
        FisheryProduce.Instance.Grow(); 
        if (ush.Cooperation < 1.1 && ush.ProductiveValue >= 8000)
        {
            CooperationSet.Instance.SetTeget(3);
        }
    }







    private void Awake()
    {
        if (Instance == null && Instance != this) { 
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    internal void SetAgricultureData(AgricultureData agricultureData)
    {
        this.agricultureData = agricultureData;
    }
    internal void SetFisheryData(FisheryData fishingData) 
    { 
        this.fishingData = fishingData;
    }
    internal void SetHerdingData(HerdingData herdingData) 
    { 
        this.herdingData = herdingData;
    }
    internal void SetUshiroyamaData(UshiroyamaData ushiroyamaData) 
    { 
        this.ushiroyamaData = ushiroyamaData;
    }








    public void DataIni()
    {
            agricultureData = new AgricultureData();
            fishingData = new FisheryData();
            herdingData = new HerdingData();
            ushiroyamaData = new UshiroyamaData();
    }
    public AgricultureData GetAgricultureData()
    {
        return agricultureData;
    }
    public FisheryData GetFisheryData()
    {
        return fishingData;
    }
    public HerdingData GetHerdingData()
    {
        return herdingData;
    }
    public UshiroyamaData GetUshiroyamaData()
    {
        return ushiroyamaData;
    }
    public void DataLoading(AgricultureData a , FisheryData f, HerdingData h , UshiroyamaData u)
    {
        agricultureData = a;
        fishingData = f;
        herdingData = h;
        ushiroyamaData = u;
    }
}
