using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Active : MonoBehaviour
{
    public GameObject obj;
    public void active() 
    {
        obj.SetActive(true);
    }
}
