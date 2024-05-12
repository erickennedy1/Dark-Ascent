using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private BossHealth bossHealth;
    [SerializeField] private Slider healthSlider;

    void OnEnable()
    {
        bossHealth.onHealthChanged += UpdateHealthBar;
    }

    void OnDisable()
    {
        bossHealth.onHealthChanged -= UpdateHealthBar;
    }

    private void Start()
    {
        // Inicializa o máximo do slider
        healthSlider.maxValue = bossHealth.maxHealth;
        healthSlider.value = bossHealth.maxHealth;
    }

    private void UpdateHealthBar(int currentHealth)
    {
        healthSlider.value = currentHealth;
    }
}
