using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Events : MonoBehaviour
{
    public ItemSO[] Goods;
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

    public void test()
    {
        BackpackManager.Instance.UpdateItems(Goods[1], 10); 
        BackpackManager.Instance.UpdateItems(Goods[0], 10);
    }

    private void Start()
    {
        //test();
    }
}
