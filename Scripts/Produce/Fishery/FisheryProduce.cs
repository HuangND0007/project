using TMPro;
using Unity.VisualScripting;
using UnityEngine;



public class FisheryProduce : InitialProduce
{
    public InitialProduce parent;
    public FisheryData fisheryData;
    public new static FisheryProduce Instance { get; private set; }

    private float Area = 0f;//湖池面积
    private float FishNum = 20;
    public int[] RipeNum;//成长度
    [SerializeField]
    private ItemSO LIttleFish;
    private ItemSO Fertilazation;
    public GameObject NoItemsTip;
    public GameObject NoItemText;


    private void UpdateInformation()
    {
        GameObject Text = parent.IniInformationPanel.GetComponentInChildren<TMP_Text>().gameObject;
        if (Destory >= 1) Text.GetComponent<TMP_Text>().text =
            "污染度:已污染,无法继续渔获";
        else
            Text.GetComponent<TMP_Text>().text = 
            "湖池面积:" + Area + "\n" +
            "已播鱼苗数量:" + FishNum + "\n" +
            "鱼苗成熟度(0~5):" + RipeNum[0] + "/" + RipeNum[1] + "/" + RipeNum[2] + "/" + RipeNum[3] + "\n" +
            "/" + RipeNum[4] + "/" + RipeNum[5] + "\n" +
            "营养度:" + Nutrition + "\n" +
            "污染度:" + Destory;

    }
    public float GetterArea()
    {
        return Area;
    }
    private bool IsFertilizationInBackpack()
    {
        bool hasFertilization = false;
        foreach (BackpackSlots slot in backpack.itemSlots)
        {
            if (slot.itemSO != null && slot.itemSO.ID == 1004 && slot.amount > 20 )
            {
                hasFertilization = true;
                this.Fertilazation = slot.itemSO;
                break;
            }
        }
        return hasFertilization;
    }
    private bool IsLittleFishInBackpack()
    {
        bool hasLittleFish = false;
        foreach (BackpackSlots slot in backpack.itemSlots)
        {
            if (slot.itemSO != null && slot.itemSO.ID == 1010 && slot.amount > 20)
            {
                hasLittleFish = true;
                this.LIttleFish = slot.itemSO;
                break;
            }
        }
        return hasLittleFish;
    }

    public void Assart()
    {//开垦override
        if (!parent.WorkJudge()) return;
        Area = 50f;
        SetTime(3f);
        UpdateUI();
        HideOperationUI();
    }

    public void Seeding()
    {//播种override
        if (!parent.WorkJudge()) return;
        if (!IsLittleFishInBackpack())
        {
            NoItemsTip.SetActive(true);
            parent.ActiveTipsUI(NoItemsTip);
            NoItemText.GetComponent<TMP_Text>().text = "背包鱼苗数量不太够(20起),或许应该去买点";
            HideOperationUI();
            return;
        }
        BackpackItemTranslate(LIttleFish, -20);
        FishNum += 20;
        SetTime(FishNum * 0.5f * GE() / 60);
        MV(-(FishNum * 0.5f * GE()));
        this.Ripe = false;
        RipeNum[0] += 20;
        UpdateUI();
        HideOperationUI();
    }
    public void Fertilizing()
    {//施肥override
        if (!parent.WorkJudge()) return;

        if (!IsFertilizationInBackpack())
        {
            NoItemsTip.SetActive(true);
            parent.ActiveTipsUI(NoItemsTip);
            NoItemText.GetComponent<TMP_Text>().text = "背包中肥料数量不太够(20起),或许应该去买点";
            HideOperationUI();
            return;
        }

        BackpackItemTranslate(Fertilazation, -20);
        SetTime(Area * 0.065f * GE());
        this.Nutrition += 10f;
        MV(-(Area * 0.65f * GE()));
        UpdateUI();
        HideOperationUI();
    }
    public void Reap()
    {//收获override
        if (!parent.WorkJudge()) return;
        if (!Ripe)
        {//成熟度不够,无法收获
            parent.UnripeTips();
            HideOperationUI();
            return;
        }
        BackpackItemTranslate(this.ProductiveItem, (int)Output);
        //Ripe = false;
        UpdateUI();
        HideOperationUI();
        FishNum -= RipeNum[5];
        RipeNum[5] = 0;

    }


    public void SetIni()
    {//设置初始UI override
        parent.ToInfomationPanel();
        UpdateInformation();
        parent.Button1.onClick.RemoveAllListeners();
        parent.Button2.onClick.RemoveAllListeners();
        parent.Button3.onClick.RemoveAllListeners();
        if (Destory >= 1)
        {
            parent.Button1.interactable = false;
            parent.Button2.interactable = false;
            parent.Button3.interactable = false;
            parent.ButtonText1.GetComponent<TMP_Text>().text = "已破坏";
            parent.ButtonText2.GetComponent<TMP_Text>().text = "已破坏";
            parent.ButtonText3.GetComponent<TMP_Text>().text = "已破坏";
            parent.OperateText.GetComponent<TMP_Text>().text = "湖泊已被污染,无法任何操作";
            return;
        }
        parent.ButtonText1.GetComponent<TMP_Text>().text = "撒鱼苗";
        parent.ButtonText2.GetComponent<TMP_Text>().text = "播鱼肥";
        parent.ButtonText3.GetComponent<TMP_Text>().text = "收获";
        parent.OperateText.GetComponent<TMP_Text>().text = "对这块湖泊做些什么好呢...";
        parent.Unripe.GetComponentInChildren<TMP_Text>().text = "鱼苗还未成熟,无法收获";
        parent.Button1.onClick.AddListener(() => { Seeding(); UpdateInformation(); });
        parent.Button2.onClick.AddListener(() => { Fertilizing(); UpdateInformation(); });
        parent.Button3.onClick.AddListener(() => { Reap(); UpdateInformation(); });
    }

    public void Grow()//季节结束被调用,作物生长
    {
        if (IsWinter()) return;
        var sea = GSN();
        if (sea > 1)
        {
            for (int i = 0; i < sea; i++)
                RipeNum[5] += RipeNum[5 - i];
        }
        for (int i = 5; i >= sea; i--)
        {
            RipeNum[i] += RipeNum[i - sea];
        }
        if (RipeNum[5] >= 0)
        { //成熟度达到5,可以收获
            //RipeNum = 0;
            Ripe = true;
        }
        Income(45 * Nutrition * Cooperation );
        ProductiveValue += 45 * Nutrition * Cooperation;
        if (Cooperation < 1.1 && ProductiveValue >= 8000)
        {
            CooperationSet.Instance.SetTeget(2);
        }
    }

    public void Clearing()
    { //季节性结算,包括生长和破坏值变化override
        Grow();
        this.DestoryChange();
    }



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        fisheryData = ProduceSaving.Instance.GetFisheryData();
        this.ProductiveValue = fisheryData.ProductiveValue;
        this.Destory = fisheryData.Destory;
        this.Nutrition = fisheryData.Nutrition;
        this.Ripe = fisheryData.Ripe;
        this.Output = fisheryData.Output;

        this.Area = fisheryData.Area;
        this.FishNum = fisheryData.FishNum;
        this.RipeNum = fisheryData.RipeNum;
        if (RipeNum.IsUnityNull())
        {
            RipeNum = new int[] { 0, 0, 0, 0, 0, 0 };
        }
    }

    private void OnDestroy()
    {
        ProduceSaving.Instance.SetFisheryData(new FisheryData(
            this.Cooperation,
            ProductiveValue, Destory, Nutrition, Ripe, Output,
            FishNum, Area, RipeNum));
    }
}
