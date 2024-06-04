using UnityEngine;

public class BeijaFlor_Sound : MonoBehaviour, ISoundEnemy
{
    [SerializeField] private AudioSource audio_Idle;
    [SerializeField] private AudioSource audio_Attack;

    public void PlayIdle()
    {
        audio_Idle.Play();
    }

    public void StopIdle()
    {
        audio_Idle.Stop();
    }

    public void PlayAttack()
    {
        audio_Attack.Play();
    }
}
