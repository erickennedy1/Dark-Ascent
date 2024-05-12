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
    private int ativa��esRealizadas = 0;  // Vari�vel para contar quantas vezes os espinhos foram ativados
    private int maxAtivacoes = 2;  // M�ximo de ativa��es permitidas

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

        while (ativa��esRealizadas < maxAtivacoes)
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

            ativa��esRealizadas++;  // Incrementa a contagem de ativa��es
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
