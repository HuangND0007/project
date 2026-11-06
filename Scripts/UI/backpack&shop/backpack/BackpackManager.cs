using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class BackpackItems
{
    public List<int> numbers;
    public List<int> itemID;

    // 默认构造函数，初始化背包物品数据
    public BackpackItems()
    {
        numbers = new List<int>() { 20, 20 };
        itemID = new List<int>() { 1001, 1002 };
    }

    public BackpackItems(List<int> numbers, List<int> ID)
    {
        this.numbers = numbers;
        itemID = ID;
    }
}

public class BackpackManager : MonoBehaviour
{
    public static BackpackManager Instance { get; private set; }

    // 背包槽位列表，用于动态生成背包槽位
    public List<BackpackSlots> itemSlots;

    // 显示金钱数量的 UI 文本对象
    public GameObject MoneyAmount;

    public TMP_Text Name;

    // 背包槽位预制体
    public BackpackSlots backpackSlotPrefab;

    // 背包槽位的父物体
    public GameObject slot_;

    // 与 itemSlots 一一对应的物品数量和 ID 列表
    public List<int> Numbers;
    public List<int> ID;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        foreach (var slot in itemSlots)
        {
            if (slot != null)
                slot.UpdateUI();
        }

        AmountUpdate();
    }

    private void MoneyAmounts()
    {
        MoneyAmount.GetComponent<TMP_Text>().text =
            PlayerManager.Instance.GetMoney().ToString();
    }

    public int UpdateItems(ItemSO itemSO, int amount)
    {
        foreach (var slot in itemSlots)
        {
            if (slot.itemSO == itemSO)
            {
                if (amount < 0 && Mathf.Abs(amount) > slot.amount)
                {
                    // 要减少的数量大于当前数量，操作失败
                    return -1;
                }

                slot.amount += amount;

                // 数量为零时，UpdateUI 会移除并销毁该格子
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
        MoneyAmounts();

        itemSlots.Add(newSlot);
        Numbers.Add(amount);
        ID.Add(itemSO.ID);

        return 0;
    }

    public int SetItems(ItemSO itemSO, int amount)
    {
        foreach (var slot in itemSlots)
        {
            if (slot.itemSO == itemSO)
            {
                if (amount < 0 && Mathf.Abs(amount) > slot.amount)
                {
                    // 要减少的数量大于当前数量，操作失败
                    return -1;
                }

                slot.amount += amount;

                // 数量为零时，UpdateUI 会移除并销毁该格子
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

    /// <summary>
    /// 从背包列表中移除指定格子及其对应数据。
    /// 格子的销毁由 BackpackSlots.UpdateUI 执行。
    /// </summary>
    public void RemoveSlot(BackpackSlots slot)
    {
        int index = itemSlots.IndexOf(slot);

        if (index < 0)
            return;

        itemSlots.RemoveAt(index);

        if (index < Numbers.Count)
            Numbers.RemoveAt(index);

        if (index < ID.Count)
            ID.RemoveAt(index);
    }

    /// 清空所有背包物品，并更新界面。
    public void ClearBackpack()
    {
        for (int i = itemSlots.Count - 1; i >= 0; i--)
        {
            if (itemSlots[i] != null)
                Destroy(itemSlots[i].gameObject);
        }

        itemSlots.Clear();
        Numbers.Clear();
        ID.Clear();

        AmountUpdate();
        NameUpdate();
    }

    public void AmountUpdate()
    {
        MoneyAmounts();
    }

    public void NameUpdate()
    {
        UpdateName();
    }

    private void UpdateName()
    {
        Name.GetComponent<TMP_Text>().text =
            PlayerManager.Instance.GetName();
    }

    public void DataIni()
    {
        DataNewing(new BackpackItems());
    }

    public BackpackItems DataSaving()
    {
        List<int> numbers = new List<int>(itemSlots.Count);
        List<int> itemIDs = new List<int>(itemSlots.Count);

        foreach (var slot in itemSlots)
        {
            if (slot == null || slot.itemSO == null || slot.amount <= 0)
                continue;

            numbers.Add(slot.amount);
            itemIDs.Add(slot.itemSO.ID);
        }

        return new BackpackItems(numbers, itemIDs);
    }

    public void DataLoading(BackpackItems data)
    {
        int count = Mathf.Min(
            data.numbers.Count,
            data.itemID.Count
        );

        Numbers = data.numbers;
        ID = data.itemID;

        Name.GetComponent<TMP_Text>().text =
            PlayerManager.Instance.GetName();

        if (count <= 0)
            return;

        for (int i = 0; i < count; i++)
        {
            Debug.Log(
                $"Loading item with ID: {data.itemID[i]} " +
                $"and amount: {data.numbers[i]}"
            );

            if (Numbers[i] <= 0)
                continue;

            ItemSO itemSO =
                ItemDatabase.Instance.GetItemByID(data.itemID[i]);

            UpdateItems(itemSO, Numbers[i]);
        }
    }

    public void DataNewing(BackpackItems data)
    {
        ClearBackpack();

        int count = Mathf.Min(
            data.numbers.Count,
            data.itemID.Count
        );

        Numbers = data.numbers;
        ID = data.itemID;

        Name.GetComponent<TMP_Text>().text =
            PlayerManager.Instance.GetName();

        if (count <= 0)
            return;

        for (int i = 0; i < count; i++)
        {
            Debug.Log(
                $"Loading item with ID: {data.itemID[i]} " +
                $"and amount: {data.numbers[i]}"
            );

            if (Numbers[i] <= 0)
                continue;

            ItemSO itemSO =
                ItemDatabase.Instance.GetItemByID(data.itemID[i]);

            SetItems(itemSO, Numbers[i]);
        }
    }
}