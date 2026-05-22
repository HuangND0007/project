using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BackpackSlots : MonoBehaviour
{
    public ItemSO itemSO;
    public int amount;

    public Image itemImage;
    public TMP_Text Amount;

    public void UpdateUI()
    {
        
        if (itemSO != null)
        {
            if(amount == 0) 
            {
                itemSO = null;
                itemImage.gameObject.SetActive(false);
                Amount.text = "";
                return;
            }
            itemImage.sprite = itemSO.Icon;
            itemImage.gameObject.SetActive(true);
            Amount.text = amount.ToString();
        }
        else
        {
            itemImage.gameObject.SetActive(false);
                Amount.text = "";
        }
    }
}
