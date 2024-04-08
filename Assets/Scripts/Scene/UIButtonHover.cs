using UnityEngine;
using UnityEngine.EventSystems; 

public class UIButtonHover : MonoBehaviour, IPointerEnterHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        UIHoverSoundManager.Instance.PlayHoverSound();
    }
}
