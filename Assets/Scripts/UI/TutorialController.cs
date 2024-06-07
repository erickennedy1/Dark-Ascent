using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    public event Action EventEndTutorial;
    public Sprite[] sprites;
    public SpriteRenderer render;

    private int currentSprite;

    public void StartTutorial()
    {
        render = GetComponent<SpriteRenderer>();
        currentSprite = 0;

        render.sprite = sprites[currentSprite];
        currentSprite++;
    }

    public void NextTutorial(){
        SoundManager.Instance.PlaySound("UI_MouseClick");
        if(currentSprite >= sprites.Length){
            EndTutorial();
            return;
        }
        render.sprite = sprites[currentSprite];
        currentSprite++;
    }

    public void EndTutorial()
    {
        EventEndTutorial?.Invoke();
        gameObject.SetActive(false);
    }
}
