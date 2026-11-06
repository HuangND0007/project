using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CIn : MonoBehaviour
{
    private string Name;
    public CanvasGroup canvasGroup;
    public TMP_InputField inputField;
    //public TMP_InputField outputField;
    public Button confirmButton;


    private void Start()
    {
        confirmButton.onClick.AddListener(() =>
        {
            Input();
        });
    }
    private void OnEnable()
    {
        ActiveUI();
    }

    private void Update()
    {
    }
    public void ActiveUI()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

    }
    private void DeactiveUI()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public void Input()
    {
        Name = inputField.text;
        PlayerManager.Instance.SetName(Name);
        BackpackManager.Instance.NameUpdate();
        //outputField.text = "输入成功！当前玩家名字为：" + PlayerManager.Instance.GetName();
        DeactiveUI();
    }






}