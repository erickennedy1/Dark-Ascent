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

    [Header("Componentes de UI")]
    public SpriteRenderer ManaCompletaSprite;
    public SpriteRenderer AnimacaoDaMana;

    private float lastManaUseTime;
    private Vector2 originalSize;
    private Vector3 originalBarPosition;
    private Vector3 originalWavePosition;

    private Coroutine manaChangeCoroutine;

    void Start()
    {
        currentMana = maxMana;
        targetMana = maxMana;
        if (ManaCompletaSprite != null && AnimacaoDaMana != null)
        {
            originalSize = ManaCompletaSprite.size;
            originalBarPosition = ManaCompletaSprite.transform.position;
            originalWavePosition = AnimacaoDaMana.transform.position;
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
        if (ManaCompletaSprite != null && AnimacaoDaMana != null)
        {
            float manaPercentage = (float)currentMana / maxMana;
            float heightChange = originalSize.y * manaPercentage;

            ManaCompletaSprite.size = new Vector2(originalSize.x, heightChange);

            AnimacaoDaMana.size = new Vector2(originalSize.x, heightChange);
            AnimacaoDaMana.transform.position = new Vector3(AnimacaoDaMana.transform.position.x, originalWavePosition.y - (originalSize.y - heightChange) / 2, originalWavePosition.z);
        }
    }
}
