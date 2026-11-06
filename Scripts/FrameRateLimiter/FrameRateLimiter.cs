
using UnityEngine;

public class FrameRateLimiter : MonoBehaviour
{
    private void Awake()
    {
        // 关闭垂直同步，避免其覆盖目标帧率设置
        QualitySettings.vSyncCount = 0;

        // 将目标帧率设为 60 FPS
        Application.targetFrameRate = 60;
        
    }
}