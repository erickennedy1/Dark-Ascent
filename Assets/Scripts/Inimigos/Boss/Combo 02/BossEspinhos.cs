using UnityEngine;

public class BossEspinhos : MonoBehaviour
{
    [SerializeField] private GameObject espinhosPrefab;
    [SerializeField] private float intervaloEntreEspinhos = 0.5f;
    [SerializeField] private float duracaoAtiva = 5f;

    private Transform jogador;
    private Rigidbody2D rbJogador;
    private float tempoAtivo;  // Adicionado para controlar o tempo de atividade dos espinhos

    void Start()
    {
        GameObject jogadorObj = GameObject.FindGameObjectWithTag("Player");
        if (jogadorObj != null)
        {
            jogador = jogadorObj.transform;
            rbJogador = jogadorObj.GetComponent<Rigidbody2D>();
        }
    }

    public void ConfigEspinhos(int intervalo, int duracao)
    {
        intervaloEntreEspinhos = intervalo;
        duracaoAtiva = duracao;
    }

    public void IniciarEspinhos()
    {
        tempoAtivo = 0f; // Resetar o contador de tempo
        InvokeRepeating(nameof(CriarEspinhos), 0f, intervaloEntreEspinhos);
    }

    public void PararEspinhos()
    {
        CancelInvoke(nameof(CriarEspinhos));
    }

    private void Update()
    {
        if (tempoAtivo >= duracaoAtiva)
        {
            PararEspinhos();
        }
        else
        {
            tempoAtivo += Time.deltaTime;
        }
    }

    private void CriarEspinhos()
    {
        if (jogador != null && rbJogador != null)
        {
            float direcaoEspinhosX = 0f;
            if (rbJogador.velocity.x > 0)
            {
                direcaoEspinhosX = 5f;
            }
            else if (rbJogador.velocity.x < 0)
            {
                direcaoEspinhosX = -5f;
            }

            Vector3 posicaoEspinhos = jogador.position + new Vector3(direcaoEspinhosX, 1f, 0f);
            Instantiate(espinhosPrefab, posicaoEspinhos, Quaternion.identity);
        }
    }
}
