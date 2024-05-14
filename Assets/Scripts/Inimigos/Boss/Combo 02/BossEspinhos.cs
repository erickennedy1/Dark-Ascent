using UnityEngine;

public class BossEspinhos : MonoBehaviour
{
    [SerializeField] private GameObject espinhosPrefab;
    [SerializeField] private float intervaloEntreEspinhos = 0.5f;
    [SerializeField] public float duracaoAtiva = 15f;

    private Transform jogador;
    private Rigidbody2D rbJogador;

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
        InvokeRepeating(nameof(CriarEspinhos), 0f, intervaloEntreEspinhos);
    }

    public void PararEspinhos()
    {
        CancelInvoke(nameof(CriarEspinhos));
    }

    private void CriarEspinhos()
    {
        if (jogador != null && rbJogador != null)
        {
            float direcaoEspinhosX = 0f;  // Reset da direção a cada chamada
            if (rbJogador.velocity.x > 0)
            {
                direcaoEspinhosX = 5f;  // Ajuste conforme necessário
            }
            else if (rbJogador.velocity.x < 0)
            {
                direcaoEspinhosX = -5f;  // Ajuste conforme necessário
            }

            // Configurando a posição dos espinhos considerando também a direção Y se necessário
            Vector3 posicaoEspinhos = jogador.position + new Vector3(direcaoEspinhosX, 1f, 0f);
            Instantiate(espinhosPrefab, posicaoEspinhos, Quaternion.identity);
        }
    }
}
