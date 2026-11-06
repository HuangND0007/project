using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CooperationSet : MonoBehaviour
{
    public static CooperationSet Instance { get; private set; }
    private static void RefuseAgr() => AgricultureProduce.Instance.ProductCooperationSet(1 , "Rural");
    private static void AcceptAgr() => AgricultureProduce.Instance.ProductCooperationSet(2 , "Rural");
    private static void RefuseFish() => FisheryProduce.Instance.ProductCooperationSet(1 , "Fish");
    private static void AcceptFish() => FisheryProduce.Instance.ProductCooperationSet(2 , "Fish");
    private static void RefuseUsh() => ProduceSaving.Instance.UshCooperationSet(1 );
    private static void AcceptUsh() => ProduceSaving.Instance.UshCooperationSet(2);

    [SerializeField]
    private TMP_Text ContentText;
    [SerializeField]
    private Button Refuse;
    [SerializeField]
    private Button Accept;
    [SerializeField]
    private GameObject Container;
    [SerializeField]
    private GameObject Ushiroyama;

    private int choice;
    private DialogueData Taget;
    [SerializeField]
    private DialogueData Agri;
    [SerializeField]
    private DialogueData Fish;
    [SerializeField]
    private DialogueData Ush;



    private int CurrentLine = 0;

    public void SetTeget(int choice)
    {
        Debug.Log("传入数字为:" + choice);
        this.choice = choice;
        switch (choice) {
            case 1: Taget = Agri; break;
            case 2: Taget = Fish; break;
            case 3: 
                Taget = Ush; 

                break;
            default:Taget = Taget = null;break;
        }
        Container.SetActive(true);
        SetButton();
        NextLine();
    }
    private void NextLine()
    {
        if (Taget != null &&  CurrentLine >= Taget.lines.Length)
        {
            Accept.gameObject.SetActive(true);
            Refuse.gameObject.SetActive(true);
            return;
        }
        ContentText.text = Taget.lines[CurrentLine].content;
        CurrentLine++;
    }
    private void Update()
    {
        if (Container.activeSelf && Taget != null)
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
    private void SetButton()
    {
        switch (choice)
        {
            case 1:
                Refuse.onClick.AddListener(() => { RefuseAgr(); });
                Accept.onClick.AddListener(() => { AcceptAgr(); });
                break;
            case 2:
                Refuse.onClick.AddListener(() => { RefuseFish(); });
                Accept.onClick.AddListener(() => { AcceptFish(); });
                break;
            case 3:
                Refuse.onClick.AddListener(() => { RefuseUsh(); });
                Accept.onClick.AddListener(() => { AcceptUsh(); });
                break;
        }
    }

    private void Start()
    {
        Refuse.onClick.AddListener(() => { EndDia(); });
        Accept.onClick.AddListener(() => {EndDia(); }); 
    }

    private void EndDia()
    {
        ContentText.text = "好的了解了,我会转达";
        StartCoroutine(WaitToHide());
    }



    private IEnumerator WaitToHide()
    {
        yield return new WaitForSeconds(3f);
        Container.SetActive(false);
    }




    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}
