using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPCDialogue : MonoBehaviour
{
    [Header("主对话UI")]
    [SerializeField] private DialogueData dialogueData;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text contentText;
    [SerializeField] private Button openOptionsButton;  // "查看选项"按钮

    [Header("选项弹窗")]
    [SerializeField] private GameObject optionsWindow;
    [SerializeField] private Transform optionsContainer;
    [SerializeField] private GameObject optionButtonPrefab;
    [SerializeField] private Button closeOptionsButton;
    [SerializeField] private TMP_Text resultText;  // 弹窗内显示选择结果

    private int currentLine = 0;
    private bool isPlayerInRange = false;
    private bool isDialogueActive = false;

    void Start()
    {
        openOptionsButton.onClick.AddListener(OpenOptionsWindow);
        closeOptionsButton.onClick.AddListener(CloseOptionsWindow);
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            if (!isDialogueActive) StartDialogue();
            else NextLine();
        }
    }

    void StartDialogue()
    {
        isDialogueActive = true;
        currentLine = 0;
        dialoguePanel.SetActive(true);
        ShowLine();
    }

    void ShowLine()
    {
        if (currentLine >= dialogueData.lines.Length)
        {
            EndDialogue();
            return;
        }

        var line = dialogueData.lines[currentLine];
        nameText.text = dialogueData.speakerName;
        contentText.text = line.content;

        // 控制"查看选项"按钮显示
        openOptionsButton.gameObject.SetActive(line.hasOptions);
    }

    // ========== 选项弹窗逻辑 ==========

    void OpenOptionsWindow()
    {
        if(openOptionsButton != null) openOptionsButton.gameObject.SetActive(false);  // 隐藏主界面按钮
        var currentOptions = dialogueData.lines[currentLine].options;
        if (currentOptions == null || currentOptions.Length == 0) return;

        // 清空旧选项
        foreach (Transform child in optionsContainer)
            if (child != null)Destroy(child.gameObject);

        resultText.text = "";  // 清空上次结果

        // 生成选项按钮
        foreach (var option in currentOptions)
        {
            GameObject btn = Instantiate(optionButtonPrefab, optionsContainer);
            if (option.optionText != null)
                btn.GetComponentInChildren<TMP_Text>().text = option.optionText;

            // 关键：点击后直接显示结果，不跳转对话
            var opt = option;  // 闭包捕获
            btn.GetComponent < Button > ().onClick.AddListener(() => OnOptionSelected(opt));
        }

        optionsWindow.SetActive(true);
    }

    void OnOptionSelected(DialogueData.DialogueOption option)
    {
        resultText.text = option.resultText;

        // 清空选项，只保留结果和关闭按钮
        foreach (Transform child in optionsContainer)
            if (child != null) Destroy(child.gameObject);
    }

    public void CloseOptionsWindow()
    {
        optionsWindow.SetActive(false);
        // 不需要回到任何对话状态，直接关闭即可
    }

    void NextLine()
    {
        currentLine++;
        ShowLine();
    }

    public void EndDialogue()
    {
        isDialogueActive = false;
        dialoguePanel.SetActive(false);
        CloseOptionsWindow();  // 确保弹窗也关闭
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) isPlayerInRange = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (isDialogueActive) EndDialogue();
        }
    }
}