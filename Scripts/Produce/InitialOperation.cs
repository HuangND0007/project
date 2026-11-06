using UnityEngine;
using Unity.UI;
using UnityEngine.UI;

public class InitialOperation : MonoBehaviour
{
    public static InitialOperation Instance { get; private set; }

    protected KeyCode OperationKey = KeyCode.F;
    protected KeyCode ESCKey = KeyCode.Escape;

    protected bool PlayerInRange = false;

    public GameObject OperationUI;//唯一的操作UI


    public void HideOperationUI()
    {
        Debug.Log("ESC被点击");
        if (OperationUI != null && OperationUI.activeSelf)
        {
            OperationUI.SetActive(false);
            ToMove();
        }
        InitialProduce.Instance.HideIni();
        Exit.Instance.FalseEscChoke();
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInRange = true;
        }
    }

    protected void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInRange = false;
        }
    }



    protected void NoMove() => Player.Instance.SetMoveFalse();
    protected void ToMove() => Player.Instance.SetMoveTrue();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private void Update()
    {
        if (Input.GetKey(ESCKey))HideOperationUI();
    }
}
