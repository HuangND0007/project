using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
   public static SceneLoader Instance { get; private set; }

   void Awake()
   {
       if (Instance == null)
       {
           Instance = this;
           DontDestroyOnLoad(gameObject);
       }
       else
       {
           Destroy(gameObject);
       }

        StartCoroutine(LoadGameAsync());
    }

    [Header("永久UI")]
    [SerializeField] private string[] UIScenename = { "UIScene" };

    [Header("初始场景")]
    //[SerializeField] private string iniScenename = ; 

    private string currentScene = "MainScene";
    //private string pendingSpawnPoint;
    private bool IsTransing = false;

    public IEnumerator LoadGameAsync(string spawnPoint = null)
    {
        foreach (var scene in UIScenename)
        {
            if (!IsSceneLoaded(scene))
            {
                yield return SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            }
        }
        //yield return LoadLevelAsync(iniScenename, spawnPoint);
    }



    // ========== 切换关卡（核心方法）==========
    public IEnumerator SwitchLevelAsync(string levelName, string spawnPoint = null)
{
    Debug.Log($"[SceneLoader] === SwitchLevelAsync 入口: {levelName}, IsTransing={IsTransing}");
    
    if (IsTransing) 
    { 
        Debug.Log("切换场景 等待中...");
        yield return new WaitUntil(() => !IsTransing);
        Debug.Log("切换场景 等待结束");
    }
    
    if (!string.IsNullOrEmpty(currentScene) && IsSceneLoaded(currentScene))
    {
        IsTransing = true;
        Debug.Log($"[SceneLoader] 开始卸载: {currentScene}");

        SceneManager.UnloadSceneAsync(currentScene);
        Debug.Log($"[SceneLoader] 卸载完成");
    }
    
    Debug.Log($"[SceneLoader] 卸载场景Log: {currentScene}");

    Debug.Log($"[SceneLoader] 准备加载: {levelName}");
    LoadLevel(levelName, spawnPoint);
    Debug.Log("[SceneLoader] 切换完成");
        currentScene = levelName;
}
    public void LoadLevel(string levelName, string spawnPoint)
    {
        Debug.Log($"[SceneLoader] 开始加载: {levelName}");

        SceneManager.LoadScene(levelName, LoadSceneMode.Additive);

        StartCoroutine(WaitAndActivate(levelName, spawnPoint));
    }

    private IEnumerator WaitAndActivate(string levelName, string spawnPoint)
    {
        Scene scene;

        // 等待场景有效且已加载
        do
        {
            yield return null;
            scene = SceneManager.GetSceneByName(levelName);
        }
        while (!scene.IsValid() || !scene.isLoaded);

        SceneManager.SetActiveScene(scene);

        TeleportPlayer(spawnPoint);
    }
    // ========== 传送逻辑 ==========
    private void TeleportPlayer(string spawnPointName)
    {
        var player = FindPlayer();
        var spawnPos = FindSpawnPoint(spawnPointName);

        if (player != null && spawnPos.HasValue)
        {
            player.SetPositionAndRotation(spawnPos.Value.position, spawnPos.Value.rotation);
            Debug.Log($"[SceneLoader] 玩家传送到: {spawnPointName}");

            StartCoroutine(Wait());

            
        }
        else
        {
            Debug.LogError($"[SceneLoader] 传送失败: Player={player}, Spawn={spawnPointName}");
        }

        //pendingSpawnPoint = null;
        PlayerController_Trans.Instance?.FinishTeleport();
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSecondsRealtime(3f);
        var pro = GameObject.Find("process");
        if (pro != null)
        {
            Debug.Log("[SceneLoader] 关闭 process");
            pro.SetActive(false);
        }
        Debug.Log("[SceneLoader] 等待结束，传送完成");
    }

    private Transform FindPlayer()
    {
        // 玩家在永久场景中，直接查找
        var player = GameObject.FindWithTag("Player")?.transform;
        if (player == null)
            player = GameObject.Find("Player")?.transform;
        return player;
    }

    private (Vector3 position, Quaternion rotation)? FindSpawnPoint(string pointName)
    {
        // 优先查找场景中的 SpawnPoint 物体
        var points = GameObject.FindGameObjectsWithTag("SpawnPoint");
        foreach (var p in points)
        {
            if (p.name == pointName || p.name == $"Spawn_{pointName}")
            {
                return (p.transform.position, p.transform.rotation);
            }
        }

        // 回退：查找 LevelData 配置
        var levelData = Resources.Load<LevelData>($"LevelData/{currentScene}");
        if (levelData != null)
        {
            foreach (var sp in levelData.spawnPoints)
            {
                if (sp.pointName == pointName)
                {
                    return (sp.position, Quaternion.Euler(sp.rotation));
                }
            }
        }

        // 最终回退：场景原点
        return (Vector3.zero, Quaternion.identity);
    }

    private bool IsSceneLoaded(string sceneName)
    {
        return SceneManager.GetSceneByName(sceneName).isLoaded;
    }
    public void reIsTrans()
    {
        IsTransing = false;
    }
}