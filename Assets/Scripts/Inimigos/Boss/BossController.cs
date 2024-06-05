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
    public Animator orb;
    public Animator portao;

    private Coroutine currentComboRoutine;
    private int currentComboNumber;
    private Collider2D bossCollider;

    private void OnEnable()
    {
        bossHealth.onHealthChanged += HandleHealthChanged;
    }

    private void OnDisable()
    {
        bossHealth.onHealthChanged -= HandleHealthChanged;
    }

    private void Start()
    {
        bossCollider = GetComponent<Collider2D>(); // Get the Collider2D component
    }

    private void HandleHealthChanged(int currentHealth)
    {
        int newComboNumber = GetComboNumber(currentHealth);
        if (newComboNumber != currentComboNumber)
        {
            currentComboNumber = newComboNumber;
            StartCombo(currentComboNumber, false);
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
            yield return new WaitForSeconds(1);
        else{
            orb.SetBool("buildOrb", true);
            yield return new WaitForSeconds(2);
            orb.SetBool("buildOrb", false);
        }

        while (true)
        {
            switch (comboNumber)
            {
                case 1:
                    yield return StartCoroutine(PerformAction(() => bossProjetil.AtivarCombo(3, 3, 15), 10));
                    yield return StartCoroutine(PerformAction(() => bossEspinhos.IniciarEspinhos(1.5f, 9), 10, bossEspinhos.PararEspinhos));
                    yield return StartCoroutine(PerformAction(() => invocacaoChefe.IniciarInvocacao(3, 2.0f), 10));
                    break;
                case 2:
                    yield return StartCoroutine(PerformAction(() => bossProjetil.AtivarCombo(4, 2.5f, 25), 12));
                    yield return StartCoroutine(PerformAction(() => bossEspinhos.IniciarEspinhos(1.25f, 14), 14, bossEspinhos.PararEspinhos));
                    yield return StartCoroutine(PerformAction(() => invocacaoChefe.IniciarInvocacao(4, 1.8f), 10));
                    break;
                case 3:
                    yield return StartCoroutine(PerformAction(() => bossProjetil.AtivarCombo(5, 2.5f, 30), 14));
                    yield return StartCoroutine(PerformAction(() => bossEspinhos.IniciarEspinhos(1f, 17), 15, bossEspinhos.PararEspinhos));
                    yield return StartCoroutine(PerformAction(() => invocacaoChefe.IniciarInvocacao(5, 1.6f), 10));
                    break;
            }

            if (bossHealth.currentHealth <= 0 || GetComboNumber(bossHealth.currentHealth) != comboNumber)
                yield break;

        }
    }

    private IEnumerator PerformAction(Action action, float waitTime, Action postAction = null)
    {
        //Invoca o ataque
        action.Invoke();
        yield return new WaitForSeconds(waitTime);

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
        bossCollider.enabled = false;
        orb.SetBool("buildOrb", false);
        orb.SetTrigger("disableOrb");
        StartCoroutine(DestroyBossAfterDelay(5f));
    }

    private IEnumerator DestroyBossAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        portao.SetTrigger("Open");
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
