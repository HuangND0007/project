using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellShopManager : MonoBehaviour
{
    [Header("售卖界面")]
    [Tooltip("第二个画布中的售卖按钮父节点，建议配合 VerticalLayoutGroup 或 GridLayoutGroup 使用。")]
    [SerializeField] private Transform sellButtonContainer;

    [Tooltip("售卖按钮预制体，根节点需要有 SellItemButtonUI、Button 组件。")]
    [SerializeField] private SellItemButtonUI sellButtonPrefab;

    [Header("售卖设置")]
    [Tooltip("每次点击售卖多少个物品；设为 1 表示一个一个出售。")]
    [Min(1)]
    [SerializeField] private int sellCountPerClick = 1;

    private readonly List<SellItemButtonUI> generatedButtons = new List<SellItemButtonUI>();
    private Coroutine refreshCoroutine;

    private void Start()
    {
        // 等待 BackpackManager.Start 完成初始 UI 更新后再生成按钮。
        RequestRefresh();
    }

    /// <summary>
    /// 根据当前背包内容重新生成全部售卖按钮。
    /// 背包数据加载完成后也可以主动调用该方法。
    /// </summary>
    public void RefreshSellButtons()
    {
        ClearSellButtons();

        if (sellButtonContainer == null)
        {
            Debug.LogWarning("SellShopManager：尚未指定 sellButtonContainer。");
            return;
        }

        if (sellButtonPrefab == null)
        {
            Debug.LogWarning("SellShopManager：尚未指定 sellButtonPrefab。");
            return;
        }

        BackpackManager backpack = BackpackManager.Instance;
        if (backpack == null || backpack.itemSlots == null)
        {
            Debug.LogWarning("SellShopManager：BackpackManager.Instance 或 itemSlots 为空。");
            return;
        }

        foreach (BackpackSlots slot in backpack.itemSlots)
        {
            if (slot == null || slot.itemSO == null || slot.amount <= 0)
            {
                continue;
            }

            SellItemButtonUI button = Instantiate(
                sellButtonPrefab,
                sellButtonContainer,
                false
            );

            button.Setup(
                slot.itemSO,
                slot.amount,
                sellCountPerClick,
                RefreshSellButtons
            );

            generatedButtons.Add(button);
        }

        // 如果父节点使用 ContentSizeFitter，生成后重新计算一次布局。
        if (sellButtonContainer is RectTransform containerRect)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(containerRect);
        }
    }

    /// <summary>
    /// 清除已经生成的售卖按钮。
    /// </summary>
    public void ClearSellButtons()
    {
        foreach (SellItemButtonUI button in generatedButtons)
        {
            if (button != null)
            {
                Destroy(button.gameObject);
            }
        }

        generatedButtons.Clear();
    }

    /// <summary>
    /// 延迟到当前帧结束后刷新，避免与 Start 的执行顺序冲突。
    /// </summary>
    public void RequestRefresh()
    {
        if (refreshCoroutine != null)
        {
            StopCoroutine(refreshCoroutine);
        }

        refreshCoroutine = StartCoroutine(RefreshNextFrame());
    }

    private IEnumerator RefreshNextFrame()
    {
        yield return null;
        RefreshSellButtons();
        refreshCoroutine = null;
    }
}
