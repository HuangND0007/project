using UnityEngine;

public class CloudMove : MonoBehaviour
{
    public float speed = 1f;      // 移动速度
    public float leftX = -12f;    // 左侧回收位置
    public float rightX = 12f;    // 右侧重生位置

    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        // 飘出左侧后，回到右侧继续循环
        if (transform.position.x < leftX)
        {
            Vector3 position = transform.position;
            position.x = rightX;
            transform.position = position;
        }
    }
}