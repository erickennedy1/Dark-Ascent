using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public BossEspinhos espinhos;
    public BossProjetil projetil;

    public Boolean teste = false;

    private void Update()
    {
        if (teste)
        {
            espinhos.gameObject.SetActive(false);
            projetil.gameObject.SetActive(true);
        }
        else
        {
            espinhos.gameObject.SetActive(true);
            projetil.gameObject.SetActive(false);
        }
    }

    public void Teste(bool teste1)
    {
        teste1 = teste;

    }
}
