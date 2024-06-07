using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlorExplosiva_Sound : MonoBehaviour, ISoundEnemy
{
    [SerializeField] private AudioSource audio_Antecipation;
    [SerializeField] private AudioSource audio_Explosion;

    public void PlayIdle()
    {
        audio_Antecipation.Play();
    }

    public void StopIdle()
    {
        audio_Antecipation.Stop();
    }

    public void PlayAttack()
    {
        audio_Explosion.Play();
    }
}
