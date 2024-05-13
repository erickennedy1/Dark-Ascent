using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public BossProjetil bossProjetil;
    public BossEspinhos bossEspinhos;

    public bool combo1Ativo = true;
    public bool combo2Ativo = false;
    public bool combo3Ativo = false;

    private Coroutine ataqueCorrente;

    private void Start()
    {
        ataqueCorrente = StartCoroutine(AtaqueContinuo());
    }

    IEnumerator AtaqueContinuo()
    {
        while (true)
        {
            if (combo1Ativo)
            {
                yield return StartCoroutine(ExecutarCombo1());
            }

            if (combo2Ativo)
            {
                yield return StartCoroutine(ExecutarCombo2());
            }

            if (combo3Ativo)
            {
                yield return StartCoroutine(ExecutarCombo3());
            }

            yield return null;
        }
    }

    IEnumerator ExecutarCombo1()
    {
        bossProjetil.infosAtaque(5, 5, 15);
        if (bossProjetil != null)
        {
            bossProjetil.AtivarCombo();
            yield return new WaitForSeconds(25);
            bossProjetil.CancelarDisparos();
            bossProjetil.ResetarDisparos();
        }

        if (bossEspinhos != null)
        {
            bossEspinhos.IniciarEspinhos();
            yield return new WaitForSeconds(15);
            bossEspinhos.PararEspinhos();
        }

        yield return new WaitForSeconds(2);
    }

    IEnumerator ExecutarCombo2()
    {
        bossProjetil.infosAtaque(6, 4, 25);
        if (bossProjetil != null)
        {
            bossProjetil.AtivarCombo();
            yield return new WaitForSeconds(28);
            bossProjetil.CancelarDisparos();
            bossProjetil.ResetarDisparos();
        }

        if (bossEspinhos != null)
        {
            bossEspinhos.IniciarEspinhos();
            yield return new WaitForSeconds(15);
            bossEspinhos.PararEspinhos();
        }

        yield return new WaitForSeconds(2);
    }

    IEnumerator ExecutarCombo3()
    {
        bossProjetil.infosAtaque(20, 2, 40);
        if (bossProjetil != null)
        {
            bossProjetil.AtivarCombo();
            yield return new WaitForSeconds(40);
            bossProjetil.CancelarDisparos();
            bossProjetil.ResetarDisparos();
        }

        if (bossEspinhos != null)
        {
            bossEspinhos.IniciarEspinhos();
            yield return new WaitForSeconds(15);
            bossEspinhos.PararEspinhos();
        }

        yield return new WaitForSeconds(2);
    }

    public void MudarCombo(int combo)
    {
        combo1Ativo = combo == 1;
        combo2Ativo = combo == 2;
        combo3Ativo = combo == 3;
        if (ataqueCorrente != null)
        {
            StopCoroutine(ataqueCorrente);
        }
        ataqueCorrente = StartCoroutine(AtaqueContinuo());
    }
}
