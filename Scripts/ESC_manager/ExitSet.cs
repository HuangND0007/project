using UnityEngine;

public class ExitSet : MonoBehaviour
{
    public static ExitSet Instance { get; private set; }
    public void QuitGame()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #endif
        Application.Quit();

    }
}
