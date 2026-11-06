using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TimeUImanage : MonoBehaviour
{
    public static TimeUImanage Instance { get; private set; }
    public GameObject[] UIText;

    [Header("UI组件")]
    public CanvasGroup canvasGroup;
    public Image image;
    public void  UpdateUI()
    {
        if (Environment.Instance.getYear() == 0) UIText[0].GetComponent<TMP_Text>().text = "预备年";
        else
            UIText[0].GetComponent<TMP_Text>().text = "第" + Environment.Instance.getYear() + "年"; 
        UIText[1].GetComponent<TMP_Text>().text = Environment.Instance.getSeason() + "季"; 
        UIText[2].GetComponent<TMP_Text>().text = "第" + Environment.Instance.getDay() + "日";
        {   float time = Environment.Instance.getTime();
            int h = (int)time;
            int m = (int)(60 * (time % 1));

            string timestr = h.ToString() + ":" + ( m >= 10 ? m.ToString() : "0" + m.ToString() ) ;


            UIText[3].GetComponent<TMP_Text>().text = timestr ;
        }
        Debug.Log("UI更新完成");     
    }

    
    public void ShowUI()// 显示UI
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
    }

    
    public void HideUI()// 隐藏UI
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }

    
    public void OnEnterGameScene()// 切换场景后调用此方法显示UI
    {
        ShowUI();
    }

private void Awake()
    {
        if (Instance != null && Instance != this )
        {
            //DontDestroyOnLoad(transform.root.gameObject);
            Instance = this;
            return;
        }
        Instance = this;
        HideUI();
    }
}
