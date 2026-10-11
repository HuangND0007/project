using UnityEngine;

public class ShopTrans : MonoBehaviour
{
    public GameObject PurchUI;
    public GameObject SellUI;

    public void OpenPurchUI()
    {
        PurchUI.SetActive(true);
        SellUI.SetActive(false);
    }

    public void OpenSellUI()
    {
        SellUI.SetActive(true);
        PurchUI.SetActive(false);
    }

}
