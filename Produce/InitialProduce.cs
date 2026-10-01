using UnityEngine;
using UnityEngine.UI;

public abstract class InitialProduce : MonoBehaviour//各个产业的父类脚本
{

    private float Input;//产业的投入值,各个子类会有一个新的对应值,不被继承


    protected float ProductiveValue = 0f;//产值
    protected float Destory = 0f;//破坏度或者说污染度,小于1的小数计算
    protected float Nutrition = 20f;//区域营养度,初始值20,影响以上两个数值
    protected bool Ripe;//是否成熟,成熟后可以收
    protected float Output;//产量

    public ItemSO ProductiveItem;//产出的物品,各个子类会自己赋值


    public Button Button1;
    public GameObject ButtonText1;
    public Button Button2;
    public GameObject ButtonText2;
    public Button Button3;
    public GameObject ButtonText3;
    public GameObject OperateText;
    public GameObject Unripe;//未成熟时弹出提示
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

    abstract public void SetIni();//设置初始UI,各个产业的UI不同,各子类会重写
    abstract public void Assart();//广义开垦,各个产业的开垦方式不同,各子类会重写
    abstract public void Seeding();//广义播种,各个产业的播种方式(即投入方式)不同,各子类会重写
    abstract public void Fertilizing();//广义施肥,各个产业的施肥方式不同,各子类会重写
    abstract public void Reap();//广义收获,各个产业的收获方式不同,各子类会重写
    abstract public void Clearing();//季节性结算





    /// 用到的其他的方法的简写,用于一些设置
    protected void UpdateUI() => TimeUImanage.Instance.UpdateUI();
    protected void SetTime(int time) => Environment.Instance.SetTime(time);
    protected void SetTime(float time) => Environment.Instance.SetTime(time);
    protected void NoMove() => Player.Instance.SetMoveFalse();
    protected void ToMove() => Player.Instance.SetMoveTrue();
    protected void Gain(ItemSO Item, int num) {
        BackpackManager.Instance.UpdateItems( Item, num );
    }
    protected bool WorkJudge() 
    {  
        if ( GL() < 1.0f )
        {
            Dim.SetActive(true);
            return false;
        }
        return true;
    }

    protected static PlayerManager playerData;
    protected static bool IsWinter()
    { //判断是否是冬天,一般作物冬季不生长
        return Environment.Instance.getSeasonNum() == 3 ? true : false;
    }



    ///一系列简写调用,可能之后会看不懂(
    protected static float GE() => PlayerManager.Instance.GetEffience();
    protected static void MV(float vitality) => PlayerManager.Instance.ModifyVitality(vitality);
    protected static int GSN() => Environment.Instance.getSeasonNum();
    protected static float GL() => Environment.Instance.getLight();
    
}
