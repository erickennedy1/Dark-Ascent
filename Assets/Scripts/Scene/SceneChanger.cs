using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneChanger : MonoBehaviour
{
    public Image fadeImage; 
    public float fadeDuration = 1.0f; 

    private bool isFading = false;

    public void ChangeScene(string sceneName)
    {
        if (!isFading)
        {
            StartCoroutine(FadeAndChangeScene(sceneName));
        }
    }

    IEnumerator FadeAndChangeScene(string sceneName)
    {
        isFading = true;

        fadeImage.gameObject.SetActive(true);

        fadeImage.color = new Color(0, 0, 0, 0);
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        SceneManager.LoadScene(sceneName);

        fadeImage.color = new Color(0, 0, 0, 1);
        elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(1 - (elapsedTime / fadeDuration));
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        fadeImage.gameObject.SetActive(false);

        isFading = false;
    }
}
