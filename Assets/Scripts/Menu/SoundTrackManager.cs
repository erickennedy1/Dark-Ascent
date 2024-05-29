using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SoundTrackManager : MonoBehaviour
{
    public static SoundTrackManager Instance { get; private set; }

    public AudioMixerGroup outputMixer;

    public List<Sound> soundtracks;

    private AudioSource currentSoundtrack;

    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

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

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch(scene.name)
        {
            case "Menu":
                PlayMusic("Menu");
                break;
            case "Cutscene_Inicial":
                PlayMusic("CutScene");
                break;
            case "Hub_Init":
                StopMusic();
                break;
            case "Hub":
                PlayMusic("Hub");
                break;
            case "Dungeon":
                PlayMusic("Exploration");
                break;
            case "BossFlorest":
                PlayMusic("Boss");
                break;
            case "EndScene":
                PlayMusic("End");
                break;
            case "EmptyRoom":
                //Só para não ficar dando mensagem de Erro
                break;
            default:
                Debug.LogWarning("Scene not Recognized");
                break;
        }
    }

    void Start(){
        PlayMusic("Menu");
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
