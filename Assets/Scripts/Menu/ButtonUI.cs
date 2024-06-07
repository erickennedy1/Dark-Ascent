using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite novoSprite;

    private Image imageComponent;
    private Sprite spriteOriginal;

    void Start()
    {
        imageComponent = gameObject.GetComponent<Image>();
        spriteOriginal = imageComponent.sprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        imageComponent.sprite = novoSprite;
        SoundManager.Instance.PlaySound("UI_MouseEnter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        imageComponent.sprite = spriteOriginal;
    }

    public void ResetToOriginalSprite()
    {
        imageComponent.sprite = spriteOriginal;
    }
}
