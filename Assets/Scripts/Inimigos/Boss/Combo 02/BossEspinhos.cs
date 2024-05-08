using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEspinhos : MonoBehaviour
{
    [SerializeField] private GameObject espinhosPrefab;
    [SerializeField] private float intervaloInicialEntreEspinhos = 0.5f;
    [SerializeField] private float intervaloSecundarioEntreEspinhos = 0.2f;  
    [SerializeField] private float duracaoAtiva = 10f;
    [SerializeField] private float duracaoPausa = 5f;

    private Transform jogador;
    private bool primeiraAtivacao = true;  

    void Start()
    {
        jogador = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(AtivarEspinhos());
    }

    private IEnumerator AtivarEspinhos()
    {
        float intervaloAtual = intervaloInicialEntreEspinhos;  

        while (true)
        {
            InvokeRepeating(nameof(CriarEspinhos), 0f, intervaloAtual);
            yield return new WaitForSeconds(duracaoAtiva);
            CancelInvoke(nameof(CriarEspinhos));
            yield return new WaitForSeconds(duracaoPausa);

            if (primeiraAtivacao)
            {
                intervaloAtual = intervaloSecundarioEntreEspinhos; 
                primeiraAtivacao = false; 
            }
        }
    }

    private void CriarEspinhos()
    {
        if (jogador != null)
        {
            Instantiate(espinhosPrefab, new Vector3(jogador.position.x, jogador.position.y+0.5f, jogador.position.z), Quaternion.identity);
        }
    }
}
