using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 放在 SpawnPoint 空物体上，仅用于标记和可视化
public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private string pointId;
    [SerializeField] private Color gizmoColor = Color.green;

    public string PointId => pointId;

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawSphere(transform.position, 0.5f);
        Gizmos.DrawRay(transform.position, transform.forward * 2f);
    }
}