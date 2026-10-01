using TMPro;
using System.Collections;
using UnityEngine;


public class Agriculture : InitialProduce
{
    public static Agriculture Instance { get; private set; }

    private float Area = 0f;//农田面积
    private float SeededNum;
    private int RipeNum;//成长度


    public float GetterArea(){
        return Area;
    }
    public override void Assart()
    {
        Area = 50f;
        SetTime(3f);
        UpdateUI();
    }

    public override void Seeding()
    {//播种
        if(WorkJudge()) return;
        SeededNum += Area * 0.5f;
        SetTime(SeededNum * 0.5f * GE());
        MV(-(SeededNum * 0.5f * GE()));
        UpdateUI();
    }
    public override void Fertilizing ()
    {//施肥
        if (WorkJudge()) return;
        SetTime(Area * 0.65f * GE());
        this.Nutrition += 10f;
        MV(-(Area * 0.65f * GE()));
        UpdateUI();
    }
    public override void Reap()
    {//收获
        if (WorkJudge()) return;
        if (!Ripe)
        {//成熟度不够,无法收获
            if (Unripe!=null)
            Unripe.SetActive(true);
            StartCoroutine(Wait());
            return;
        }
        Gain(ProductiveItem ,(int)Output);

    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(2f);
        if (Unripe != null)
            Unripe.SetActive(false);
    }


    public override void SetIni()
    {
        ButtonText1.GetComponent<TMP_Text>().text = "播种";
        ButtonText2.GetComponent<TMP_Text>().text = "施肥";
        ButtonText3.GetComponent<TMP_Text>().text = "收获";
        OperateText.GetComponent<TMP_Text>().text = "对这块土地做些什么好呢...";
        Button1.onClick.AddListener(() => { Seeding(); });
        Button2.onClick.AddListener(() => { Fertilizing(); });
        Button3.onClick.AddListener(() => { Reap(); });
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

    public override void Clearing() { //季节性结算,包括生长和破坏值变化
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
