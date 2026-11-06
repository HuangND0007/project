using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerData", menuName = "ScriptableObjects/PlayerData")]
public class _PlayerData : ScriptableObject
{
    [Header("玩家数据信息")]
    [Tooltip("玩家名字")]
    public string Name = "初始名称";
    [Tooltip("速度")]
    public float Speed;
    [Tooltip("余额")]
    public float Money;
    [Tooltip("精力")]
    public float Vitality;
    [Tooltip("心情")]
    public float Mood;
    [Tooltip("场景")]
    public string Location;
    [Tooltip("位置")]
    public float X, Y, Z;

    public string GetSaveId() => "Player";
}
