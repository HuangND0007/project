using TMPro;
using System.Collections;
using UnityEngine;
using Unity.VisualScripting;



public class AgricultureProduce : InitialProduce
{
    public InitialProduce parent;
    public AgricultureData agricultureData;
    public new static AgricultureProduce Instance { get; private set; }

    private float Area = 0f;//农田面积
    private float SeededNum;
    public int[] RipeNum;//成长度 = new int[] { 0, 0, 0, 0, 0, 0 } 
    private ItemSO Seed;
    private ItemSO Fertilazation;
    public GameObject NoItemsTip;
    public GameObject NoItemText;


    private void UpdateInformation()
    { 
        GameObject Text = parent.IniInformationPanel.GetComponentInChildren<TMP_Text>().gameObject;
        if (Destory >= 1) Text.GetComponent<TMP_Text>().text =
            "已破坏,无法继续耕种";
        else
            Text.GetComponent<TMP_Text>().text = "农田面积:" + Area + "\n" +
            "已播种数量:" + SeededNum + "\n" +
            "成熟度的数量(0~5):" + RipeNum[0] + "/" + RipeNum[1] + "/" + RipeNum[2] + "/" + RipeNum[3] + "\n" +
            "/" + RipeNum[4] + "/" + RipeNum[5]  +"\n" +
            "营养度:" + Nutrition + "\n" +
            "破坏度:" + Destory;

    }
    public float GetterArea(){
        return Area;
    }
    public float GetterSeededNum()
    {
        return SeededNum;
    }
    private bool IsSeedsInBackpack()
    {
        bool hasSeeds = false;
        foreach (BackpackSlots slot in backpack.itemSlots)
        {
            if (slot.itemSO != null && slot.itemSO.ID == 1003 && slot.amount > 20)
            {
                hasSeeds = true;
                this.Seed = slot.itemSO;
                SeededNum += 20;
                break;
            }
        }
        return hasSeeds;
    }
    private bool IsFertilizationInBackpack()
    {
        bool hasFertilization = false;
        foreach (BackpackSlots slot in backpack.itemSlots)
        {
            if (slot.itemSO != null && slot.itemSO.ID == 1004 && slot.amount > 20)
            {
                hasFertilization = true;
                this.Fertilazation = slot.itemSO;
                break;
            }
        }
        return hasFertilization;
    }
    public  void Assart()
    {//开垦override
        if (!parent.WorkJudge()) return;
        Area = 50f;
        SetTime(3f);
        UpdateUI();
        HideOperationUI();
    }

    public  void Seeding()
    {//播种override
        if(!parent.WorkJudge()) return;
       

        if (!IsSeedsInBackpack())
        {
            NoItemsTip.SetActive(true);
            parent.ActiveTipsUI(NoItemsTip);
            NoItemText.GetComponent<TMP_Text>().text = "背包中没有种子,或许应该去买点";
            HideOperationUI();
            return;
        }

        BackpackItemTranslate( Seed ,-20 );
        SeededNum += 20;
        SetTime(SeededNum * 5f * GE() /60 );
        MV(-(SeededNum * 0.5f * GE()));
        //this.Ripe = false;
        RipeNum[0] += 20;
        UpdateUI();
        HideOperationUI();
    }
    public  void Fertilizing ()
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

        BackpackItemTranslate(Fertilazation, -20 );
        SetTime(Area * 0.065f * GE() / 60);
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
        Ripe = false;

        BackpackItemTranslate(this.ProductiveItem ,(int)Output);
        UpdateUI();
        HideOperationUI();
        SeededNum -= RipeNum[5];
        RipeNum[5] = 0;


    }


    public  void SetIni()
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
            parent.OperateText.GetComponent<TMP_Text>().text = "土地已被破坏,无法任何操作";
            return;
        }
        parent.ButtonText1.GetComponent<TMP_Text>().text = "播种";
        parent.ButtonText2.GetComponent<TMP_Text>().text = "施肥";
        parent.ButtonText3.GetComponent<TMP_Text>().text = "收获";
        parent.OperateText.GetComponent<TMP_Text>().text = "对这块土地做些什么好呢...";
        parent.Unripe.GetComponentInChildren<TMP_Text>().text = "作物还未成熟,无法收获";
        parent.Button1.onClick.AddListener(() => { Seeding();UpdateInformation(); });
        parent.Button2.onClick.AddListener(() => { Fertilizing(); UpdateInformation(); });
        parent.Button3.onClick.AddListener(() => { Reap(); UpdateInformation(); });
    }

    public void Grow()//季节结束被调用,作物生长
    {
        if (IsWinter()) return;

        var sea = GSN();
        if(sea > 1)
        {
            for (int i = 0; i < sea; i++)
                RipeNum[5] += RipeNum[5 - i];
        }
        for(int i = 5; i >=sea; i--)
        {
            RipeNum[i] += RipeNum[i - sea];
        }

        if (RipeNum[5] > 0)
        { //成熟度达到5,可以收获
            //RipeNum = 0; 
            Ripe = true;
        }
        Income(30 * Area * Nutrition * Cooperation);
        this.ProductiveValue += 30 * Area * Nutrition * Cooperation;

        if(Cooperation < 1.1 && ProductiveValue >= 8000)
        {
            CooperationSet.Instance.SetTeget(1);
        }
    }

    public  void Clearing() 
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
        agricultureData = ProduceSaving.Instance.GetAgricultureData();
            this.Area = agricultureData.Area;
            this.SeededNum = agricultureData.SeededNum;
            this.RipeNum = agricultureData.RipeNum;
            this.Nutrition = agricultureData.Nutrition;
            this.Ripe = agricultureData.Ripe;
            this.Output = agricultureData.Output;
            this.Destory = agricultureData.Destory;

        if (RipeNum.IsUnityNull())
        {
            RipeNum =  new int[] { 0, 0, 0, 0, 0, 0 };
        }
    }
    
    private void OnDestroy()
    {
        ProduceSaving.Instance.SetAgricultureData(new AgricultureData(
            this.Cooperation,
            ProductiveValue, Destory, Nutrition, Ripe, Output,
            SeededNum, Area, RipeNum));
    }
}
