using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;

    private AudioSource audioSource;

    void Awake()
    {
        // 单例：跨场景不销毁
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;          // 循环播放
        audioSource.playOnAwake = false;
        audioSource.volume = 0.5f;
    }

    // 播放（传入 AudioClip）
    public void PlayBGM(AudioClip clip)
    {
        if (audioSource.clip == clip) return; // 同一首不重复播
        audioSource.clip = clip;
        audioSource.Play();
    }

    // 停止
    public void StopBGM()
    {
        audioSource.Stop();
    }

    // 暂停 / 恢复
    public void PauseBGM() => audioSource.Pause();
    public void ResumeBGM() => audioSource.UnPause();

    // 渐变控制音量（用于淡入淡出）
    public void SetVolume(float v) => audioSource.volume = Mathf.Clamp01(v);
}