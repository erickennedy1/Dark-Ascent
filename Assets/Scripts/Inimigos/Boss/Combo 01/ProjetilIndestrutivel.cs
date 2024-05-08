using UnityEngine;

public class ProjetilIndestrutivel : MonoBehaviour
{
    [Header("Configurações de dano")]
    public int dano = 1;
    public float tempoDeVida = 5f;

    void Start()
    {
        Destroy(gameObject, tempoDeVida);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(dano);
            }

            Destroy(gameObject);
        }
    }
}
