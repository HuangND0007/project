using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); 
        }
    }


    public Vector3 defaultPlayerPosition = new Vector3(0, 0, 0);

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene,LoadSceneMode mode)
    {
        transform.position = defaultPlayerPosition;

        var vcam = FindObjectOfType<CinemachineVirtualCamera>();
        if( vcam != null)
        {
            vcam.Follow = transform;
        }
        else
        {
            Debug.LogWarning("notfand  CinemachineVirtualCamera");
        }
    }

  
}
