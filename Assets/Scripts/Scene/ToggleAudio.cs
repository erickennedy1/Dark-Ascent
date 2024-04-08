using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleAudio : MonoBehaviour
{
    public AudioSource audioSource;

    public void ToggleSound(bool isOn)
    {
        if (audioSource != null)
        {
            audioSource.mute = !isOn;
        }
    }
}
