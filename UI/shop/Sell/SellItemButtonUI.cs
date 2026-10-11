using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 单个背包物品的售卖按钮。
/// 将该脚本挂在售卖按钮预制体根节点上。
/// </summary>
public class SellItemButtonUI : MonoBehaviour, IPointerClickHandler
{
    [Header("按钮界面引用")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private TMP_Text amountText;
    [SerializeField] private Button sellButton;

    private ItemSO item;
    private int sellAmount = 1;
    private Action onSold;

    private void Awake()
    {
        if (sellButton == null)
        {
            sellButton = GetComponent<Button>();
        }
    }

    /// <summary>
    /// 初始化售卖按钮。
    /// </summary>
    /// <param name="itemSO">要售卖的物品数据</param>
    /// <param name="currentAmount">背包中的剩余数量</param>
    /// <param name="amountPerClick">每次点击售卖的数量</param>
    /// <param name="soldCallback">售卖成功后执行的回调，通常用于刷新商品列表</param>
    public void Setup(
        ItemSO itemSO,
        int currentAmount,
        int amountPerClick,
        Action soldCallback)
    {
        item = itemSO;
        sellAmount = Mathf.Max(1, amountPerClick);
        onSold = soldCallback;

        if (item != null)
        {
            gameObject.name = $"SellButton_{item.name}";

            if (itemIcon != null)
            {
                itemIcon.sprite = item.Icon;
                itemIcon.enabled = item.Icon != null;
            }
        }

        UpdateDisplay(currentAmount);
    }

    /// <summary>
    /// 更新售价和剩余数量显示。
    /// </summary>
    public void UpdateDisplay(int currentAmount)
    {
        if (priceText != null && item != null)
        {
            int totalPrice = item.sellPrice * sellAmount;
            priceText.text = $"售价：{totalPrice}";
        }

        if (amountText != null)
        {
            amountText.text = $"数量：{currentAmount}";
        }
    }

    /// <summary>
    /// 不依赖 Button.onClick，直接响应 EventSystem 的点击事件。
    /// 因为调试已经确认 PointerClick 能到达该物体。
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        SellItem();
    }

    /// <summary>
    /// 点击按钮后售卖物品。
    /// </summary>
    private void SellItem()
    {
        // 临时排查日志：确认 SellItem 是否被调用。
        Debug.Log($"SellItem 被调用：{(item == null ? "空物品" : item.name)}", this);

        if (item == null || BackpackManager.Instance == null)
        {
            return;
        }

        // BackpackManager.UpdateItems 使用负数表示从背包中扣除物品。
        int result = BackpackManager.Instance.UpdateItems(item, -sellAmount);

        if (result == 0)
        {
            // 售卖成功后重建按钮列表，同步显示最新数量。
            onSold?.Invoke();
            PlayerManager.Instance.ModifyMoney(item.sellPrice * sellAmount);
            BackpackManager.Instance.AmountUpdate();
        }
        else if (priceText != null)
        {
            // result 为 -1 表示背包中的物品不足。
            priceText.text = "物品数量不足";
        }
    }

    private void OnDestroy()
    {
        onSold = null;
        item = null;
    }
}