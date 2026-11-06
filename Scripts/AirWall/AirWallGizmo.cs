using UnityEngine;

[ExecuteInEditMode]  // 在编辑器中也运行
[RequireComponent(typeof(BoxCollider2D))]
public class AirWallGizmo : MonoBehaviour
{
    [Header("调试颜色")]
    public Color gizmoColor = new Color(1f, 0.2f, 0.2f, 0.4f);

    private BoxCollider2D col;

    void OnDrawGizmos()
    {
        if (col == null) col = GetComponent<BoxCollider2D>();

        Gizmos.color = gizmoColor;

        // 绘制填充框
        Vector3 center = transform.position + (Vector3)col.offset;
        Vector3 size = new Vector3(col.size.x * transform.localScale.x,
                                   col.size.y * transform.localScale.y, 0.1f);
        Gizmos.DrawCube(center, size);

        // 绘制边框
        Gizmos.color = new Color(gizmoColor.r, gizmoColor.g, gizmoColor.b, 1f);
        Gizmos.DrawWireCube(center, size);
    }
}