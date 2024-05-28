using UnityEngine;

public class Button : MonoBehaviour
{
    public Sprite novoSprite;

    private SpriteRenderer spritePadrao;   
    private Sprite spriteOriginal;


    void Start()
    {
        spritePadrao = gameObject.GetComponent<SpriteRenderer>();
        spriteOriginal = spritePadrao.sprite;
    }

    void OnMouseEnter()
    {
        spritePadrao.sprite = novoSprite;
        SoundManager.Instance.PlaySound("UI_MouseEnter");
    }

    void OnMouseExit()
    {
        spritePadrao.sprite = spriteOriginal;
    }

    public void ResetToOriginalSprite()
    {
        spritePadrao.sprite = spriteOriginal;
    }
}
