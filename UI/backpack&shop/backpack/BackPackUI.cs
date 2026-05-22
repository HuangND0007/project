using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;


public class BackPackUI : MonoBehaviour
{
    public static BackPackUI Instance { get; private set; }

    public CanvasGroup BackPackCanva;
    private bool isOpen = false;


    public void BackPackOpenandClose()//打开或关闭背包逻辑实现
    {
            if (isOpen)
            {
                BackPackCanva.alpha = 0;
                BackPackCanva.interactable = false;
                BackPackCanva.blocksRaycasts = false;
                isOpen = false;
            }
            else
            {
                BackPackCanva.alpha = 1;
                BackPackCanva.interactable = true;
                BackPackCanva.blocksRaycasts = true;
                isOpen = true;
            }

    }
    private void BackPack()//按下B键打开或关闭背包
    {
        if (UnitySceneManager.GetActiveScene().name!= "MainScene" && Input.GetKeyDown(KeyCode.B))
        {
            BackPackOpenandClose();
        }
    }
    public bool IsOpen()
    {
        return isOpen;
    }


    private void Start()
    {
        // 防御性检查
        if (BackPackCanva == null)
        {
            Debug.LogError("BackPackCanva 未赋值！请在 Inspector 中设置。", this);
            enabled = false;
            return;
        }
    }
    private void Update()
    {
        BackPack();
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            // 已存在实例，销毁新创建的
            Destroy(gameObject);
            return;
        }

            // 首次创建，设为单例并保留
            Instance = this;
            //DontDestroyOnLoad(gameObject);
    }
}
