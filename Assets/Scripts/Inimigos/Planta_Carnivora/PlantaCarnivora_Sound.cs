using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantaCarnivora_Sound : MonoBehaviour, ISoundEnemy
{
    [SerializeField] private AudioSource audio_Attack;

    public void PlayIdle()
    {
        return;
    }

    public void StopIdle()
    {
        return;
    }

    public void PlayAttack()
    {
        audio_Attack.Play();
    }
}
