using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    [SerializeField]
    public int maxHealth = 30;
    public int currentHealth;

    public BossController bossController;

    public delegate void OnHealthChanged(int currentHealth);
    public event OnHealthChanged onHealthChanged;

    private int currentCombo = 0; 

    void Start()
    {
        currentHealth = maxHealth;
        onHealthChanged?.Invoke(currentHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;
        onHealthChanged?.Invoke(currentHealth);
        CheckDeath();
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        onHealthChanged?.Invoke(currentHealth);
    }

    private void CheckDeath()
    {
        if (currentHealth <= 0)
        {
            Debug.Log("Boss defeated!");
            // Aqui você pode adicionar mais lógica para quando o boss for derrotado.
        }
    }
}
