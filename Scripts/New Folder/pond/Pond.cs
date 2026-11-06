using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pond : MonoBehaviour
{
    private void UpdateUI() => TimeUImanage.Instance.UpdateUI();

    public ItemSO cropItem;
    public int output = 1;       // 每次收获的产物数量

    private bool isPlanted = false;   // 是否已播种
    private int product = 0;          // 待收集的产物数量

    public int waitTime = 2;//收获时间

    public int currentTime = 0;//开始时间

    public bool continuousProduction = false;//是否持续产出
    [SerializeField] private int plantDay = 0;//记录时间

    public void Plant()
    {
        // 只能在春天播种，且不能重复播种
        if (isPlanted)
        {
            Debug.Log("已经播种过了");
            return;
        }

        //currentTime = Environment.Instance.getDay();


        plantDay = Environment.Instance.getDay();

        isPlanted = true;
        Debug.Log("播种成功，等待{plantDay + waitTime}天");
        TransSet(1f);
        //Harvest();//测试
    }

    // Update is called once per frame
    void Update()
    {

        if (!isPlanted || Environment.Instance == null) return;

        int currentDay = Environment.Instance.getDay();
        int daysPassed = currentDay - plantDay;

        if (daysPassed >= waitTime)
        {
            OnMature(currentDay, daysPassed);
        }

        //if (!isPlanted) return;
        //if (Environment.Instance == null) return;

        //int newTime = Environment.Instance.getDay();

        //int chTime = (newTime - currentTime) / waitTime;
        //Debug.Log("new-=" + (newTime-currentTime));
        //Debug.Log("chTime=" + chTime);

        //if (chTime!=0)
        //{

        //    Harvest();
        //}

        //if (newTime>currentTime)
        //{
        //    Harvest();
        //}

    }


    private void OnMature(int currentDay, int daysPassed)
    {
        if (continuousProduction)
        {
            // 持续模式：每 waitTime 天自动产出
            int cycles = daysPassed / waitTime;         // 计算已经过的完整周期数
            product += output * cycles;                 // 累积产物
            plantDay += cycles * waitTime;              // 前进计数，避免重复触发
            Debug.Log($"持续产出 {cycles} 次，当前产物总数：{product}");
        }
        else
        {
            // 一次性模式：只收一次
            product += output;
            isPlanted = false;
            Debug.Log($"成熟！产物数量：{product}，请收集后重新播种");
        }
    }


    //private void Harvest()
    //{
    //    product += output;
    //    //isPlanted = false;
    //    Debug.Log($"当前产物数量：{product}");

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
            Debug.LogError("未设置产物 ItemSO！");
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

    public void setwaitTime(int inTime)//改变收获时间
    {
        waitTime = inTime;
    }


    private void TransSet(float time)//统一化设置场景和时间的方法，方便调用
    {
        Environment.Instance.SetTime(time);

        //ModifyPosition(scene, x, y, z ,preScene);
        UpdateUI();
    }
}
