using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Pondui : MonoBehaviour
{
    public GameObject UIText;
    public Pond targetPond;   // 

    void Update()
    {
       
        if (targetPond != null)
            UIText.GetComponent<TMP_Text>().text = "ø… ’ªÒ£∫" + targetPond.getproduct();
    }
}
