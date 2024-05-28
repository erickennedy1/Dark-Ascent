using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    public AudioMixerGroup outputMixer;
    public List<Sound> sounds;
    private List<Sound> activeLoopSounds = new List<Sound>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.outputAudioMixerGroup = outputMixer;
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void PlaySound(string name)
    {
        Sound s = sounds.Find(sound => sound.name == name);
        if(s == null)
        {
            Debug.LogWarning("Sound: "+name+" not found!");
            return;
        }

        if(s.loop){
            activeLoopSounds.Add(s);
        }
        s.source.Play();
    }

    public void StopSoundLoop(string name){
        Sound s = activeLoopSounds.Find(sound => sound.name == name);
        if(s == null)
        {
            Debug.LogWarning("SoundLoop: "+name+" not found!");
            return;
        }

        s.source.Stop();
        activeLoopSounds.Remove(s);
    }
}
