using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

public class Player : MonoBehaviour
{

    public static Player Instance { set;  get; }
    public Rigidbody2D rb;

    private Vector3 pendingPosition;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else Destroy(gameObject);
    }

    private void Update()
    {

        if ("MainScene".Equals(UnitySceneManager.GetActiveScene().name)) return;
        float movex = Input.GetAxis("Horizontal");
        float movey = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(movex, movey);
        transform.Translate(movement * PlayerManager.Instance.Speed * Time.deltaTime);
    }
    public Vector3 GetPosition()
    {
        return transform.position;
    }
    

    public void ModifyPosition(string sceneName, float x, float y, float z ,string preScene )
    {
        Vector3 targetPosition = new Vector3(x, y, z);
        pendingPosition = targetPosition;
        SceneManager.LoadScene(sceneName);
        Debug.Log("位置"+transform.position);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 场景完全加载后才设置位置
        transform.position = pendingPosition;
        Debug.Log($"已传送到: {pendingPosition}");
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
