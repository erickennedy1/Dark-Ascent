using System.Collections;
using UnityEngine;

public class PlayerMana : MonoBehaviour
{
    [Header("Configurações de Mana")]
    public int maxMana = 100;
    public int currentMana;
    private int targetMana;

    private float manaChangeDuration = 1.5f;

    private SpriteRenderer ManaCompletaSprite;
    private SpriteRenderer AnimacaoDaMana;

    private float lastManaUseTime;
    private Vector2 originalSize;

    private Coroutine manaChangeCoroutine;

    void Start()
    {
        ManaCompletaSprite = GameObject.Find("ManaCompleta").GetComponent<SpriteRenderer>();
        AnimacaoDaMana = GameObject.Find("Animação da Mana").GetComponent<SpriteRenderer>();

        currentMana = 30;
        targetMana = currentMana;

        if (ManaCompletaSprite != null && AnimacaoDaMana != null)
        {
            originalSize = ManaCompletaSprite.size;
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

    public void recuperarMana(int quantidade)
    {
        if (targetMana < maxMana)
        {
            Debug.Log("Recuperando Mana: " + quantidade);
            targetMana = currentMana + quantidade;
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

            float extraOffset = 0.24f * originalSize.y;

            Vector3 newWavePosition = new Vector3(AnimacaoDaMana.transform.position.x, ManaCompletaSprite.transform.position.y + heightChange / 2 - extraOffset, AnimacaoDaMana.transform.position.z);
            AnimacaoDaMana.size = new Vector2(originalSize.x, heightChange);
            AnimacaoDaMana.transform.position = newWavePosition;
        }
    }
}
