using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoVisibility : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public float duration = 5.0f;

    public List<GameObject> objects = new List<GameObject>();

    void Start()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        StartCoroutine(FadeIn());
        StartCoroutine(ActivateObjects());
    }

    IEnumerator FadeIn()
    {
        float currentTime = 0.0f;
        Color startColor = spriteRenderer.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1.0f);

        while (currentTime < duration)
        {
            float alpha = Mathf.Lerp(0.0f, 1.0f, currentTime / duration);
            spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            currentTime += Time.deltaTime;
            yield return null;
        }

        spriteRenderer.color = endColor;
    }

    IEnumerator ActivateObjects()
    {
        yield return new WaitForSeconds(3.5f);  

        foreach (GameObject obj in objects)
        {
            if (obj != null)
            {
                obj.SetActive(true);  
            }
        }
    }
}
