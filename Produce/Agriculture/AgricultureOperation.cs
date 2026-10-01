using UnityEngine;


public class AgricultureOperation : InitialOperation
{


    public GameObject IniCan;


    public void SetFalse()
    { //按钮调用，关闭初始UI
        IniCan.SetActive(false);
        ToMove();
    }
    


    public void HideIni() {  
        if (IniCan != null)
        {
            IniCan.SetActive(false);
            ToMove();
        }
    }
    public void HideOperateUI()
    {
        if (OperationUI != null)
        {
            OperationUI.SetActive(false);
            ToMove();
        }
    }

    private void Update()
    {
        ///F键打开UI进行操作
        if (PlayerInRange && Input.GetKeyDown(OperationKey))
        {
            NoMove();
            if (Agriculture.Instance.GetterArea() == 0) { 
                IniCan.SetActive(true);
            }
            else if (OperationUI != null)
            {
                if(IniCan != null)Destroy(IniCan);
                
                MonupulateUISetActive();
            }


        }
        ///ESC关闭UI不进行操作
        if(IniCan != null)
            if (IniCan.activeSelf && Input.GetKeyDown(ESCKey))
            {
                HideIni();
            }

        if (OperationUI != null)
            if (OperationUI.activeSelf && Input.GetKeyDown(ESCKey))
            { 
                HideOperateUI(); 
            }
    }




    private void MonupulateUISetActive()
    {
        OperationUI.SetActive(true);
        Agriculture.Instance.SetIni();
    }

    



}
