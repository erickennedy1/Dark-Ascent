using System.Collections;
using UnityEngine;
using System;

public class BossController : MonoBehaviour
{
    [SerializeField] private BossHealth bossHealth;
    [SerializeField] private BossProjetil bossProjetil;
    [SerializeField] private BossEspinhos bossEspinhos;
    [SerializeField] private InvocacaoChefe invocacaoChefe;

    public Animator cabecaBoss;

    private Coroutine currentComboRoutine;
    private int currentComboNumber;

    private void OnEnable()
    {
        bossHealth.onHealthChanged += HandleHealthChanged;
    }

    private void OnDisable()
    {
        bossHealth.onHealthChanged -= HandleHealthChanged;
    }

    private void HandleHealthChanged(int currentHealth)
    {
        int newComboNumber = GetComboNumber(currentHealth);
        if (newComboNumber != currentComboNumber)
        {
            currentComboNumber = newComboNumber;
            StartCombo(currentComboNumber, true);
        }
    }

    private int GetComboNumber(int health)
    {
        if (health <= 30 && health > 20) return 1;
        if (health <= 20 && health > 10) return 2;
        if (health <= 10 && health > 0) return 3;
        return 0;
    }

    private void StartCombo(int comboNumber, bool immediate = false)
    {
        if (currentComboRoutine != null)
            StopCoroutine(currentComboRoutine);

        CancelCurrentActions();

        // Definir triggers no Animator
        SetAnimatorTrigger(comboNumber);

        currentComboRoutine = StartCoroutine(ComboRoutine(comboNumber, immediate));
    }

    private IEnumerator ComboRoutine(int comboNumber, bool immediate)
    {
        if (immediate)
        {
            yield return new WaitForSeconds(1);
        }
        else
        {
            yield return new WaitForSeconds(3);
        }

        while (true)
        {
            switch (comboNumber)
            {
                case 1:
                    yield return StartCoroutine(PerformAction(() => bossProjetil.AtivarCombo(5, 3, 15), 16, bossProjetil.ResetarDisparos));
                    yield return StartCoroutine(PerformAction(() => bossEspinhos.IniciarEspinhos(2f, 9), 10, bossEspinhos.PararEspinhos));
                    yield return StartCoroutine(PerformAction(() => invocacaoChefe.IniciarInvocacao(3, 2.0f), 10));
                    break;
                case 2:
                    yield return StartCoroutine(PerformAction(() => bossProjetil.AtivarCombo(7, 3, 25), 22, bossProjetil.ResetarDisparos));
                    yield return StartCoroutine(PerformAction(() => bossEspinhos.IniciarEspinhos(1.5f, 14), 15, bossEspinhos.PararEspinhos));
                    yield return StartCoroutine(PerformAction(() => invocacaoChefe.IniciarInvocacao(4, 2.0f), 11));
                    break;
                case 3:
                    yield return StartCoroutine(PerformAction(() => bossProjetil.AtivarCombo(9, 2, 35), 19, bossProjetil.ResetarDisparos));
                    yield return StartCoroutine(PerformAction(() => bossEspinhos.IniciarEspinhos(1f, 17), 18, bossEspinhos.PararEspinhos));
                    yield return StartCoroutine(PerformAction(() => invocacaoChefe.IniciarInvocacao(5, 2.0f), 13));
                    break;
            }

            if (bossHealth.currentHealth <= 0 || GetComboNumber(bossHealth.currentHealth) != comboNumber)
                yield break;

            yield return null;
        }
    }

    private IEnumerator PerformAction(Action action, float waitTime, Action postAction = null)
    {
        action.Invoke();
        float elapsed = 0f;
        while (elapsed < waitTime)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
        postAction?.Invoke();
    }

    private void CancelCurrentActions()
    {
        bossProjetil.CancelarDisparosImediatamente();
        bossEspinhos.PararEspinhos();
        invocacaoChefe.PararInvocacao();
    }

    public void HandleDeath()
    {
        if (currentComboRoutine != null)
        {
            StopCoroutine(currentComboRoutine);
        }

        CancelCurrentActions();
        StartCoroutine(DestroyBossAfterDelay(3f));
    }

    private IEnumerator DestroyBossAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    private void SetAnimatorTrigger(int comboNumber)
    {
        switch (comboNumber)
        {
            case 1:
                break;
            case 2:
                cabecaBoss.SetTrigger("primeiraTroca");
                break;
            case 3:
                cabecaBoss.SetTrigger("segundaTroca");
                cabecaBoss.ResetTrigger("primeiraTroca");
                break;
            default:
                cabecaBoss.ResetTrigger("primeiraTroca");
                cabecaBoss.ResetTrigger("segundaTroca");
                break;
        }
    }
}