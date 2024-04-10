using System.Collections;
using UnityEngine;

public class PlayerMana : MonoBehaviour
{
    [Header("Configurações de Mana")]
    public int maxMana = 100;
    public int currentMana;
    private int targetMana;
    public float manaRegenRate = 5f;
    public float manaRegenCooldown = 5f;

    [Header("Animação de Mana")]
    public float manaChangeDuration = 1.5f;

    [Header("Componente")]
    public SpriteRenderer manaSprite;

    private float lastManaUseTime;
    private Vector2 originalSize;
    private Vector3 originalPosition;

    private Coroutine manaChangeCoroutine;

    void Start()
    {
        currentMana = maxMana;
        targetMana = maxMana;
        if (manaSprite != null)
        {
            originalSize = manaSprite.size;
            originalPosition = manaSprite.transform.position;
        }
        UpdateManaUI();
    }

    public bool UseMana(int amount)
    {
        if (targetMana >= amount)
        {
            targetMana -= amount;
            lastManaUseTime = Time.time;
            StartManaChangeCoroutine();
            return true;
        }
        return false;
    }

    private void RegenerateMana()
    {
        if (Time.time - lastManaUseTime >= manaRegenCooldown && targetMana < maxMana)
        {
            targetMana = Mathf.Min(targetMana + Mathf.FloorToInt(manaRegenRate * Time.deltaTime), maxMana);
            StartManaChangeCoroutine();
        }
    }

    private void StartManaChangeCoroutine()
    {
        if (manaChangeCoroutine != null)
        {
            StopCoroutine(manaChangeCoroutine);
        }
        manaChangeCoroutine = StartCoroutine(AnimateManaChange());
    }

    IEnumerator AnimateManaChange()
    {
        float elapsedTime = 0f;
        int startingMana = currentMana;

        while (elapsedTime < manaChangeDuration)
        {
            elapsedTime += Time.deltaTime;
            currentMana = (int)Mathf.Lerp(startingMana, targetMana, elapsedTime / manaChangeDuration);
            UpdateManaUI();
            yield return null;
        }

        currentMana = targetMana;
        UpdateManaUI();
    }

    void UpdateManaUI()
    {
        if (manaSprite != null)
        {
            float manaPercentage = (float)currentMana / maxMana;
            manaSprite.size = new Vector2(originalSize.x, originalSize.y * manaPercentage);
        }
    }
}
