using UnityEngine;

public class IniPanelDisAvtive : MonoBehaviour
{
    [SerializeField]
    private GameObject pa;
    void Update()
    {
        if (this.gameObject.activeSelf && Input.GetKeyDown(KeyCode.F)) { 
            this.gameObject.SetActive(false);
            pa.SetActive(false);
        Player.Instance.SetMoveTrue();
        }

    }
}
