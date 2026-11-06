using TMPro;
using System.Collections;
using UnityEngine;

public class AgricultureProduce : InitialProduce
{
    public InitialProduce parent;
    public static AgricultureProduce Instance { get; private set; }

    private float Area = 0f;//农田面积
    private float SeededNum;
    private int RipeNum;//成长度
    private ItemSO Seed;


    public float GetterArea(){
        return Area;
    }
    private bool IsSeedsInBackpack() { 
    bool hasSeeds = false;
        foreach (BackpackSlots slot in backpack.itemSlots) 
        { 
            if(slot.itemSO != null && slot.itemSO.ID == 1003) 
            { 
                hasSeeds = true; 
                break;
            }
        }
        return hasSeeds;
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
            //parent.NoSeedsTips();
            HideOperationUI();
            return;
        }

        BackpackItemTranslate(Seed, (int)(Area * 0.5f));
        SeededNum += Area * 0.5f;
        SetTime(SeededNum * 0.5f * GE());
        MV(-(SeededNum * 0.5f * GE()));
        this.Ripe = false;
        RipeNum = 0;
        UpdateUI();
        HideOperationUI();
    }
    public  void Fertilizing ()
    {//施肥override
        if (!parent.WorkJudge()) return;
        SetTime(Area * 0.65f * GE());
        this.Nutrition += 10f;
        MV(-(Area * 0.65f * GE()));
        UpdateUI();
        HideOperationUI();
    }
    public  void Reap()
    {//收获override
        if (!parent.WorkJudge()) return;
        if (!Ripe)
        {//成熟度不够,无法收获
            parent.UnripeTips();
            HideOperationUI();
            return;
        }
        BackpackItemTranslate(this.ProductiveItem ,(int)Output);
        HideOperationUI();

    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(2f);
        if (Unripe != null)
            Unripe.SetActive(false);
    }


    public  void SetIni()
    {//设置初始UI override
        parent.ButtonText1.GetComponent<TMP_Text>().text = "播种";
        parent.ButtonText2.GetComponent<TMP_Text>().text = "施肥";
        parent.ButtonText3.GetComponent<TMP_Text>().text = "收获";
        parent.OperateText.GetComponent<TMP_Text>().text = "对这块土地做些什么好呢...";
        parent.Unripe.GetComponentInChildren<TMP_Text>().text = "作物还未成熟,无法收获";
        parent.Button1.onClick.AddListener(() => { Seeding(); });
        parent.Button2.onClick.AddListener(() => { Fertilizing(); });
        parent.Button3.onClick.AddListener(() => { Reap(); });
    }

    public void Grow()//季节结束被调用,作物生长
    {
        if (IsWinter()) return;
        RipeNum += GSN();

        if(RipeNum >= 5)
        { //成熟度达到5,可以收获
            RipeNum = 0; 
            Ripe = true;
        }
    }

    public  void Clearing() 
    { //季节性结算,包括生长和破坏值变化override
        Grow();
        this.DestoryChange();
    }



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Instance = this;
            return;
        }
        Instance = this;
    }

}
