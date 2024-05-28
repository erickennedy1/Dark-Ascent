using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionMouseDetector : MonoBehaviour
{
    public ResolutionChange resolutionChange;
    public ResolutionNumberChange resolutionNumberChange;
    void OnMouseEnter()
    {
        SoundManager.Instance.PlaySound("UI_MouseEnter");

        if (gameObject.tag == "Esquerda")
        {
            resolutionChange.SpriteParaEsquerda();
        }
        else if (gameObject.tag == "Direita")
        {
            resolutionChange.SpriteParaDireita();
        }
    }

    void OnMouseDown()
    {
        SoundManager.Instance.PlaySound("UI_MouseClick");

        if (gameObject.tag == "Esquerda")
        {
            resolutionNumberChange.ResolucaoEsquerda();
        }
        else if (gameObject.tag == "Direita")
        {
            resolutionNumberChange.ResolucaoDireita();
        }
    }

    void OnMouseExit()
    {
        resolutionChange.ResetarSprite();
    }
}
