using UnityEngine;

[System.Serializable]
public struct eventsdata
{
    public int Rural;//分别记录农业、林业、牧业、渔业的选择节点,0表示未触发,正数接受,负数表示拒绝
    public int Foretry;
    public int Herd;
    public int Fish;

    public int Count;//记录当前选择接受的节点的数量

    public bool isdestory_foretry ;//林地是否被破坏
    public int destory_count ;//林地以外被破坏的数量

    public bool IsWent;
    public float Income;//记录当前的收入,用于后续剧情的触发

    public eventsdata(
        int rural = 0, int foretry = 0, int herd = 0, int fish = 0,
        int count = 0, bool isdestory_foretry = false, int destory_count = 0, bool isWent = false, 
        float income = 0f)
    {
        Rural = rural;
        Foretry = foretry;
        Herd = herd;
        Fish = fish;
        this.isdestory_foretry = isdestory_foretry;
        this.destory_count = destory_count;
        this.Count = count;
        IsWent = isWent;
        Income = income;
    }

}


public class DataEvents : MonoBehaviour//记录选择节点的脚本，方便后续剧情的触发
{
    public static DataEvents Instance { get; private set; }
    private static void ToEnd(int choice) => FinalControl.Instance.SetFinal(choice);

    private eventsdata data = new eventsdata();
    public bool isdestory_foretry = false;//林地是否被破坏
    public int destory_count = 0;//林地以外被破坏的数量


    public bool isIsWent()
    {
        return data.IsWent;
    }
    /// 合作商的接受与拒绝的记录,用于后续剧情的触发
    public void Receive(string type)
    {
        switch (type)
        {
            case "Rural":
                data.Rural++;
                data.Count++;
                break;
            case "Foretry":
                data.Foretry++;
                data.Count++;
                break;
            case "Herd":
                data.Herd++;
                data.Count++;
                break;
            case "Fish":
                data.Fish++;
                data.Count++;
                break;
            default:
                Debug.LogWarning("Unknown type: " + type);
                break;
        }
    }

    public void Refuse(string type)
    {
        switch (type)
        {
            case "Rural":
                data.Rural = -1;
                break;
            case "Foretry":
                data.Foretry = -1;
                break;
            case "Herd":
                data.Herd = -1;
                break;
            case "Fish":
                data.Fish = -1;
                break;
            default:
                Debug.LogWarning("Unknown type: " + type);
                break;
        }
    }

    public void Income(float income)
    {
        data.Income += income;
    }

    /// 破坏状态的记录,用于后续剧情的触发
    public void SetDestoryForetry()
    {
        isdestory_foretry = true;
    }

    public void IncreaceDestoryCount()//破坏不可逆,仅有加法方法
    {
        destory_count++;
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

    public bool DestorySettlement() 
    {
        if(isdestory_foretry)
        {
            //林地结算
            ToEnd(1);
            return true;
        }
        if(destory_count >= 2)
        {
            //林地以外结算
            ToEnd(2);
            return true;
        }
        return false;
    }
    private void Update()
    {
        //DestorySettlement();
    }

    //结算最终的剧情走向,根据选择节点的数量和收入来判断,游戏结束被调用
    public void FinalSettlement()
    {
        if(DestorySettlement())
        {
            return;
        }

        if (data.Count == 0) 
        {
            //1类结局
            ToEnd(3);
            return;
        }
        else if(data.Count <= 3 )
        {
             if(data.Income >= 20000)
             {
                //2类结局
                ToEnd(4);
             }
             else 
            {
                ToEnd(5);
            }
             return;
        }
        else if(data.Count == 4)
        {
            if(data.Income >= 20000)
             {
                //4类结局
                ToEnd(6);
             }
             else 
            {
                ToEnd(5);
            }
             return;
        }
    }

    public void DataIni()
    {
        data = new eventsdata();
        isdestory_foretry = false;
        destory_count = 0;
    }
    public eventsdata DataSaving()
    {
        return new eventsdata(
            data.Rural, data.Foretry, data.Herd, data.Fish, data.Count,
            isdestory_foretry, destory_count , data.IsWent, data.Income);
    }
    public void DataLoading(eventsdata d)
    {
        data = d;
        isdestory_foretry = d.isdestory_foretry;
        destory_count = d.destory_count;    
    }
}
