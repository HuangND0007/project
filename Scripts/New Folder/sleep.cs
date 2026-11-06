using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class sleep : MonoBehaviour
{
    public static sleep Instance { get; private set; }

    public GameObject NotTired;//精力值相关UI提示
    public GameObject Tired;
    public VideoPlayer NewDay;
    private void UpdateUI() => TimeUImanage.Instance.UpdateUI();

    private void PlayVidio()
    {
        NewDay.Play();
        NewDay.playbackSpeed = 2;
    }
    private void TransSet(float time)
    {
        

        Tired.SetActive(true);
        Player.Instance.SetMoveFalse();
        StartCoroutine(SeepTips(2));

        Environment.Instance.SetTime(time);
        UpdateUI();
        PlayerManager.Instance.ModifyVitality(60f);

    }
    public void Home_Home()
    {
        if (Environment.Instance.getTime() > 18f || Environment.Instance.getTime() < 6f)
        {
            Environment.Instance.SetTime();
            StartCoroutine(SeepTips(4));
            Tired.SetActive(true);
            PlayVidio();
            UpdateUI();
            PlayerManager.Instance.ModifyVitality(60f);
            return;
        }
        if (PlayerManager.Instance.GetVitality() > 80 &&
            (Environment.Instance.getTime() < 18f) || Environment.Instance.getTime() > 6f)
        {
            NotTired.SetActive(true);
            StartCoroutine(SeepTips(1));
            return;
        }
        else
            TransSet(8f);
    }
    private IEnumerator SeepTips(int choice)
    {
        if (choice == 1)
        {
            yield return new WaitForSeconds(1.5f);
            NotTired.SetActive(false);
        }
        else
        {
            yield return new WaitForSeconds(4f);
            Tired.SetActive(false);
            Player.Instance.SetMoveTrue();
            NewDay.Stop();
        }
    }

}
