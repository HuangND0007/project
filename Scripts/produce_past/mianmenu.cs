using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mianmenu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("Home");
    }
    public void Quit ()
    {
        Application.Quit();
    }
}
