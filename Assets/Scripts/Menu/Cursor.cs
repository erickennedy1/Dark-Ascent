using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class CustomCursor : MonoBehaviour
{
    public Texture2D cursorTex;
    public float cursorSize = 1.0f;
    private Texture2D resizedCursor;

    void Awake()
    {
        resizedCursor = ResizeTexture(cursorTex, cursorSize);
        Cursor.SetCursor(resizedCursor, Vector2.zero, CursorMode.ForceSoftware);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Cursor.SetCursor(resizedCursor, Vector2.zero, CursorMode.ForceSoftware);
    }

    Texture2D ResizeTexture(Texture2D source, float scale)
    {
        int newWidth = Mathf.CeilToInt(source.width * scale);
        int newHeight = Mathf.CeilToInt(source.height * scale);

        Texture2D result = new Texture2D(newWidth, newHeight, source.format, false);

        for (int y = 0; y < newHeight; y++)
        {
            for (int x = 0; x < newWidth; x++)
            {
                Color color = source.GetPixelBilinear((float)x / newWidth, (float)y / newHeight);
                result.SetPixel(x, y, color);
            }
        }
        result.Apply(); 

        return result;
    }
}