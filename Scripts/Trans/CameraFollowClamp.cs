using UnityEngine;

public class CameraFollowClamp : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.15f;
    public bool drawGizmos = true;

    [Header("地图三个角点标记")]
    public GameObject posi1;   // 左上 (-29.4, 16.4)
    public GameObject posi2;   // 右上 ( 45.4, 16.4)
    public GameObject posi3;   // 左下 (-29.4,-39.1)

    private Vector3 velocity;

    void Update()
    {
        if (target == null)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) target = p.transform;
        }
    }

    void LateUpdate()
    {
        if (target == null || posi1 == null || posi2 == null || posi3 == null) return;

        Camera cam = GetComponent<Camera>();

        // —— 每帧读角点：改 posi 立即生效 ——
        float ax = posi1.transform.position.x, ay = posi1.transform.position.y;
        float bx = posi2.transform.position.x, by = posi2.transform.position.y;
        float cx = posi3.transform.position.x, cy = posi3.transform.position.y;
        float minX = Mathf.Min(ax, bx, cx), maxX = Mathf.Max(ax, bx, cx);
        float minY = Mathf.Min(ay, by, cy), maxY = Mathf.Max(ay, by, cy);

        float halfH = cam.orthographicSize;
        float halfW = halfH * cam.aspect;

        Vector3 desired = Vector3.SmoothDamp(transform.position, target.position, ref velocity, smoothTime);
        desired.z = transform.position.z;

        desired.x = Mathf.Clamp(desired.x, minX + halfW, maxX - halfW);
        desired.y = Mathf.Clamp(desired.y, minY + halfH, maxY - halfH);

        transform.position = desired;
    }

    void OnDrawGizmos()
    {
        if (!drawGizmos || posi1 == null || posi2 == null || posi3 == null) return;

        float ax = posi1.transform.position.x, ay = posi1.transform.position.y;
        float bx = posi2.transform.position.x, by = posi2.transform.position.y;
        float cx = posi3.transform.position.x, cy = posi3.transform.position.y;
        float minX = Mathf.Min(ax, bx, cx), maxX = Mathf.Max(ax, bx, cx);
        float minY = Mathf.Min(ay, by, cy), maxY = Mathf.Max(ay, by, cy);

        var cam = GetComponent<Camera>();
        float halfH = cam != null ? cam.orthographicSize : 5f;
        float halfW = halfH * (cam != null ? cam.aspect : 16f / 9f);

        // 绿：地图边界   黄：相机中心活动范围（实时跟随 posi 变化）
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector3((minX + maxX) / 2f, (minY + maxY) / 2f, 0),
                            new Vector3(maxX - minX, maxY - minY, 0));
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(new Vector3((minX + maxX) / 2f, (minY + maxY) / 2f, 0),
                            new Vector3(Mathf.Max(0.01f, maxX - minX - halfW * 2f),
                                        Mathf.Max(0.01f, maxY - minY - halfH * 2f), 0));

        // 红线：相机 → 玩家，验证跟随目标对不对
        if (target != null && Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, target.position);
        }
    }
}