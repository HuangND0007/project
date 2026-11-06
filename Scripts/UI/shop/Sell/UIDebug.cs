using UnityEngine;
using UnityEngine.EventSystems;

public class UIDebug : MonoBehaviour, IPointerDownHandler, IPointerClickHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log($"PointerDown£º{name}", this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"Click£º{name}", this);
    }
}