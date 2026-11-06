using UnityEngine;

public class TransButton : MonoBehaviour
{
    public GameObject TransConfirUI;
    public GameObject TransButtonUI;
    public KeyCode interkey = KeyCode.F;

    private bool playerinrange = false;

    private void Update()
    {
        if (playerinrange && Input.GetKeyDown(interkey))
        {
            if (TransConfirUI != null && TransButtonUI != null)
            {
                TransConfirUI.SetActive(true);
                TransButtonUI.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerinrange = true;
            if (TransButtonUI != null)
            {
                TransButtonUI.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerinrange = false;
            if (TransButtonUI != null)
            {
                TransButtonUI.SetActive(false);
            }
        }
    }


}
