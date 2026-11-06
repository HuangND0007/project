using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections;

public class SleepTipsUI : MonoBehaviour
{
    public static SleepTipsUI Instance {get; private  set; }
    public GameObject SleepUI;//Ë¯Ãßui
    public GameObject SkipUI;//Ìø¹ý¼¾½ÚUI

    public Button SleepButton;//Ë¯¾õ°´Å¥
    public Button SkipButton;//Ìø¹ý¼¾½Ú°´Å¥


    public VideoPlayer SkipSean;
    public GameObject SkipContainer;

    public KeyCode interkey = KeyCode.F;

    private bool playerinrange = false;
    private bool Clog = false;

    private void PlayVidio()
    {
        SkipSean.Play();
    }
    private void Update()
    {
        if (!Clog && playerinrange && Input.GetKeyDown(interkey))
        {
            if(SleepButton != null && SleepButton != null)
            {
                SleepUI.SetActive(true);
                SkipUI.SetActive(true);
            }
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private void Start()
    {
        SkipButton.onClick.AddListener(() =>
        {
            Debug.Log("SkipButton clicked");
            SkipContainer.SetActive(true);
            UISetFalse();
            PlayVidio();
            StartCoroutine(SeepTips());
        });
        SleepButton.onClick.AddListener(() =>
        {UISetFalse();
        });
    }
    private IEnumerator SeepTips()
    {
        yield return new WaitForSeconds(4f);
        Player.Instance.SetMoveTrue();
            SkipSean.Stop();
        SkipContainer.SetActive(false);
        Environment.Instance.SkipSeason();
        TimeUImanage.Instance.UpdateUI();

    }
    public void UISetFalse()
    {
        SkipUI.SetActive(false);
        SleepUI.SetActive(false);
    }

    public void TrueClog()
    { Clog = true; }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")){
            playerinrange=true;
            //if(SleepUI != null && SkipUI != null)
            //{
            //    SleepUI.SetActive(true);
            //    SkipUI.SetActive(true);
            //}
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
