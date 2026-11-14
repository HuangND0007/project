using System.Collections;
using UnityEngine;

public class HideProcess : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(setfalse());
    }

    IEnumerator setfalse()
    {
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }

}
