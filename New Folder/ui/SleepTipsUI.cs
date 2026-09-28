using UnityEngine;
using UnityEngine.UI;

public class SleepTipsUIi : MonoBehaviour
{
    public GameObject SleepUI;//Ë¯Ãßui
    public GameObject SkipUI;//Ìø¹ý¼¾½ÚUI

    public Button SleepButton;//Ë¯¾õ°´Å¥
    public Button SkipButton;//Ìø¹ý¼¾½Ú°´Å¥

    

    public KeyCode interkey = KeyCode.F;

    private bool playerinrange = false;

    private void Update()
    {
        if (playerinrange && Input.GetKeyDown(interkey))
        {
            if(SleepButton != null && SleepButton != null)
            {
                SleepUI.SetActive(false);
                SkipUI.SetActive(false);
            }
        }
    }

    private void Start()
    {
        SkipButton.onClick.AddListener(() =>
        {
            Debug.Log("SkipButton clicked");
            Environment.Instance.SkipSeason();
            TimeUImanage.Instance.UpdateUI();
        });
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")){
            playerinrange=true;
            if(SleepUI != null && SkipUI != null)
            {
                SleepUI.SetActive(true);
                SkipUI.SetActive(true);
            }
        }    
     
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerinrange = false;
            if (SleepUI != null && SkipUI != null) {
                SleepUI.SetActive(false); 
                SkipUI.SetActive(false);
            }
        }
    }
}
