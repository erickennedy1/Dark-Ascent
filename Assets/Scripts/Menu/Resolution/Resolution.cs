using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resolution : MonoBehaviour
{
    public GameObject iconFull;
    public bool fullScreen = false;

    void Start()
    {
        fullScreen = false;
    }

    void OnMouseDown()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.mouseClick);

            if (fullScreen)
        {
            fullScreen = false;
            iconFull.gameObject.SetActive(true);
            Screen.fullScreen = true;
        }
        else
        {
            fullScreen = true;
            iconFull.gameObject.SetActive(false);
            Screen.fullScreen = false;
        }
    }
}
