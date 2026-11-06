using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 全局唯一的对话管理器：持有所有 对话/选项/商店 UI 引用。
/// 按钮监听只在这里注册一次，多个 NPC 共用同一套 UI，互不干扰。
/// 场景里只挂一个（建议挂在专门的空物体或 Canvas 上）。
/// </summary>
public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("主对话UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text contentText;
    [SerializeField] private Button openOptionsButton;  // "查看选项"按钮
    [SerializeField] private Button openShopButton;     // "查看商店"按钮
    [SerializeField] private Button guildButton;        // 向导按钮

    [Header("选项弹窗")]
    [SerializeField] private GameObject optionsWindow;
    [SerializeField] private Transform optionsContainer;
    [SerializeField] private GameObject optionButtonPrefab;
    [SerializeField] private Button closeOptionsButton;
    [SerializeField] private TMP_Text resultText;

    [Header("商店弹窗")]
    [SerializeField] private CanvasGroup shopWindow;

    private DialogueData currentData;        // 当前正在对话的 NPC 的数据
    private int currentLine = 0;
    private bool isDialogueActive = false;
    private bool isOptionsWindowActive = false;

    public bool IfDialogueActive() => isDialogueActive;
    /// <summary>是否正忙（对话或选项窗打开中），忙时不接受新对话</summary>
    public bool IsBusy => isDialogueActive || isOptionsWindowActive;
    /// <summary>当前是否正在和持有该数据的 NPC 对话</summary>
    public bool IsTalkingTo(DialogueData data) => isDialogueActive && currentData == data;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);  // 场景里只允许一个
            return;
        }
        Instance = this;
    }

    void Start()
    {
        // 监听只注册这一次，修复多 NPC 重复 AddListener 互相清空选项的问题
        openOptionsButton.onClick.AddListener(OpenOptionsWindow);
        openShopButton.onClick.AddListener(OpenShopWindow);
        guildButton.onClick.AddListener(OnGuildClicked);
        closeOptionsButton.onClick.AddListener(CloseOptionsWindow);
        CloseShopAndReset();
    }

    // ========== 对话流程 ==========
    public void StartDialogue(DialogueData data)
    {
        if (data == null || IsBusy) return;
        currentData = data;
        currentLine = 0;
        isDialogueActive = true;
        LockPlayer(true);
        dialoguePanel.SetActive(true);
        ShowLine();
    }

    public void NextLine()
    {
        if (!isDialogueActive) return;
        currentLine++;
        ShowLine();
    }

    void ShowLine()
    {
        if (currentData == null) return;
        if (currentLine >= currentData.lines.Length)
        {
            EndDialogue();
            return;
        }
        var line = currentData.lines[currentLine];
        nameText.text = line.speakerName;
        contentText.text = line.content;
        openOptionsButton.gameObject.SetActive(line.hasOptions);
        openShopButton.gameObject.SetActive(line.shop);
        guildButton.gameObject.SetActive(line.Guild);
    }

    // ========== 选项弹窗 ==========
    void OpenOptionsWindow()
    {
        if (currentData == null || currentLine >= currentData.lines.Length) return;
        var currentOptions = currentData.lines[currentLine].options;
        if (currentOptions == null || currentOptions.Length == 0) return;

        dialoguePanel.SetActive(false);     // 隐藏主界面
        isOptionsWindowActive = true;
        isDialogueActive = false;           // 选项期间不推进对话

        ClearOptions();
        resultText.text = "您有什么疑问?";

        foreach (var option in currentOptions)
        {
            GameObject btn = Instantiate(optionButtonPrefab, optionsContainer);
            var label = btn.GetComponentInChildren<TMP_Text>();
            if (label != null && option.optionText != null)
                label.text = option.optionText;
            var opt = option;  // 闭包捕获
            btn.GetComponent<Button>().onClick.AddListener(() => OnOptionSelected(opt));
        }
        optionsWindow.SetActive(true);
    }

    void OnOptionSelected(DialogueData.DialogueOption option)
    {
        optionsContainer.gameObject.SetActive(false);  
        resultText.text = option.resultText;
        ClearOptions();  // 修复：只清空子物体，不再 SetActive(false) 容器本身
    }

    public void CloseOptionsWindow()
    {
        optionsWindow.SetActive(false);
        isOptionsWindowActive = false;   // 修复：复位标志，否则之后按 F 会走错分支
        EndDialogue();                   // 统一收尾：复位状态、解锁玩家
    }

    // ========== 商店弹窗 ==========
    void OpenShopWindow()
    {
        // 先复位对话状态（修复：原 closeDialogue(false) 故意不复位，导致状态机错乱）
        isDialogueActive = false;
        isOptionsWindowActive = false;
        currentData = null;
        dialoguePanel.SetActive(false);
        optionsWindow.SetActive(false);
        ClearOptions();
        resultText.text = "";

        // 只在背包未打开时打开，避免重复 toggle 造成奇偶错乱
        if (BackPackUI.Instance != null && !BackPackUI.Instance.IsOpen())
            BackPackUI.Instance.BackPackOpenandClose();

        if (shopWindow != null)
        {
            shopWindow.alpha = 1;
            shopWindow.interactable = true;
            shopWindow.blocksRaycasts = true;
        }
    }

    void OnGuildClicked()
    {
        // 原逻辑在此处生成了一个无后续处理的孤立按钮，已移除；向导功能按需在此实现
        EndDialogue();
    }

    // ========== 收尾 ==========
    public void EndDialogue()
    {
        isDialogueActive = false;
        isOptionsWindowActive = false;
        currentData = null;
        currentLine = 0;
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        if (optionsWindow != null) optionsWindow.SetActive(false);
        if (optionsContainer != null) optionsContainer.gameObject.SetActive(true);
        CloseShopAndReset();
        LockPlayer(false);
    }

    /// <summary>统一关闭商店并复位 CanvasGroup，保证不会残留挡点击的透明面板</summary>
    void CloseShopAndReset()
    {
        if (shopWindow != null)
        {
            shopWindow.alpha = 0;
            shopWindow.interactable = false;
            shopWindow.blocksRaycasts = false;
        }
        if (BackPackUI.Instance != null && BackPackUI.Instance.IsOpen())
            BackPackUI.Instance.BackPackOpenandClose();
    }

    void ClearOptions()
    {
        foreach (Transform child in optionsContainer)
            if (child != null) Destroy(child.gameObject);
    }

    void LockPlayer(bool locked)
    {
        if (Player.Instance != null)
        {
            if (locked) Player.Instance.SetMoveFalse();
            else Player.Instance.SetMoveTrue();
        }
        if (Exit.Instance != null)
        {
            if (locked) Exit.Instance.TrueEscChoke();
            else Exit.Instance.FalseEscChoke();
        }
    }
}