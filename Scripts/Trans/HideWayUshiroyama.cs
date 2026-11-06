using UnityEngine;

public class HideWayUshiroyama : MonoBehaviour
{
    public GameObject Hided;
    private void Awake()
    {
        print("Awake HideWayUshiroyama");
        if (Environment.Instance.getYear() >= 1 && Environment.Instance.getSeasonNum() >= 2 )
        {
            Hided.SetActive(true);
        }
    }
}
