using UnityEngine;

public class ResolutionChange : MonoBehaviour
{
    public Sprite spriteEsquerda;
    public Sprite spriteDireita;
    private SpriteRenderer spritePadrao;
    private Sprite spriteOriginal;


    public ResolutionMouseDetector resolutionDetector;

    void Start()
    {
        spritePadrao = GetComponent<SpriteRenderer>();
        spriteOriginal = spritePadrao.sprite;
    }

    public void SpriteParaEsquerda()
    {
        spritePadrao.sprite = spriteEsquerda;
    }

    public void SpriteParaDireita()
    {
        spritePadrao.sprite = spriteDireita;
    }

    public void ResetarSprite()
    {
        spritePadrao.sprite = spriteOriginal;
    }

}
