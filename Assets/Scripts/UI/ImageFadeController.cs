using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImageFadeController : MonoBehaviour
{
    public static ImageFadeController instance;
    public Image image1;
    public Image image2;
    public float fadeDuration = 2.0f;
    private static Coroutine fadeCoroutine;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public static void FadeIn()
    {
        if (fadeCoroutine != null)
        {
            instance.StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = instance.StartCoroutine(FadeToAlpha(1)); // Fading to fully visible
    }

    public static void ResetFade()
    {
        if (instance.image1 != null)
            instance.image1.color = new Color(instance.image1.color.r, instance.image1.color.g, instance.image1.color.b, 0);
        if (instance.image2 != null)
            instance.image2.color = new Color(instance.image2.color.r, instance.image2.color.g, instance.image2.color.b, 0);
    }

    private static IEnumerator FadeToAlpha(float targetAlpha)
    {
        float time = 0;
        float startAlpha1 = instance.image1.color.a;
        float startAlpha2 = instance.image2.color.a;

        while (time < instance.fadeDuration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha1, targetAlpha, time / instance.fadeDuration);
            if (instance.image1 != null)
                instance.image1.color = new Color(instance.image1.color.r, instance.image1.color.g, instance.image1.color.b, alpha);
            if (instance.image2 != null)
                instance.image2.color = new Color(instance.image2.color.r, instance.image2.color.g, instance.image2.color.b, alpha);
            yield return null;
        }
    }
}
