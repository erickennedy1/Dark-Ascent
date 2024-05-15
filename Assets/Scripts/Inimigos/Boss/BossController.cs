using System.Collections;
using UnityEngine;
using System;

public class BossController : MonoBehaviour
{
    [SerializeField] private BossHealth bossHealth;
    [SerializeField] private BossProjetil bossProjetil;
    [SerializeField] private BossEspinhos bossEspinhos;
    [SerializeField] private InvocacaoChefe invocacaoChefe;

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
            StartCombo(currentComboNumber);
        }
    }

    private int GetComboNumber(int health)
    {
        if (health <= 30 && health > 20) return 1;
        if (health <= 20 && health > 10) return 2;
        if (health <= 10 && health > 0) return 3;
        return 0;
    }

    private void StartCombo(int comboNumber)
    {
        if (currentComboRoutine != null)
            StopCoroutine(currentComboRoutine);

        currentComboRoutine = StartCoroutine(ComboRoutine(comboNumber));
    }

    private IEnumerator ComboRoutine(int comboNumber)
    {
        yield return new WaitForSeconds(3);

        switch (comboNumber)
        {
            case 1:
                yield return StartCoroutine(PerformAction(bossProjetil.AtivarCombo, 15, bossProjetil.ResetarDisparos));
                yield return StartCoroutine(PerformAction(bossEspinhos.IniciarEspinhos, 10, bossEspinhos.PararEspinhos));
                yield return StartCoroutine(PerformAction(() => invocacaoChefe.IniciarInvocacao(3, 2.0f), 10));
                break;
            case 2:
                yield return StartCoroutine(PerformAction(() => invocacaoChefe.IniciarInvocacao(3, 2.0f), 10));
                yield return StartCoroutine(PerformAction(bossEspinhos.IniciarEspinhos, 10, bossEspinhos.PararEspinhos));
                yield return StartCoroutine(PerformAction(bossProjetil.AtivarCombo, 15, bossProjetil.ResetarDisparos));
                break;
            case 3:
                yield return StartCoroutine(PerformAction(bossEspinhos.IniciarEspinhos, 10, bossEspinhos.PararEspinhos));
                yield return StartCoroutine(PerformAction(bossProjetil.AtivarCombo, 15, bossProjetil.ResetarDisparos));
                yield return StartCoroutine(PerformAction(() => invocacaoChefe.IniciarInvocacao(3, 2.0f), 10));
                break;
        }

        if (bossHealth.currentHealth > 0 && GetComboNumber(bossHealth.currentHealth) == comboNumber)
            StartCombo(comboNumber); // Loop the current combo if the boss is still alive and health stage hasn't changed
    }

    private IEnumerator PerformAction(Action action, float waitTime, Action postAction = null)
    {
        action.Invoke();
        yield return new WaitForSeconds(waitTime);
        postAction?.Invoke();
    }
}
