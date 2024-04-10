using UnityEngine;
using UnityEngine.Audio; 

public class AudioManager : MonoBehaviour
{
    public AudioMixer audioMixer;

    public void SetMasterVolume(float volume)
    {
        volume = Mathf.Max(volume, 0.0001f);
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
    }
    public void SetMusicVolume(float volume)
    {
        volume = Mathf.Max(volume, 0.0001f);
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume(float volume)
    {
        volume = Mathf.Max(volume, 0.0001f); 
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
    }
}
