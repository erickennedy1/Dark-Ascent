using UnityEngine;
using UnityEngine.EventSystems; 

public class UIHoverSoundManager : MonoBehaviour
{
    public static UIHoverSoundManager Instance;

    public AudioClip hoverSound; 
    public AudioSource audioSource; 

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = hoverSound;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayHoverSound()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        audioSource.Play();
    }
}
