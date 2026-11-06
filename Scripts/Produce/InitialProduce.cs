using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HideInDerivedAttribute : PropertyAttribute { }

public class InitialProduce : MonoBehaviour//各个产业的父类脚本
{
    public static InitialProduce Instance { get; private set; }
    private float Input;//产业的投入值,各个子类会有一个新的对应值,不被继承


    protected float ProductiveValue = 0f;//产值
    protected float Destory = 0f;//破坏度或者说污染度,小于1的小数计算
    protected float Nutrition = 20f;//区域营养度,初始值20,影响以上两个数值
    protected bool Ripe;//是否成熟,成熟后可以收
    protected float Output;//产量
    protected float Cooperation = 1f;//合作商带来的增产,接受合作后变为1.5,并且会增长;拒绝合作变为1.1不再变化

    public ItemSO ProductiveItem;//产出的物品,各个子类会自己赋值
    public GameObject IniInformationPanel;

    [HideInDerived]
    public Button Button1;
    [HideInDerived]
    public GameObject ButtonText1;
    [HideInDerived]
    public Button Button2;
    [HideInDerived]
    public GameObject ButtonText2;
    [HideInDerived]
    public Button Button3;
    [HideInDerived]
    public GameObject ButtonText3;
    [HideInDerived]
    public GameObject OperateText;
    [HideInDerived]
    public GameObject TipsUI;
    [HideInDerived]
    public GameObject Unripe;//未成熟时弹出提示
    [HideInDerived]
    public GameObject Dim;//提示亮度不够,无法工作


    public void DestoryChange()//用于更新破坏度,传入营养度,计算破坏度变化
    {
        if (Destory >= 1) return;//破坏不可逆

        if (Nutrition > 0)//过高过低都有问题
            Destory += ((Nutrition - 70) * 0.25f);
        else
            Destory += ((Nutrition + 70) * 0.25f);

        //破坏度的数值规范
        if (Destory < 0) Destory = 0;
        else if (Destory > 1)
        {
            Destory = 100;
            DataEvents.Instance.IncreaceDestoryCount();
        }
    }

    public bool WorkJudge()
    {
        Debug.Log("光照被判定");
        if (GL() <= 1.0f)
        {
            print("光照不足，无法工作");
            if (TipsUI != null && Dim != null)
            {
                TipsUI.SetActive(true);
                Dim.SetActive(true);
                print("Dim is not null");
                ToMove();
            }
            StartCoroutine(Wait(Dim));
            return false;
        }
        return true;
    }
    public void UnripeTips()
    {
        print("产品还未成熟,无法收获");
        if (TipsUI != null && Unripe != null)
        {
            TipsUI.SetActive(true);
            Unripe.SetActive(true);
            StartCoroutine(Wait(Unripe));
        }
    }
    public void ToInfomationPanel() { 
        if (IniInformationPanel != null)
        {
           IniInformationPanel.SetActive(true);
        }
    }

    public void ProductCooperationSet(int choice , string type)
    {
        switch (choice)
        {
            case 1: Cooperation = 1.1f;Refuse(type); break;
            case 2: Cooperation = 1.5f;Receive(type); break;
        }
    }
    public void ActiveTipsUI(GameObject obj) //外部调用激活提示UI,并在2秒后关闭
    {
        TipsUI.SetActive(true);
        StartCoroutine(Wait(obj));
    }
    private IEnumerator Wait(GameObject obj)
    {

        yield return new WaitForSeconds(2f);
        if (obj != null && TipsUI != null)
        {
            obj.SetActive(false);
            TipsUI.SetActive(false);
        }
        HideOperationUI();
    }



    /// 用到的其他的方法的简写,用于一些设置
    protected void UpdateUI() => TimeUImanage.Instance.UpdateUI();
    protected void SetTime(int time) => Environment.Instance.SetTime(time);
    protected void SetTime(float time) => Environment.Instance.SetTime(time);
    protected void NoMove() => Player.Instance.SetMoveFalse();
    protected void ToMove() => Player.Instance.SetMoveTrue();
    protected void BackpackItemTranslate(ItemSO Item , int num) {
        backpack.UpdateItems( Item , num );
    }


    protected static PlayerManager playerData = PlayerManager.Instance;
    protected static BackpackManager backpack = BackpackManager.Instance;
    protected static Environment env = Environment.Instance;
    protected static bool IsWinter()
    { //判断是否是冬天,一般作物冬季不生长
        return env.getSeasonNum() == 3 ;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }



    ///一系列简写调用,可能之后会看不懂(
    protected static float GE() => PlayerManager.Instance.GetEffience();
    protected static void MV(float vitality) => PlayerManager.Instance.ModifyVitality(vitality);
    protected static int GSN() => Environment.Instance.getSeasonNum();
    protected static float GL() => Environment.Instance.getLight();
    protected static void Income(float income) => DataEvents.Instance.Income(income);
    public static void Refuse(string type) => DataEvents.Instance.Refuse(type);
    public static void Receive(string type) => DataEvents.Instance.Receive(type);

    protected static void HideOperationUI()
    {
       
        InitialOperation.Instance.HideOperationUI();
    }
    public void HideIni()
    {
        IniInformationPanel.SetActive(false);
    }

}
