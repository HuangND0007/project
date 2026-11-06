using UnityEngine;

public class PlayerController_Trans : MonoBehaviour
{
    public static PlayerController_Trans Instance { get; private set; }

    [SerializeField] private CharacterController charController;
    private bool isTeleporting;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

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
