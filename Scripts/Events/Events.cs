using UnityEngine;

public class Events : MonoBehaviour
{
    public ItemSO Good;
    public float value;
    public int Amount;
    public void Work(float timeIni)///废弃
    {
        //if (Environment.Instance.getLight() < 1.0f)
        //{ return; }
        //else
        //{
        //    float time;
        //    time = timeIni / PlayerManager.Instance.GetEffience();
        //    Environment.Instance.SetTime(time);
        //}
    }

    public void Purchase()
    {
        Debug.Log("Purchase内调用");
        if (PlayerManager.Instance.GetMoney() < value)
        {
            //其他的提示语句可以在UI上显示,这里直接返回
            return;//如果金钱不足,则返回,表示操作失败
        }
        PlayerManager.Instance.ModifyMoney(-value);//扣除金钱
        BackpackManager.Instance.UpdateItems(Good,Amount);//增加物品数量  
    }

    private void Start()
    {

    }
    
}
