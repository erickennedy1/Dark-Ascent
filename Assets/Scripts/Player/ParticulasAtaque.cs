using UnityEngine;

public class ParticulasAtaque : MonoBehaviour
{
    public GameObject sparksPrefab;
    public float particleLifetime = 0.5f;

    public void SpawnParticles(Vector2 position)
    {
        GameObject sparksInstance = Instantiate(sparksPrefab, position, Quaternion.identity);
        sparksInstance.SetActive(true);
        Destroy(sparksInstance, particleLifetime);
    }
}
