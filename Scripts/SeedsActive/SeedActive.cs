using UnityEngine;

public class SeedActive : MonoBehaviour
{
    [SerializeField]
    private GameObject SeedsContainer;

    private void SeedSetActive()
    {
        if(AgricultureProduce.Instance.GetterSeededNum() > 0 )
        { 
            SeedsContainer.SetActive( true );
        }
    }

    private void Update()
    {
        SeedSetActive();
    }
}

