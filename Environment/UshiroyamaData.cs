using UnityEngine;

public class UshiroyamaData : MonoBehaviour
{

    private float Integrality;
    private float Trees;


    
    public void IntegralityModify()
    {
        Integrality += ( Trees - 0.60f ) * 1.5f; 
        if(Integrality > 1f) Integrality = 1f;
    }

    // Start is called before the first frame update
     void Start()
     {
        
     }

    // Update is called once per frame
    void Update()
    {
        
    }
}
