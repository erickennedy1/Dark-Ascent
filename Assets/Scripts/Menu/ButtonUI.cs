using UnityEngine;
using UnityEngine.UI; // Importa o namespace necess�rio para trabalhar com UI.

public class ButtonUI : MonoBehaviour
{
    public Sprite novoSprite;

    private Image imageComponent; // Alterado de SpriteRenderer para Image.
    private Sprite spriteOriginal;

    void Start()
    {
        imageComponent = gameObject.GetComponent<Image>(); // Obt�m o componente Image ao inv�s de SpriteRenderer.
        spriteOriginal = imageComponent.sprite; // Armazena o sprite original.
    }

    void OnMouseEnter()
    {
        imageComponent.sprite = novoSprite; // Muda o sprite.
        SoundManager.Instance.PlaySound(SoundManager.Instance.mouseEnter); // Assume que voc� tem um gerenciador de som.
    }

    void OnMouseExit()
    {
        imageComponent.sprite = spriteOriginal; // Retorna ao sprite original.
    }

    public void ResetToOriginalSprite()
    {
        imageComponent.sprite = spriteOriginal; // M�todo para resetar o sprite manualmente.
    }
}
