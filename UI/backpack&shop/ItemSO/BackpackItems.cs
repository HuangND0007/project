using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    public ItemSO itemSO;
    public SpriteRenderer sr;




    private void OnValidate()
    {
        if (itemSO == null) 
        {
            Debug.Log("ÎïÆ·SO¶ªÊ§ÁË");
            return;

        }
        sr.sprite = itemSO.Icon;
        this.name = itemSO.itemName;


    }

}
