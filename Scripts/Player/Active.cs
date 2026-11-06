using UnityEngine;

public class Active : MonoBehaviour
{
    public GameObject obj;
    public void active() 
    {
        obj.SetActive(true);
    }
}
