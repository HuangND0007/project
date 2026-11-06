using UnityEngine;
using TMPro;


public class FinalText : MonoBehaviour
{
    public static FinalText Instance { get; private set; }
    public DialogueData Taget;
    public TMP_Text ContentText;

    private int CurrentLine = 0;

    public void SetTeget(DialogueData taget) 
    {
        Taget = taget;
        NextLine();
    }
    private void NextLine()
    {
        if (CurrentLine >= Taget.lines.Length)
        {
            FinalControl.Instance.ExitGame();
            return;
        }
        ContentText.text = Taget.lines[CurrentLine].content;
        CurrentLine++; 
    }
    private void Update()
    {
        if (Taget != null)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Player.Instance.SetMoveFalse();  // 禁止玩家移动
                Exit.Instance.TrueEscChoke();  // 禁止ESC键功能
                Debug.Log("按下F键，触发对话逻辑");
                NextLine();
            }
        }
    }











    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}
