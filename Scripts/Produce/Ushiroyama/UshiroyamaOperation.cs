
using UnityEngine;

public class UshiroyamaOperation : InitialOperation
{
    
    public void HideIni()
    {
        if (OperationUI != null)
        {
            OperationUI.SetActive(false);
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
            print("F键按下，打开操作UI");
            NoMove();
            MonupulateUISetActive();
            


        }
        ///ESC关闭UI不进行操作
        if (OperationUI != null)
            if (OperationUI.activeSelf && Input.GetKeyDown(ESCKey))
            {
                HideOperateUI();
                ToMove();
            }
    }




    private void MonupulateUISetActive()
    {
        OperationUI.SetActive(true);
    }



}
