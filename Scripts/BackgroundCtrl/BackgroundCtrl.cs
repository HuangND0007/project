using UnityEngine;

public class BackgroundCtrl : MonoBehaviour
{
    [SerializeField]
    private GameObject Day;
    [SerializeField]
    private GameObject Night;

    private static Environment env = Environment.Instance;

    private bool isDay()
    {
        return env.getTime() < 18f && env.getTime() > 6f;
    }

    private void TransDayNight()
    {
        if (isDay() && Night.activeSelf) { 
            Night.SetActive(false);
            Day.SetActive(true);
        } 
        if(!isDay() && Day.activeSelf)
        {
            Day.SetActive(false);
            Night.SetActive(true);
        }

    }

    private void Update()
    {
        TransDayNight();
    }
}
