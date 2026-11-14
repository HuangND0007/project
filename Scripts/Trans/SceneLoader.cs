using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
   public static SceneLoader _instance;
    public static SceneLoader Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindFirstObjectByType<SceneLoader>(); // 2022.3 可用
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
        if (IsTransing)
            yield return new WaitUntil(() => !IsTransing);

        IsTransing = true;
        try
        {
            // 1. 加载新场景
            var loadOp = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
            if (loadOp == null)
            {
                Debug.LogError($"[SceneLoader] 加载失败: {levelName}，请确认它在 Build Settings 里");
                yield break;
            }
            yield return loadOp;

            // 2. 激活新场景
            var scene = SceneManager.GetSceneByName(levelName);
            if (scene.IsValid() && scene.isLoaded)
                SceneManager.SetActiveScene(scene);

            // 3. 立即更新记录（放在传送之前，防止异常导致失步）
            currentScene = levelName;

            // 4. 卸载除新场景、常驻 UI 场景之外的所有已加载场景（自愈逻辑）
            for (int i = SceneManager.sceneCount - 1; i >= 0; i--)
            {
                var s = SceneManager.GetSceneAt(i);
                if (!s.isLoaded || s.name == levelName) continue;

                bool isPermanent = false;
                foreach (var ui in UIScenename)
                    if (s.name == ui) { isPermanent = true; break; }
                if (isPermanent) continue;

                yield return SceneManager.UnloadSceneAsync(s);
                Debug.Log($"[SceneLoader] 已卸载: {s.name}");
            }

            // 5. 传送单独包一层，失败不影响场景切换结果
            try { TeleportPlayer(spawnPoint); }
            catch (System.Exception e) { Debug.LogError($"[SceneLoader] 传送异常: {e}"); }

            if (Player.Instance != null)
                Player.Instance.SetMoveTrue();
        }
        finally
        {
            IsTransing = false;
        }
    }

  
    // ========== 传送逻辑 ==========
    private void TeleportPlayer(string spawnPointName)
    {
        var player = FindPlayer();
        var spawnPos = FindSpawnPoint(spawnPointName);

        if (player != null && spawnPos.HasValue)
        {
            player.SetPositionAndRotation(spawnPos.Value.position, spawnPos.Value.rotation);

            var prb = player.GetComponent<Rigidbody2D>();
            if (prb != null)
            {
                prb.position = spawnPos.Value.position;   // 关键：同步物理位置，否则被弹回
                prb.velocity = Vector2.zero;
            }

            Debug.Log($"[SceneLoader] 玩家传送到: {spawnPointName} @ {spawnPos.Value.position}");
            StartCoroutine(Wait());
        }
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
            if (p.name == pointName || p.name == $"Spawn_{pointName}" || p.name == $"SpawnPoint_{pointName}")
                return (p.transform.position, p.transform.rotation);
        }

        // 找不到时把现场打印出来，方便排查
        var names = new System.Text.StringBuilder();
        foreach (var p in points) names.Append(p.name).Append(" | ");
        Debug.LogWarning($"[SceneLoader] 找不到出生点 '{pointName}'，当前已加载的 SpawnPoint: {names}");

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