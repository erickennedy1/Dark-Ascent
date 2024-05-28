using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundTrackManager : MonoBehaviour
{
    public static SoundTrackManager Instance { get; private set; }

    public AudioMixerGroup outputMixer;

    public List<Sound> soundtracks;

    private AudioSource currentSoundtrack;

    void Awake()
    {
        if (Instance == null){
            Instance = this;
        }
        else{
            Destroy(gameObject);
            return;
        }

        foreach(Sound s in soundtracks)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.outputAudioMixerGroup = outputMixer;
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    void Start(){
        PlayMusic("Menu_Music");
    }

    public void PlayMusic(string name)
    {
        Sound s = soundtracks.Find(sound => sound.name == name);
        if(s == null)
        {
            Debug.LogWarning("Sound: "+name+" not found!");
            return;
        }

        //Only one Soundtrack by time
        StopMusic();
        currentSoundtrack = s.source;
        currentSoundtrack.Play();    
    }

    public void StopMusic(){
        if(currentSoundtrack == null)
            return;
        currentSoundtrack.Stop();
        currentSoundtrack = null;
    }
}
