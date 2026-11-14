using System;
using Unity.VisualScripting;

//using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

public class Player : MonoBehaviour
{
    public static Player _instance;
    public static Player Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindFirstObjectByType<Player>(); // 2022.3 可用
            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);   // MainScene 重新加载带进来的副本，销毁
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
        RightObject.SetActive(true);
        LeftObject.SetActive(false);
    }


    public GameObject PlayerPerson;
    public int Face = 1;
    public Rigidbody2D rb;
    private bool Move = true;
    public GameObject LeftObject;
    public GameObject RightObject;
    public Animator Left;
    public Animator Right;

    //private Vector3 pendingPosition;

    public void SetMoveTrue()
    {
        Move = true;
    }
    public void SetMoveFalse()
    {
        Move = false;
    }



    private void Update()
    {
        // Update 里只处理非移动逻辑（如场景判断等）
        if ("MainScene".Equals(UnitySceneManager.GetActiveScene().name) || !Move) return;
    }

    void FixedUpdate()
    {
        if ("MainScene".Equals(UnitySceneManager.GetActiveScene().name) || !Move) return;

        // 读取输入
        float movex = Input.GetAxis("Horizontal");
        float movey = Input.GetAxis("Vertical");

        if (movex < 0 && RightObject.activeSelf)
        {

            RightObject.SetActive(false);
            LeftObject.SetActive(true);
        }
        else if (movex > 0 && LeftObject.activeSelf)
        {
            LeftObject.SetActive(false);
            RightObject.SetActive(true);
        }
        if (LeftObject.activeSelf)
        {
            Left.SetFloat("horizontal", Mathf.Abs(movex));
            Left.SetFloat("vertical", Mathf.Abs(movey));
        }
        else if (RightObject.activeSelf)
        {
            Right.SetFloat("horizontal", Mathf.Abs(movex));
            Right.SetFloat("vertical", Mathf.Abs(movey));
        }
        Vector2 movement = new Vector2(movex, movey);

        // 如果没有输入，直接返回
        if (movement.sqrMagnitude < 0.001f)
        {

            return;
        }

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

    private void Flip()
    {
        Debug.Log("翻转");
        Face *= -1;
        PlayerPerson.transform.localScale = new Vector3
            (PlayerPerson.transform.localScale.x * -1,
            PlayerPerson.transform.localScale.y, PlayerPerson.transform.localScale.z);
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }



    public void Teleport(Vector3 pos)
    {
        transform.position = pos;
        if (rb != null)
        {
            rb.position = pos;
            rb.velocity = Vector2.zero;
        }


    }
}