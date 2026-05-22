using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Events : MonoBehaviour
{
    public ItemSO Good;
    public float value;
    public void Work(float timeIni)
    {
        if (Environment.Instance.returnLight() < 1.0f)
        { return; }
        else
        {
            float time;
            time = timeIni / PlayerManager.Instance.GetEffience();
            Environment.Instance.setTime(time);
        }
    }

    public void Purchase()
    {
        if (PlayerManager.Instance.GetMoney() < value)
        {
            //其他的提示语句可以在UI上显示,这里直接返回
            return;//如果金钱不足,则返回,表示操作失败
        }
        PlayerManager.Instance.ModifyMoney(-value);//扣除金钱
        BackpackManager.Instance.UpdateItems(Good, 1);//增加物品数量  
    }

    private void Start()
    {
        //test();
    }
}
