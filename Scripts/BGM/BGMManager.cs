using UnityEngine;

public class BGMManager : MonoBehaviour
{
    private AudioSource audioSource;
    public static BGMManager _instance;
    public static BGMManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindFirstObjectByType<BGMManager>(); // 2022.3 可用
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
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;          // 循环播放
        audioSource.playOnAwake = false;
        audioSource.volume = 0.5f;
    }
   
    

    // 播放（传入 AudioClip）
    public void PlayBGM(AudioClip clip)
    {
        StopBGM(); // 先停止当前播放
        audioSource.clip = clip;
        audioSource.Play();
        print($"[BGMManager] PlayBGM: {clip.name}");
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