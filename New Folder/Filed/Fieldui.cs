using TMPro;
using UnityEngine;

public class fieldui : MonoBehaviour
{
    public GameObject UIText;
    public Field targetField;   // 农田

    void Update()
    {
        //if (targetField != null)
        //{
        //    Debug.Log("product 当前值: " + targetField.getproduct());
        //    UIText.GetComponent<TMP_Text>().text = "可收获：" + targetField.getproduct();
        //}
        //else
        //{
        //    Debug.LogWarning("targetField 是空的！");
        //}


        if (targetField != null)
            UIText.GetComponent<TMP_Text>().text = "可收获：" + targetField.getproduct();
    }
}