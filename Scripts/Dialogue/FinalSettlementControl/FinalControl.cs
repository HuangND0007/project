using UnityEngine;
using Unity.UI;
using UnityEngine.UI;

public class FinalControl : MonoBehaviour
{
    public static FinalControl Instance { get; private set; }

    public DialogueData Death;
    public DialogueData Destory;
    public DialogueData Manor;//庄园结局
    public DialogueData CommonEnd;//普通的开发者
    public DialogueData GreatCoorperator;//优秀合作人
    public DialogueData HighIncome;//真正的(?)休闲山庄

    public GameObject EndContainer;
    public Image EndImage;
    public Button EndButton;
    public GameObject TextTainer;

    public Sprite dea;
    public Sprite des;
    public Sprite other;

    private void SetTeget(DialogueData dia) => FinalText.Instance.SetTeget(dia);

    public void SetFinal(int choice)
    {
        if (EndContainer != null) EndContainer.SetActive(true);
        else {
            print("EndContainer 未指定");return;
                }
        switch (choice) 
        { 
            case 1: 
                SetTeget(Death);
                EndImage.sprite = dea;
                break;
            case 2:
                SetTeget(Destory);
                EndImage.sprite = des;
                break;
            case 3:
                SetTeget(Manor);
                EndImage.sprite = other;
                break;
            case 4:
                SetTeget(CommonEnd);
                EndImage.sprite = other;
                break;
            case 5:
                SetTeget(GreatCoorperator);
                EndImage.sprite = other;
                break;
            case 6:
                SetTeget(HighIncome);
                EndImage.sprite = other;
                break;
            default:return;

        }
    }

    public void ExitGame()
    {
        TextTainer?.SetActive(false);
        EndButton.onClick.AddListener(() =>
        {
            DataManager.Instance.ButtonOnClick();
            TimeUImanage.Instance.HideUI();
        });
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}
