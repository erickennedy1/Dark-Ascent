using UnityEngine;
using System.Collections;

public class BossEspinhos : MonoBehaviour
{
    [SerializeField] private GameObject espinhosPrefab;
    [SerializeField] private float intervaloInicialEntreEspinhos = 0.5f;
    [SerializeField] private float intervaloSecundarioEntreEspinhos = 0.2f;
    [SerializeField] public float duracaoAtiva = 10f;
    [SerializeField] public float duracaoPausa = 5f;

    private Transform jogador;
    private bool primeiraAtivacao = true;
    private int ativaçõesRealizadas = 0;  // Variável para contar quantas vezes os espinhos foram ativados
    private int maxAtivacoes = 2;  // Máximo de ativações permitidas

    void Start()
    {
        jogador = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void IniciarEspinhos()
    {
        StartCoroutine(AtivarEspinhos());
    }

    public void PararEspinhos()
    {
        StopCoroutine(AtivarEspinhos());
        CancelInvoke(nameof(CriarEspinhos));
    }

    public IEnumerator AtivarEspinhos()
    {
        float intervaloAtual = intervaloInicialEntreEspinhos;

        while (ativaçõesRealizadas < maxAtivacoes)
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

            ativaçõesRealizadas++;  // Incrementa a contagem de ativações
        }
    }

    private void CriarEspinhos()
    {
        if (jogador != null)
        {
            Instantiate(espinhosPrefab, new Vector3(jogador.position.x, jogador.position.y + 0.5f, jogador.position.z), Quaternion.identity);
        }
    }
}
