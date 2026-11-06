using UnityEngine;

/// <summary>
/// 挂在每个 NPC 身上：只保存自己的对话数据和触发范围。
/// 所有 UI 操作统一交给 DialogueManager，不再持有任何 UI 引用。
/// </summary>
public class NPCDialogue : MonoBehaviour
{
    public static NPCDialogue Instance {get ; private set; }
    [SerializeField] private DialogueData dialogueData;
    private bool isPlayerInRange = false;
    private bool isDialogue = false;

    void Update()
    {
        if (!isPlayerInRange || DialogueManager.Instance == null) return;
        isDialogue = true;
        if (Input.GetKeyDown(KeyCode.F))
        {
            var mgr = DialogueManager.Instance;
            if (mgr.IsTalkingTo(dialogueData))
                mgr.NextLine();                      // 正在和本NPC对话 → 推进下一行
            else if (!mgr.IsBusy)
                mgr.StartDialogue(dialogueData);     // 空闲 → 开始新对话
        }
    }
    public bool IfDialogueActive()
    {
        return isDialogue;
    }

    /// <summary>供外部脚本强制开启本NPC的对话</summary>
    public void ForceStartDialogue()
    {
        if (DialogueManager.Instance != null && !DialogueManager.Instance.IsBusy)
            DialogueManager.Instance.StartDialogue(dialogueData);
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
            // 只在玩家正和本NPC对话时才结束，避免误关别的对话
            if (DialogueManager.Instance != null && DialogueManager.Instance.IsTalkingTo(dialogueData))
            {
                DialogueManager.Instance.EndDialogue();
                isDialogue = false;
            }
        }
    }
}