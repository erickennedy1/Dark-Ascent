using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionNumberChange : MonoBehaviour
{
    public Sprite sprite1280;
    public Sprite sprite1600;
    public Sprite sprite1920;

    private SpriteRenderer spriteRenderer;

    private int currentSpriteIndex;

    private List<Sprite> sprites;
    private List<Vector2Int> resolutions;  

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        sprites = new List<Sprite> { sprite1280, sprite1600, sprite1920 };
        resolutions = new List<Vector2Int> { new Vector2Int(1280, 720), new Vector2Int(1600, 900), new Vector2Int(1920, 1080) }; 
        currentSpriteIndex = 2;
        spriteRenderer.sprite = sprites[currentSpriteIndex];
        SetResolution(currentSpriteIndex);  
    }

    private void SetResolution(int index)
    {
        Vector2Int res = resolutions[index];
        Screen.SetResolution(res.x, res.y, Screen.fullScreen);  
    }

    public void ResolucaoEsquerda()
    {
        if (currentSpriteIndex > 0)
            currentSpriteIndex--;
        else
            currentSpriteIndex = sprites.Count - 1;

        spriteRenderer.sprite = sprites[currentSpriteIndex];
        SetResolution(currentSpriteIndex); 
    }

    public void ResolucaoDireita()
    {
        if (currentSpriteIndex < sprites.Count - 1)
            currentSpriteIndex++;
        else
            currentSpriteIndex = 0;

        spriteRenderer.sprite = sprites[currentSpriteIndex];
        SetResolution(currentSpriteIndex);
    }
}
