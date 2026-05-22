using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Exit : MonoBehaviour
{
    public CanvasGroup escCanva;
    private bool isOpen = false;

    private void Escing()//按下ESC键打开或关闭菜单
    {
        if(BackPackUI.Instance == null)
        {
            Debug.Log("背包UI出了问题");return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {

            if (BackPackUI.Instance.IsOpen())//如果背包界面打开了，按下ESC键应该先关闭背包界面
            {
                BackPackUI.Instance.BackPackOpenandClose();
                return;
            }
            if (isOpen)
            {
                escCanva.alpha = 0;
                escCanva.interactable = false;
                escCanva.blocksRaycasts = false;
                isOpen = false;
            }
            else
            {
                escCanva.alpha = 1;
                escCanva.interactable = true;
                escCanva.blocksRaycasts = true ;
                isOpen = true;
            }
        }
    }

    private void Update()
    {
        Escing();
    }

    private void Awake()
    {
        //DontDestroyOnLoad(gameObject);
    }

}
