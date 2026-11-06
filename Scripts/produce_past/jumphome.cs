using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class jumphome : MonoBehaviour
{
    public void OnLogButterClick()
    {
        SceneManager.LoadScene("Home");
    }
}
