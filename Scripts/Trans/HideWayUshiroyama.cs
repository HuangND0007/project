using UnityEngine;

public class HideWayUshiroyama : MonoBehaviour
{
    public GameObject Hided;
    private void Awake()
    {
        print("Awake HideWayUshiroyama");
        if (EnvironmentManageer.Instance.getYear() >= 1 && EnvironmentManageer.Instance.getSeasonNum() >= 2 )
        {
            Hided.SetActive(true);
        }
    }
}
