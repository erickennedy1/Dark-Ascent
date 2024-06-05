using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    public event Action EventEndTutorial;
    public Sprite[] sprites;
    public SpriteRenderer render;
    private bool isTutorialOn = false;

    private int currentSprite;

    public void StartTutorial()
    {
        render = GetComponent<SpriteRenderer>();
        currentSprite = 0;

        isTutorialOn = true;
        render.sprite = sprites[currentSprite];
        currentSprite++;
    }

    public void NextTutorial(){
        if(currentSprite >= sprites.Length){
            EndTutorial();
            return;
        }
        render.sprite = sprites[currentSprite];
        currentSprite++;
    }

    public void EndTutorial()
    {
        isTutorialOn = false;
        EventEndTutorial?.Invoke();
        gameObject.SetActive(false);
    }
}
