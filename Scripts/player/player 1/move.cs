using UnityEngine;

public class move : MonoBehaviour
{
    public float speed = 5f;

    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // 防止移动时旋转，避免碰撞后翻滚
        rb.freezeRotation = true;

        // 如果速度较快，建议开启连续检测，进一步降低穿墙概率
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    void Update()
    {
        // 1. 在 Update 中读取输入（输入刷新频率与帧率一致）
        float movex = Input.GetAxis("Horizontal");
        float movey = Input.GetAxis("Vertical");
        movement = new Vector2(movex, movey);
    }

    void FixedUpdate()
    {
        // 2. 在 FixedUpdate 中用物理引擎移动（与物理刷新频率一致）
        Vector2 newPosition = rb.position + movement * speed * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);
    }
}