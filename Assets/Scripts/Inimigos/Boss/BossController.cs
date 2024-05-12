using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public BossProjetil bossProjetil;
    public BossEspinhos bossEspinhos;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            AtivarComboProjetil();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            AtivarComboEspinhos();
        }
    }

    private void AtivarComboProjetil()
    {
        if (bossProjetil != null)
        {
            bossProjetil.AtivarCombo();
        }
    }

    private void AtivarComboEspinhos()
    {
        if (bossEspinhos != null)
        {
            bossEspinhos.IniciarEspinhos();
        }
    }
}
