using UnityEngine;

public class iniinfor : MonoBehaviour
{
    public GameObject ini;
    public GameObject Guild;

    private void Set()
    {
        if ("³õÊ¼Ãû³Æ".Equals(PlayerManager.Instance.GetName()))
        {
            ini.GetComponent<NPCDialogue>().ForceStartDialogue();
        }
        else
        {
            Guild.SetActive(false);
        }
    }
    private void Awake()
    {
        Set();   
    }


}