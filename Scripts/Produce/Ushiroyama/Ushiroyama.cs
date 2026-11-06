using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Ushiroyama : MonoBehaviour
{
    private Environment evn = Environment.Instance;
    private Player player = Player.Instance;
    private PlayerManager PM = PlayerManager.Instance;
    private BackpackManager backpack = BackpackManager.Instance;
    private void NoMove() => player.SetMoveFalse();
    private void ToMove() => player.SetMoveTrue();
    [SerializeField]
    public UshiroyamaData ush;

    public static Ushiroyama Instance { get; private set; }

    [SerializeField]
    private ItemSO MushroomItem;
    [SerializeField]
    private ItemSO WoodItem;
    [SerializeField]
    private GameObject OperationUI;
    [SerializeField]
    public GameObject TipsUI;

    private ItemSO Sapling;
    private float Froest ;//林木存量百分比= 1f
    private float Destory = 0f;//破坏度百分比
    private int Mushroom;//蘑菇存量 = 100 
    private int SaplingNum = 0;//树苗数量
    private float ProductiveValue = 0f;
    private bool IsPicked = false;
    private float Cooperation;

    public void DestoryChange()//用于更新破坏度,计算破坏度变化
    {
        if (Destory >= 1) return;//破坏不可逆

        Destory += ((1 - Froest) * 0.25f);
        //破坏度的数值规范
        if (Destory < 0) Destory = 0;
        else if (Destory > 1)
        {
            Destory = 100;
            DataEvents.Instance.SetDestoryForetry();
        }
    }

    public void Lumbering() 
    {
        if (Froest < 0.2f)
        {
            if (TipsUI != null)
            {
                TipsUI.SetActive(true);
                TipsUI.GetComponentInChildren<TMPro.TMP_Text>().text = "林木存量过低,无法伐木";
                StartCoroutine(WaitTips());
            }
            return;
        }
        evn.SetTime(2f);
        backpack.UpdateItems(WoodItem, 20);
        PM.ModifyVitality(-12.5f);
        FroestChange(-0.2f);

    }

    public void Plant()
    { 
        if(!IsSeedsInBackpack())
        {
            if (TipsUI != null)
            {
                TipsUI.SetActive(true);
                TipsUI.GetComponentInChildren<TMPro.TMP_Text>().text = "背包中没有树苗,无法种植";
                StartCoroutine(WaitTips());
            }
            return;
        }
        evn.SetTime(2f * SaplingNum / 50);
        backpack.UpdateItems(Sapling, -SaplingNum);
        PM.ModifyVitality(-12.5f * SaplingNum / 50);
        FroestChange(0.2f);

    }

    public void PickMushroom()
    {
        if (IsPicked)
        {
            TipsUI.SetActive(true);
            TipsUI.GetComponentInChildren<TMPro.TMP_Text>().text = "蘑菇需要生长,下次再来吧";
            StartCoroutine(WaitTips());
            return;
        }
        if (Mushroom <= 0)
        {
            if (TipsUI != null)
            {
                TipsUI.SetActive(true);
                TipsUI.GetComponentInChildren<TMPro.TMP_Text>().text = "蘑菇存量不足,无法采摘";
                StartCoroutine(WaitTips());
            }
            return;
        }
        evn.SetTime(2f);
        backpack.UpdateItems(MushroomItem, 20);
        PM.ModifyVitality(-12.5f);
        Mushroom -= 20;
        IsPicked = true;
    }
    private void FroestChange(float change)//用于更新林木存量,传入变化值,计算林木存量变化
    {
        Froest += change;
        //林木存量的数值规范
        if (Froest < 0) Froest = 0;
        else if (Froest > 1) Froest = 1;
    }
    










    private bool IsSeedsInBackpack()
    {
        bool hasSapling = false;
        foreach (BackpackSlots slot in backpack.itemSlots)
        {
            if (slot.itemSO != null && slot.itemSO.ID == 1009 && slot.amount > 1)
            {
                hasSapling = true;
                Sapling = slot.itemSO;
                SaplingNum = slot.amount - 1;
                break;
            }
        }
        return hasSapling;
    }
    IEnumerator WaitTips() { 
        yield return new WaitForSeconds(2f);
        if (TipsUI != null)
        {
            TipsUI.SetActive(false);
        }
    }
    public float GetFroest()
    {
        return Froest;
    }

    public bool WorkJudge()
    {
        if (evn.getLight() <= 1.0f)
        {
            print("光照不足，无法工作");
            if (TipsUI != null)
            {
                TipsUI.SetActive(true);
                ToMove();
            }
            StartCoroutine(WaitTips());
            return false;
        }
        return true;
    }


   
    public void HideUIS()
    {
        if (OperationUI != null)
            OperationUI.SetActive(false);
        if (TipsUI != null)
            TipsUI.SetActive(false);

        ToMove();
    }





    public void Grow()//季节结束被调用,作物生长
    {
        var income = 30 * (100 - Mushroom) + Froest * 1000 * 20;
        DataEvents.Instance.Income(income);
        ProductiveValue += income; 
        DestoryChange();
    }

    private void Awake()
    {
        if (Instance == null && Instance != this)
        {
            Instance = this;
            //DontDestroyOnLoad(this.gameObject);
        }
        
        ush = ProduceSaving.Instance.GetUshiroyamaData();
        Froest = ush.Froest;
        Destory = ush.Destory;
        Mushroom = ush.Mushroom;
    }

    private void OnDestroy()
    {
        ProduceSaving.Instance.SetUshiroyamaData(new UshiroyamaData(
            Froest, Destory, Mushroom,ProductiveValue , Cooperation));
    }

}
