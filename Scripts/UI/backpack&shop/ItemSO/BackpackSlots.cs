using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BackpackSlots : MonoBehaviour
{
    public ItemSO itemSO;
    public int amount;

    public Image itemImage;
    public TMP_Text Amount;

    public void UpdateUI()
    {
        // 物品数量为零时，直接销毁整个格子
        if (amount <= 0)
        {
            // 先从背包管理器的列表中移除，避免留下无效引用
            if (BackpackManager.Instance != null)
            {
                BackpackManager.Instance.RemoveSlot(this);
            }

            Destroy(gameObject);
            return;
        }

        if (itemSO != null)
        {
            itemImage.sprite = itemSO.Icon;
            itemImage.gameObject.SetActive(true);
            Amount.text = amount.ToString();
        }
        else
        {
            itemImage.gameObject.SetActive(false);
            Amount.text = "";
        }
    }
}