using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionManager : MonoBehaviour
{
    [Header("Configurações de Full Screen")]
    public GameObject iconFull;
    private bool fullScreen = false;

    [Header("Configurações de Sprites")]
    public Sprite spriteEsquerda;
    public Sprite spriteDireita;
    private SpriteRenderer spriteRenderer;
    private Sprite spriteOriginal;

    [Header("Configurações de Resolução")]
    public Sprite sprite1280;
    public Sprite sprite1600;
    public Sprite sprite1920;

    private int currentSpriteIndex;
    private List<Sprite> resolutionSprites;
    private List<Vector2Int> resolutions;

    void Start()
    {
        // Inicialização de Full Screen
        fullScreen = false;

        // Inicialização de Sprites
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteOriginal = spriteRenderer.sprite;

        // Inicialização de Resoluções
        resolutionSprites = new List<Sprite> { sprite1280, sprite1600, sprite1920 };
        resolutions = new List<Vector2Int> { new Vector2Int(1280, 720), new Vector2Int(1600, 900), new Vector2Int(1920, 1080) };
        currentSpriteIndex = 2; // Índice inicial
        spriteRenderer.sprite = resolutionSprites[currentSpriteIndex];
        SetResolution(currentSpriteIndex);
    }

    void OnMouseDown()
    {
        SoundManager.Instance.PlaySound("UI_MouseClick");

        if (gameObject.tag == "FullScreenToggle")
        {
            ToggleFullScreen();
        }
        else if (gameObject.tag == "Esquerda")
        {
            ResolucaoEsquerda();
        }
        else if (gameObject.tag == "Direita")
        {
            ResolucaoDireita();
        }
    }

    void OnMouseEnter()
    {
        SoundManager.Instance.PlaySound("UI_MouseEnter");

        if (gameObject.tag == "Esquerda")
        {
            SpriteParaEsquerda();
        }
        else if (gameObject.tag == "Direita")
        {
            SpriteParaDireita();
        }
    }

    void OnMouseExit()
    {
        ResetarSprite();
    }

    private void ToggleFullScreen()
    {
        fullScreen = !fullScreen;
        iconFull.SetActive(!fullScreen);
        Screen.fullScreen = fullScreen;
    }

    private void ResolucaoEsquerda()
    {
        if (currentSpriteIndex > 0)
            currentSpriteIndex--;
        else
            currentSpriteIndex = resolutionSprites.Count - 1;

        spriteRenderer.sprite = resolutionSprites[currentSpriteIndex];
        SetResolution(currentSpriteIndex);
    }

    private void ResolucaoDireita()
    {
        if (currentSpriteIndex < resolutionSprites.Count - 1)
            currentSpriteIndex++;
        else
            currentSpriteIndex = 0;

        spriteRenderer.sprite = resolutionSprites[currentSpriteIndex];
        SetResolution(currentSpriteIndex);
    }

    private void SetResolution(int index)
    {
        Vector2Int res = resolutions[index];
        Screen.SetResolution(res.x, res.y, Screen.fullScreen);
    }

    private void SpriteParaEsquerda()
    {
        spriteRenderer.sprite = spriteEsquerda;
    }

    private void SpriteParaDireita()
    {
        spriteRenderer.sprite = spriteDireita;
    }

    private void ResetarSprite()
    {
        spriteRenderer.sprite = spriteOriginal;
    }
}
