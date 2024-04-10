using UnityEngine;

public class AtaqueEspecial00 : MonoBehaviour
{
    [Header("Configurações de ataque")]
    public int damageAmount = 10;
    public float specialAttackRange = 3f;
    public float attackParticules = 0.5f;
    public int specialAttackManaCost = 30;
    private GameObject sparksAnimationObject;
    private PlayerMana playerMana;

    void Start()
    {
        Transform sparksTransform = transform.Find("Efeito de ataque");

        playerMana = GetComponent<PlayerMana>();

        if (sparksTransform != null)
        {
            sparksAnimationObject = sparksTransform.gameObject;
        }
    }

    public bool TryPerformSpecialAttack()
    {
        if (playerMana && playerMana.UseMana(specialAttackManaCost))
        {
            PerformSpecialAttack();
            return true;
        }
        Debug.Log("Mana insuficiente para ataque especial.");
        return false;
    }

    public void PerformSpecialAttack()
    {
        Vector2 attackPoint = transform.position;

        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(attackPoint, specialAttackRange, LayerMask.GetMask("Enemy", "Projectile"));
        foreach (var hit in hitObjects)
        {
            if (hit.CompareTag("Enemy"))
            {
                EnemyMovementAndHealth enemy = hit.GetComponent<EnemyMovementAndHealth>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damageAmount);
                    GameObject sparksInstance = Instantiate(sparksAnimationObject, enemy.transform.position, Quaternion.identity);
                    sparksInstance.SetActive(true);
                    Destroy(sparksInstance, attackParticules);
                }
            }
            else if (hit.CompareTag("Projectile"))
            {
                Destroy(hit.gameObject);
            }
        }
    }
}
