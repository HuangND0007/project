using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ui : MonoBehaviour
{
    public GameObject tipui;//ÏÔÊ¾ui
    public GameObject targetui;//Ä¿±êui
    public KeyCode interkey = KeyCode.F;

    private bool playerinrange = false;

    private void Update()
    {
        if (playerinrange && Input.GetKeyDown(interkey))
        {
            if(targetui != null)
            {
                targetui.SetActive(true);
                tipui.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")){
            playerinrange=true;
            if(tipui != null)
            {
                Debug.Log("111");
                tipui.SetActive(true);
            }
        }    
     
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerinrange = false;
            if(tipui!= null)
                tipui.SetActive(false);
            if(targetui != null) 
                targetui.SetActive(false);
        }
    }
}
