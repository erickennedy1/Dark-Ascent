using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private BossHealth bossHealth;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private float updateSpeedSeconds = 0.5f;

    private Coroutine currentCoroutine;

    void OnEnable()
    {
        bossHealth.onHealthChanged += HandleHealthChanged;
    }

    void OnDisable()
    {
        bossHealth.onHealthChanged -= HandleHealthChanged;
    }

    private void Start()
    {
        // Inicializa o máximo do slider
        healthSlider.maxValue = bossHealth.maxHealth;
        healthSlider.value = bossHealth.maxHealth;
    }

    private void HandleHealthChanged(int currentHealth)
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(UpdateHealthBar(currentHealth));
    }

    private IEnumerator UpdateHealthBar(int currentHealth)
    {
        float preChangeValue = healthSlider.value;
        float elapsed = 0f;

        while (elapsed < updateSpeedSeconds)
        {
            elapsed += Time.deltaTime;
            healthSlider.value = Mathf.Lerp(preChangeValue, currentHealth, elapsed / updateSpeedSeconds);
            yield return null;
        }

        healthSlider.value = currentHealth;
    }
}
