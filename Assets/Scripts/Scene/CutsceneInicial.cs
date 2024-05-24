using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneInicial : MonoBehaviour
{
    [Header("Texto principal")]
    public TMP_Text TextTMP;
    public GameObject TextTM;
    [Header("Lista de textos")]
    public List<string> text = new List<string>();
    [Header("Lista de CutScenes")]
    public List<Image> image = new List<Image>();

    [Header("Video")]
    public GameObject videoScene;

    [Header("Velocidade da digitação")]
    public float velocidade = 0.1f;



    private void Start()
    {
        TextTMP.text = string.Empty;

        StartCoroutine(CutsceneTextos());
    }

    IEnumerator CutsceneTextos()
    {
        TextTMP.text = string.Empty;
        StartCoroutine(FadeImage(true, 0));
        StartCoroutine(escritaTexto(0));
        yield return new WaitForSeconds(6);

        TextTMP.text = string.Empty;
        StartCoroutine(FadeImage(true, 1));
        StartCoroutine(escritaTexto(1));
        yield return new WaitForSeconds(6);

        TextTMP.text = string.Empty;
        StartCoroutine(FadeImage(true, 2));
        StartCoroutine(escritaTexto(2));
        yield return new WaitForSeconds(6);

        TextTMP.text = string.Empty;
        StartCoroutine(FadeImage(true, 3));
        StartCoroutine(escritaTexto(3));
        yield return new WaitForSeconds(6);

        TextTMP.text = string.Empty;
        StartCoroutine(FadeImage(false, 0));
        StartCoroutine(FadeImage(false, 1));
        StartCoroutine(FadeImage(false, 2));
        StartCoroutine(FadeImage(false, 3));
        yield return new WaitForSeconds(2);

        StartCoroutine(FadeImage(true, 4));
        StartCoroutine(escritaTexto(4));
        yield return new WaitForSeconds(6);

        TextTMP.text = string.Empty;
        StartCoroutine(FadeImage(true, 5));
        StartCoroutine(escritaTexto(5));
        yield return new WaitForSeconds(5);

        TextTMP.text = string.Empty;
        StartCoroutine(FadeImage(true, 6));
        StartCoroutine(escritaTexto(6));
        yield return new WaitForSeconds(3);

        StartCoroutine(FadeImage(false, 4));
        StartCoroutine(FadeImage(false, 5));
        StartCoroutine(FadeImage(false, 6));
        yield return new WaitForSeconds(1.5f);

        TextTMP.text = string.Empty;
        TextTM.SetActive(false);
        videoScene.SetActive(true);
    }

    IEnumerator escritaTexto(int textoAtual)
    {
        foreach (char letter in text[textoAtual])
        {
            TextTMP.text += letter; 
            yield return new WaitForSeconds(velocidade); 
        }
    }

    IEnumerator FadeImage(bool aparecendo, int imageCount)
    {
        if (aparecendo)
        {
            for (float i = 0; i <= 1; i += (Time.deltaTime * 0.2f))
            {
                image[imageCount].color = new Color(1, 1, 1, i);
                yield return null;
            }
        }
        else
        {

            for (float i = 1; i >= 0; i -= (Time.deltaTime * 0.5f))
            {
                image[imageCount].color = new Color(1, 1, 1, i);
                yield return null;
            }
        }
    }
}
