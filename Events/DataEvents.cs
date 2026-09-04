using System.Collections;
using System.Collections.Generic;
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



    public static eventsdata Default => new eventsdata (0, 0, 0, 0, 0 ,false, 0);

    public eventsdata(int rural, int foretry, int herd , int fish, int count, bool isdestory_foretry, int destory_count)
    {
        Rural = rural;
        Foretry = foretry;
        Herd = herd;
        Fish = fish;
        this.isdestory_foretry = isdestory_foretry;
        this.destory_count = destory_count;
        this.Count = count;
    }

}


public class DataEvents : MonoBehaviour//记录选择节点的脚本，方便后续剧情的触发
{
    public static DataEvents Instance { get; private set; }

    private eventsdata data = new eventsdata();
    private bool isdestory_foretry = false;//林地是否被破坏
    private int destory_count = 0;//林地以外被破坏的数量

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



    /// 破坏状态的记录,用于后续剧情的触发
    public void SetDestoryForetry()
    {
        isdestory_foretry = true;
    }

    public void SetDestoryCount()
    {
        destory_count++;
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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





    public void DataIni()
    {
        data = eventsdata.Default;
        isdestory_foretry = false;
        destory_count = 0;
    }
    public eventsdata DataSaving()
    {
        return new eventsdata(data.Rural, data.Foretry, data.Herd, data.Fish, data.Count, isdestory_foretry, destory_count);
    }
    public void DataLoading(eventsdata d)
    {
        data = d;
        isdestory_foretry = d.isdestory_foretry;
        destory_count = d.destory_count;    
    }
}
