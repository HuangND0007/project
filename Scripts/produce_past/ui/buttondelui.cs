using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttondelui : MonoBehaviour
{
    public GameObject ui;
    public void OnLogButtonClick()
    {
        ui.SetActive(false);
    }
}
