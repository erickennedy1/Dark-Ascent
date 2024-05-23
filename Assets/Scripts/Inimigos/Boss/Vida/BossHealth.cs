using System;
using System.Collections;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    [SerializeField]
    public int maxHealth = 30;
    public int currentHealth;

    public BossController bossController;

    public delegate void OnHealthChanged(int currentHealth);
    public event OnHealthChanged onHealthChanged;
    public GameObject orbe;

    private int currentCombo = 0;
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer separateSpriteRenderer;
    public Sprite defeatedSprite;
    private Animator animator;
    private Animator separateAnimator;

    void Start()
    {
        currentHealth = maxHealth;
        onHealthChanged?.Invoke(currentHealth);
        spriteRenderer = GetComponent<SpriteRenderer>();
        separateSpriteRenderer = transform.Find("cabeçaBoss").GetComponent<SpriteRenderer>(); 
        animator = GetComponent<Animator>();
        separateAnimator = separateSpriteRenderer.GetComponent<Animator>(); 
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
            orbe.SetActive(false);
            Debug.Log("Boss defeated!");
            bossController.HandleDeath();
            StopAllAnimations();
            StartCoroutine(ChangeSpriteGradually(defeatedSprite, 2.0f));
        }
    }

    private void StopAllAnimations()
    {
        if (animator != null)
        {
            animator.enabled = false;
        }
        if (separateAnimator != null)
        {
            separateAnimator.enabled = false;
        }
    }

    private IEnumerator ChangeSpriteGradually(Sprite newSprite, float duration)
    {
        spriteRenderer.sprite = newSprite;
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            if (separateSpriteRenderer != null)
            {
                Color separateColor = separateSpriteRenderer.color;
                separateColor.a = Mathf.Lerp(0.5f, 0f, t);
                separateSpriteRenderer.color = separateColor;
            }

            yield return null;
        }

        if (separateSpriteRenderer != null)
        {
            Color separateFinalColor = separateSpriteRenderer.color;
            separateFinalColor.a = 0f; 
            separateSpriteRenderer.color = separateFinalColor;
        }
    }
}
