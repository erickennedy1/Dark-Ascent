using UnityEngine;

public class AutoDeactivate : MonoBehaviour
{
    public float timeParticle = 0.2f;
    void OnEnable()
    {
        Invoke("Deactivate", timeParticle);
    }

    void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
