using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public AudioManager audioManager; 

    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;   
    public Slider sfxVolumeSlider;    

    void Start()
    {
        InitializeSliders();

        masterVolumeSlider.onValueChanged.AddListener(HandleMasterVolumeChanged);
        musicVolumeSlider.onValueChanged.AddListener(HandleMusicVolumeChanged);
        sfxVolumeSlider.onValueChanged.AddListener(HandleSFXVolumeChanged);
    }

    void InitializeSliders()
    {
        float currentVolume;
        if (audioManager.audioMixer.GetFloat("MasterVolume", out currentVolume))
        {
            masterVolumeSlider.value = Mathf.Pow(10, currentVolume / 20); 
        }
        if (audioManager.audioMixer.GetFloat("MusicVolume", out currentVolume))
        {
            musicVolumeSlider.value = Mathf.Pow(10, currentVolume / 20); 
        }
        if (audioManager.audioMixer.GetFloat("SFXVolume", out currentVolume))
        {
            sfxVolumeSlider.value = Mathf.Pow(10, currentVolume / 20); 
        }
    }

    private void HandleMasterVolumeChanged(float volume)
    {
        audioManager.SetMasterVolume(volume);
    }

    private void HandleMusicVolumeChanged(float volume)
    {
        audioManager.SetMusicVolume(volume);
    }

    private void HandleSFXVolumeChanged(float volume)
    {
        audioManager.SetSFXVolume(volume);
    }
}
