using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainServierBGM : MonoBehaviour
{
    AudioClip Main;
    private void Awake()
    {
        Main = Resources.Load<AudioClip>("BGMS/Easy Going Medieval Tavern - Loop");
        if (Main == null) { 
        print("[MainServierBGM] Main BGM not found!");
            return;
        }
        BGMManager.Instance.PlayBGM(Main);
    }
}
