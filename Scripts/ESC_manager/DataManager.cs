using UnityEngine;
using System.IO;
using System.Text;
using LitJson;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }
    // ========== 通用工具方法 ==========

    /// <summary>LitJson 没有美化选项，这里手动格式化缩进</summary>
    private static string PrettifyJson(string json)
    {
        StringBuilder sb = new StringBuilder();
        int indent = 0;
        bool inString = false;
        bool escape = false;

        foreach (char c in json)
        {
            if (escape)
            {
                sb.Append(c);
                escape = false;
                continue;
            }
            if (c == '\\')
            {
                sb.Append(c);
                escape = true;
                continue;
            }
            if (c == '"')
            {
                inString = !inString;
                sb.Append(c);
                continue;
            }
            if (inString)
            {
                sb.Append(c);
                continue;
            }

            switch (c)
            {
                case '{':
                case '[':
                    sb.Append(c);
                    sb.AppendLine();
                    sb.Append(' ', ++indent * 2);
                    break;
                case '}':
                case ']':
                    sb.AppendLine();
                    sb.Append(' ', --indent * 2);
                    sb.Append(c);
                    break;
                case ',':
                    sb.Append(c);
                    sb.AppendLine();
                    sb.Append(' ', indent * 2);
                    break;
                case ':':
                    sb.Append(": ");
                    break;
                default:
                    sb.Append(c);
                    break;
            }
        }
        return sb.ToString();
    }

    /// <summary>统一写入 JSON 文件（带缩进美化）</summary>
    private static void SaveJson(string fileName, object data)
    {
        string json = PrettifyJson(JsonMapper.ToJson(data));
        string path = Path.Combine(Application.streamingAssetsPath, fileName);
        File.WriteAllText(path, json);
    }

    /// <summary>统一读取 JSON 文件，不存在返回 null</summary>
    private static string LoadJson(string fileName)
    {
        string path = Path.Combine(Application.streamingAssetsPath, fileName);
        if (!File.Exists(path)) return null;
        return File.ReadAllText(path);
    }

    // ========== 存档 ==========

    private void EnvironmentSaving()
    {
        SaveJson("environmentdata.json", Environment.Instance.DataSaving());
        Debug.Log("环境数据保存成功");
    }

    private void PlayerSaving()
    {
        SaveJson("playerdata.json", PlayerManager.Instance.DataSaving());
        Debug.Log("玩家数据保存成功");
    }

    private void BackpackSaving()
    {
        SaveJson("backpackdata.json", BackpackManager.Instance.DataSaving());
    }

    private void EventsDataSaving()
    {
        SaveJson("eventsdata.json", DataEvents.Instance.DataSaving());
    }

    private void ProduceDataSaving()
    {
        var data = new
        {
            Agriculturedata = ProduceSaving.Instance.GetAgricultureData(),
            Fishingdata = ProduceSaving.Instance.GetFisheryData(),
            Herdingdata = ProduceSaving.Instance.GetHerdingData(),
            UshiroyamaData = ProduceSaving.Instance.GetUshiroyamaData()
        };
        SaveJson("producedata.json", data);
    }

    // ========== 读档 ==========

    private void PlayerLoading()
    {
        string json = LoadJson("playerdata.json");
        if (json != null)
        {
            PlayerManager.Instance.DataLoading(JsonMapper.ToObject<PlayerData>(json));
            Debug.Log("玩家数据加载成功");
        }
        else
        {
            PlayerManager.Instance.DataIni();
            Debug.Log("玩家数据初始化完成");
        }
    }

    private void EnvironmentLoading()
    {
        string json = LoadJson("environmentdata.json");
        if (json != null)
        {
            Environment.Instance.DataLoading(JsonMapper.ToObject<EnvironmentData>(json));
            Debug.Log("环境数据加载成功");
        }
        else
        {
            Environment.Instance.DataIni();
            Debug.Log("环境数据初始化完成");
        }
    }

    private void BackpackLoading()
    {
        string json = LoadJson("backpackdata.json");
        if (json != null)
        {
            BackpackManager.Instance.DataLoading(JsonMapper.ToObject<BackpackItems>(json));
            Debug.Log("背包数据加载成功");
        }
        else
        {
            BackpackManager.Instance.DataIni();
            Debug.Log("背包数据初始化完成");
        }
    }

    private void EventsDataLoading()
    {
        string json = LoadJson("eventsdata.json");
        if (json != null)
        {
            DataEvents.Instance.DataLoading(JsonMapper.ToObject<eventsdata>(json));
            Debug.Log("事件数据加载成功");
        }
        else
        {
            DataEvents.Instance.DataIni();
            Debug.Log("事件数据初始化完成");
        }
    }

    private void ProduceDataLoading()
    {
        string json = LoadJson("producedata.json");
        if (json != null)
        {
            JsonData wrapper = JsonMapper.ToObject(json);
            var agri = JsonMapper.ToObject<AgricultureData>(wrapper["Agriculturedata"].ToJson());
            var fish = JsonMapper.ToObject<FisheryData>(wrapper["Fishingdata"].ToJson());
            var herd = JsonMapper.ToObject<HerdingData>(wrapper["Herdingdata"].ToJson());
            var ushiro = JsonMapper.ToObject<UshiroyamaData>(wrapper["UshiroyamaData"].ToJson());
            ProduceSaving.Instance.DataLoading(agri, fish, herd , ushiro);
            Debug.Log("生产数据加载成功");
        }
        else
        {
            ProduceSaving.Instance.DataIni();
            Debug.Log("生产数据初始化完成");
        }
    }

    //========== 新游戏重置信息 ==========

    public void AllInitial()
    {
        PlayerManager.Instance.DataIni();
        Environment.Instance.DataIni();
        BackpackManager.Instance.DataIni();
        DataEvents.Instance.DataIni();
        ProduceSaving.Instance.DataIni();

    }

    // ========== 入口 ==========

    private void DataSaving()
    {
        PlayerSaving();
        EnvironmentSaving();
        BackpackSaving();
        EventsDataSaving();
        ProduceDataSaving();
    }

    public void DataLoading()
    {
        PlayerLoading();
        EnvironmentLoading();
        BackpackLoading();
        EventsDataLoading();
        ProduceDataLoading();
    }

    public void ButtonOnClick()
    {
        Debug.Log("点击了退出按钮");
        DataSaving();
        StartCoroutine(SceneLoader.Instance.SwitchLevelAsync("MainScene", "ExitPotint")); 
        SceneLoader.Instance.reIsTrans();
    }
   

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        DataLoading();
    }
}