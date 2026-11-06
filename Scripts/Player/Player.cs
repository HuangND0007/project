using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

public class Player : MonoBehaviour
{
    public static Player Instance { set; get; }
    public Rigidbody2D rb;
    private bool Move = true;

    private Vector3 pendingPosition;

    public void SetMoveTrue()
    {
        Move = true;
    }
    public void SetMoveFalse()
    {
        Move = false;
    }

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
        // Update 里只处理非移动逻辑（如场景判断等）
        if ("MainScene".Equals(UnitySceneManager.GetActiveScene().name) || !Move ) return;
    }

    void FixedUpdate()
    {
        if ("MainScene".Equals(UnitySceneManager.GetActiveScene().name)|| !Move ) return;

        // 读取输入
        float movex = Input.GetAxis("Horizontal");
        float movey = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(movex, movey);

        // 如果没有输入，直接返回
        if (movement.sqrMagnitude < 0.001f) return;

        // 关键：如果 rb 没有赋值，回退到 Translate 并报警告
        if (rb == null)
        {
            transform.Translate(movement * PlayerManager.Instance.Speed * Time.fixedDeltaTime);
            Debug.LogWarning("Rigidbody2D (rb) 未赋值！空气墙可能失效。请在 Inspector 中拖拽赋值。");
            return;
        }

        // 物理驱动移动 —— 空气墙此时会生效
        Vector2 newPos = rb.position + movement * PlayerManager.Instance.Speed * Time.fixedDeltaTime;
        rb.MovePosition(newPos);
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void ModifyPosition(string sceneName, float x, float y, float z, string preScene)
    {
        Vector3 targetPosition = new Vector3(x, y, z);
        pendingPosition = targetPosition;
        SceneManager.LoadScene(sceneName);
        Debug.Log("位置" + transform.position);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        transform.position = pendingPosition;
        Debug.Log($"已传送到: {pendingPosition}");
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}