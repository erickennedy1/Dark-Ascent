using UnityEngine;
using UnityEngine.UI; // Importa o namespace necessário para trabalhar com UI.

public class ButtonUI : MonoBehaviour
{
    public Sprite novoSprite;

    private Image imageComponent; // Alterado de SpriteRenderer para Image.
    private Sprite spriteOriginal;

    void Start()
    {
        imageComponent = gameObject.GetComponent<Image>(); // Obtém o componente Image ao invés de SpriteRenderer.
        spriteOriginal = imageComponent.sprite; // Armazena o sprite original.
    }

    void OnMouseEnter()
    {
        imageComponent.sprite = novoSprite; // Muda o sprite.
        SoundManager.Instance.PlaySound(SoundManager.Instance.mouseEnter); // Assume que você tem um gerenciador de som.
    }

    void OnMouseExit()
    {
        imageComponent.sprite = spriteOriginal; // Retorna ao sprite original.
    }

    public void ResetToOriginalSprite()
    {
        imageComponent.sprite = spriteOriginal; // Método para resetar o sprite manualmente.
    }
}
