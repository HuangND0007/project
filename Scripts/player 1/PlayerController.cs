using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    [Header("地图范围（相机活动边界，即空气墙）")]
    public Vector2 mapCenter = Vector2.zero;
    public Vector2 mapSize = new Vector2(60f, 40f);

    [Header("有地图碰撞体可拖这里，自动取其范围")]
    public Collider2D mapBounds;

    private PolygonCollider2D airWall;

    private void Awake()
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
    }

    public Vector3 defaultPlayerPosition = new Vector3(0, 0, 0);

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        transform.position = defaultPlayerPosition;

        var vcam = FindObjectOfType<CinemachineVirtualCamera>();
        if (vcam == null)
        {//不再需要
            //Debug.LogWarning("notfand  CinemachineVirtualCamera");
            return;
        }

        vcam.Follow = transform;
        SetupCameraAirWall(vcam);  
    }

    /// <summary>
    /// 生成不可见空气墙，并绑定给 Cinemachine Confiner，阻挡相机出界
    /// </summary>
    void SetupCameraAirWall(CinemachineVirtualCamera vcam)
    {
        // 1. 生成（或重建）空气墙
        if (airWall == null)
        {
            var wallObj = new GameObject("CameraAirWall");
            airWall = wallObj.AddComponent<PolygonCollider2D>();
            airWall.isTrigger = true;   // 触发器，不会挡住玩家移动
        }

        Bounds b = mapBounds != null ? mapBounds.bounds
                                     : new Bounds((Vector3)mapCenter, (Vector3)mapSize);
        airWall.pathCount = 1;
        airWall.SetPath(0, new[]
        {
            new Vector2(b.min.x, b.min.y),
            new Vector2(b.max.x, b.min.y),
            new Vector2(b.max.x, b.max.y),
            new Vector2(b.min.x, b.max.y)
        });

        // 2. 给虚拟相机挂 Confiner（Cinemachine 2.x 用这个组件）
        var confiner = vcam.GetComponent<CinemachineConfiner>();
        if (confiner == null)
            confiner = vcam.gameObject.AddComponent<CinemachineConfiner>();

        confiner.m_BoundingShape2D = airWall;
        confiner.InvalidatePathCache();   // 2.x 是 InvalidatePathCache，不是 InvalidateCache
    }
}