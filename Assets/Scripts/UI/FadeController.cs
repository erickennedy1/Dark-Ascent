using UnityEngine;
using System.Collections;

public class FadeController : MonoBehaviour
{
    public static FadeController instance;
    public Material targetMaterial; // Material que contém o shader a ser atualizado
    public float fadeDuration = 2.0f;
    private static float currentValue = 1.0f;
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

    public static void StartFade(bool toLow)
    {
        if (fadeCoroutine != null)
        {
            instance.StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = instance.StartCoroutine(toLow ? FadeFromOneToValue(0.08f) : FadeFromValueToOne(0.08f));
    }

    private static IEnumerator FadeFromOneToValue(float targetValue)
    {
        float time = 0;
        currentValue = 1.0f;

        while (time < instance.fadeDuration)
        {
            time += Time.deltaTime;
            currentValue = Mathf.Lerp(1.0f, targetValue, time / instance.fadeDuration);
            if (instance.targetMaterial != null)
            {
                instance.targetMaterial.SetFloat("_Fade", currentValue);
            }
            yield return null;
        }
        currentValue = targetValue;
    }

    private static IEnumerator FadeFromValueToOne(float startValue)
    {
        float time = 0;
        currentValue = startValue;

        while (time < instance.fadeDuration)
        {
            time += Time.deltaTime;
            currentValue = Mathf.Lerp(startValue, 1.0f, time / instance.fadeDuration);
            if (instance.targetMaterial != null)
            {
                instance.targetMaterial.SetFloat("_Fade", currentValue);
            }
            yield return null;
        }
        currentValue = 1.0f;
    }

    public static float GetCurrentFadeValue()
    {
        return currentValue;
    }
}
