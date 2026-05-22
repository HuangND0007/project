using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "NPC/Dialogue")]
public class DialogueData : ScriptableObject
{
    public string speakerName;
    [System.Serializable]
    public class DialogueOption
    {
        public string optionText;
        public string resultText;      // 选择后的反馈文字
    }

    [System.Serializable]
    public class DialogueLine
    {
        [TextArea(3, 5)]
        public string content;
        public bool hasOptions;        // 是否显示"查看选项"按钮
        public bool shop;          // 如果hasOptions为true，显示选项按钮；如果shop为true，显示商店按钮
        public DialogueOption[] options;
    }

    public DialogueLine[] lines;
}