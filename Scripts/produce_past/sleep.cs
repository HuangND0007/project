using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sleep : MonoBehaviour
{
    public static sleep Instance { get; private set; }

    public GameObject NotTired;//精力值相关UI提示
    public GameObject Tired;
    private void UpdateUI() => TimeUImanage.Instance.UpdateUI();

    private void TransSet(float time)//统一化设置场景和时间的方法，方便调用
    {
        if(PlayerManager.Instance.GetVitality() > 80)
        {
            NotTired.SetActive(true);
            StartCoroutine(SeepTips(1));
            return;
        }
        Tired.SetActive(true);
        Player.Instance.SetMoveFalse();
        StartCoroutine(SeepTips(2));

        Environment.Instance.SetTime(time);
        UpdateUI();
        PlayerManager.Instance.ModifyVitality(60f);

    }
    public void Home_Home() => TransSet(8f);

    private IEnumerator SeepTips(int choice)
    {
        if (choice == 1)
        {
            yield return new WaitForSeconds(1.5f);
            NotTired.SetActive(false);
        }
        else
        {
            yield return new WaitForSeconds(3f);
            Tired.SetActive(false);
            Player.Instance.SetMoveTrue();
        }
    }

}
