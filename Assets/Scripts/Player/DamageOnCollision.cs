using UnityEngine;

public class DamageOnCollision : MonoBehaviour
{
    public int damageAmount = 1; // Quantidade de dano a ser aplicada ao jogador
    public float damageCooldown = 2f; // Tempo de espera entre cada aplicação de dano

    private float lastDamageTime; // Guarda o tempo da última aplicação de dano

    void OnTriggerEnter2D(Collider2D other)
    {
        // Verificar se o objeto que colidiu possui o script PlayerHealth anexado
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth != null && Time.time - lastDamageTime >= damageCooldown)
        {
            // Se o jogador colidiu e já passou o cooldown, aplique o dano
            playerHealth.TakeDamage(damageAmount);
            lastDamageTime = Time.time;
        }
    }
}
