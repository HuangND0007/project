using TMPro;
using UnityEngine;

public class Field : MonoBehaviour


{
    private void UpdateUI() => TimeUImanage.Instance.UpdateUI();

    public ItemSO cropItem;
    public int output = 1;       // 每次收获的产物数量

    private bool isPlanted = false;   // 是否已播种
    private int product = 0;          // 待收集的产物数量

    private string lastSeason = "";   // 用于检测季节变化

    private void Start()
    {
        // 记录初始季节
       // Debug.Log("100");
        if (Environment.Instance != null)
        {
            //Debug.Log("不为空");
             lastSeason = Environment.Instance.getSeason();
        }
           
    }

  
    /// 按钮绑定的播种方法

    public void Plant()
    {
        // 只能在春天播种，且不能重复播种
        if (isPlanted)
        {
            Debug.Log("已经播种过了");
            return;
        }

        string currentSeason = Environment.Instance.getSeason();
        if (currentSeason != "春")
        {
            Debug.Log($"只能在春天播种，当前是{currentSeason}");
            return;
        }

        isPlanted = true;
        Debug.Log("播种成功，等待秋天成熟");
        TransSet(1f);
        //Harvest();//测试
    }



    //检查成熟条件
   
    private void Update()
    {
         
       

    
        if (!isPlanted) return;               // 未播种不检查
        if (Environment.Instance == null) return;

        string currentSeason = Environment.Instance.getSeason();

        // 季节发生变化
        if (currentSeason != lastSeason && currentSeason == "秋")
        {
            Harvest();
        }

        lastSeason = currentSeason;
    }

    private void Harvest()
    {
        product += output;
        isPlanted = false;
        Debug.Log($"当前产物数量：{product}");
        
    }

   
    // 玩家取走产物
    //public int Collect()
    //{
    //    int collected = product;
    //    product = 0;
    //   // return collected;
    //}


    public void CollectButtonClick()
    {
        if (product <= 0)
        {
            Debug.Log("没有可收取的产物");
            return;
        }
        if (cropItem == null)
        {
            Debug.LogError("农田未设置产物 ItemSO！");
            return;
        }

        // 调用背包管理器添加物品
        int result = BackpackManager.Instance.UpdateItems(cropItem, product);
        if (result == 0)
        {
            Debug.Log($"成功将 {product} 个 {cropItem.itemName} 放入背包");
            product = 0;
        }
        else
        {
            Debug.LogWarning("背包添加失败");
        }
    }


    public int getproduct()
    {
        return product;
    }



    private void TransSet(float time)//统一化设置场景和时间的方法，方便调用
    {
        Environment.Instance.SetTime(time);

        //ModifyPosition(scene, x, y, z ,preScene);
        UpdateUI();
    }
}


