using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase Instance;

    public Dictionary<int, ItemSO> idToItem;

    private void LoadItems()
    {
        idToItem = new Dictionary<int, ItemSO>();
        List<ItemSO> items = new List<ItemSO>(Resources.LoadAll<ItemSO>("Items"));
        foreach (ItemSO item in items)
        {
            idToItem[item.ID] = item;
        }
    }

    public ItemSO GetItemByID(int id)
    {
        return idToItem.ContainsKey(id) ? idToItem[id] : null;
    }

    private void Start()
    {
        print("ItemDatabase loaded with " + idToItem.Count + " items.");

    }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadItems();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
