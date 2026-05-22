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
            if(NPCDialogue.Instance != null && NPCDialogue.Instance.IfDialogueActive())//如果对话界面打开了，取消按下ESC键的功能
                return;

            if (BackPackUI.Instance.IsOpen())//如果背包界面打开了，按下ESC键应该先关闭背包界面
            {
                Debug.Log("当前对话激活状态为" + NPCDialogue.Instance.IfDialogueActive());
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
