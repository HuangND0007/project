using UnityEngine;

public class InitialOperation : MonoBehaviour
{
    protected KeyCode OperationKey = KeyCode.F;
    protected KeyCode ESCKey = KeyCode.Escape;

    protected bool PlayerInRange = false;

    public GameObject OperationUI;//Î¨Ò»µÄ²Ù×÷UI


    public void HideOperationUI()
    {
        if (OperationUI != null)
        {
            OperationUI.SetActive(false);
            ToMove();
        }
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

}
