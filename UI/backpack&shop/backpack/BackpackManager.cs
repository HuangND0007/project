using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;



[System.Serializable]
public class BackpackItems//用于保存背包物品数量的数据结构
{
    public List<int> numbers;
    public List<int> itemID;


    public BackpackItems()//默认构造函数，初始化一个空的列表
    {
        numbers = new List<int>();
        itemID = new List<int>();
    }
    public BackpackItems(List<int> numbers , List<int> ID)//带参数的构造函数，接受一个整数列表作为参数,用于初始化背包物品数量的数据结构
    {
        this.numbers = numbers;
        this.itemID = ID;
    }



}
public class BackpackManager : MonoBehaviour
{
    public static BackpackManager Instance { get; private set; }


    //public BackpackSlots[] itemSlots;//旧逻辑使用固定数组,现已修改
    public List<BackpackSlots> itemSlots;//背包槽位列表,用于动态生成背包槽位
    public GameObject MoneyAmount;//显示金钱数量的UI文本对象

    public BackpackSlots backpackSlotPrefab;//背包槽位预制体,用于动态生成背包槽位
    public GameObject slot_;//背包槽位的父物体,用于动态生成背包槽位


    public List<int> Numbers;//用于保存背包物品数量的列表,与itemSlots一一对应
    public List<int> ID;//用于保存背包物品ID的列表,与itemSlots一一对应

    private void MoneyAmounts()
    {
        MoneyAmount.GetComponent<TMP_Text>().text = PlayerManager.Instance.GetMoney().ToString();
    }


    public int UpdateItems(ItemSO itemSO, int amount)
    {
        foreach (var slot in itemSlots)//暂时不设置物品上限,后续考虑
        {
            if (slot.itemSO == itemSO)
            {
                if ( amount < 0 && Mathf.Abs(amount) > slot.amount ) return -1;//如果要减少的数量大于当前物品数量,则返回-1,表示操作失败 
                slot.amount += amount;
                slot.UpdateUI();
                MoneyAmounts();
                return 0;
            }
        }

        BackpackSlots newSlot = Instantiate(backpackSlotPrefab); 
        newSlot.transform.SetParent(slot_.transform, false);
        newSlot.itemSO = itemSO;
        newSlot.amount = amount;
        newSlot.UpdateUI();
        itemSlots.Add(newSlot);
        Numbers.Add(amount);
        ID.Add(itemSO.ID);
        return 0;

    }









    private void AmountUpdate()
    {
        MoneyAmounts();
    }

    void Start()
    {
        foreach (var slot in itemSlots)
        {
            slot.UpdateUI();
        }
       AmountUpdate();
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }






    public void DataIni()
    {
        DataLoading(new BackpackItems());
    }
    public BackpackItems DataSaving()
    {
        List<int> numbers = new List<int>(itemSlots.Count);//创建一个新的列表来存储物品数量
        List<int> ID = new List<int>(itemSlots.Count);//创建一个新的列表来存储物品ID
        foreach (var slot in itemSlots)
        {
            if (slot.itemSO == null || slot.amount == 0) continue;//如果物品槽位没有物品,就跳过
            numbers.Add(slot.amount);
            ID.Add(slot.itemSO != null ? slot.itemSO.ID : -1);//如果物品槽位有物品,就保存物品ID,否则保存-1
        }
        return new BackpackItems(numbers , ID);
    }
    public void DataLoading(BackpackItems data)
    {
        int count = Mathf.Min(data.numbers.Count, data.itemID.Count);//获取物品数量和物品ID列表中较小的长度,避免越界
       
        Numbers  = data.numbers;
        ID       = data.itemID;
        if (count <= 0) return;//如果没有物品数据,就直接返回
        for (int i = 0; i < count; i++)
       {
            Debug.Log($"Loading item with ID: {data.itemID[i]} and amount: {data.numbers[i]}");
            if (Numbers[i] <= 0) continue;
            ItemSO itemSO = ItemDatabase.Instance.GetItemByID(data.itemID[i]);//根据物品ID获取物品数据

            UpdateItems(itemSO, Numbers[i]);
        }

    }
}
