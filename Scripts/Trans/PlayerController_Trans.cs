using UnityEngine;

public class PlayerController_Trans : MonoBehaviour
{
    public static PlayerController_Trans _instance;
    public static PlayerController_Trans Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindFirstObjectByType<PlayerController_Trans>(); // 2022.3 可用
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
    }

    [SerializeField] private CharacterController charController;
    private bool isTeleporting;

   

    public void SetPositionAndRotation(Vector3 pos, Quaternion rot)
    {
        // 先禁用 CharacterController，直接修改 Transform
        if (charController != null) charController.enabled = false;

        transform.SetPositionAndRotation(pos, rot);

        if (charController != null) charController.enabled = true;
    }


    public bool IsTeleporting()
    {
        return isTeleporting;
    }
    public void PrepareTeleport()
    {
        isTeleporting = true;
        // 禁用输入、播放特效等
        // ...
    }

    public void FinishTeleport()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
