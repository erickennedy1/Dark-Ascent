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

    private int currentCombo = 0; // Adicionado para rastrear o combo atual

    void Start()
    {
        currentHealth = maxHealth;
        onHealthChanged?.Invoke(currentHealth);
    }

    private void Update()
    {
        int newCombo = 0;

        if (currentHealth <= 30 && currentHealth >= 21)
        {
            newCombo = 1;
        }
        else if (currentHealth <= 20 && currentHealth >= 11)
        {
            newCombo = 2;
        }
        else if (currentHealth <= 10 && currentHealth > 0)
        {
            newCombo = 3;
        }

        // Só muda o combo se ele for diferente do atual
        if (newCombo != currentCombo)
        {
            currentCombo = newCombo;
            bossController.combo1Ativo = newCombo == 1;
            bossController.combo2Ativo = newCombo == 2;
            bossController.combo3Ativo = newCombo == 3;
            bossController.MudarCombo(newCombo);
        }
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
