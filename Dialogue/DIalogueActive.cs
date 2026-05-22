using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DIalogueActive : MonoBehaviour
{
   public static DIalogueActive instance { get; private set; }

   private GameObject UI;
    private void Awake()
    {
        UI = this.gameObject;
    }

    public void SetActive()
    {
        UI.SetActive(true);
    }
    public void SetUnactive()
    {
        UI.SetActive(false);
    }
}
