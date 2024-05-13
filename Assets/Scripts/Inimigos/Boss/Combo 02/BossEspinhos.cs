using UnityEngine;

public class BossEspinhos : MonoBehaviour
{
    [SerializeField] private GameObject espinhosPrefab;
    [SerializeField] private float intervaloEntreEspinhos = 0.5f;  // Intervalo constante para a criação de espinhos
    [SerializeField] public float duracaoAtiva = 15f;  // Tempo de duração da ativação dos espinhos

    private Transform jogador;

    void Start()
    {
        jogador = GameObject.FindGameObjectWithTag("Player").transform;
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
        if (jogador != null)
        {
            Vector3 posicaoEspinhos = new Vector3(jogador.position.x, jogador.position.y + 0.5f, jogador.position.z);
            Instantiate(espinhosPrefab, posicaoEspinhos, Quaternion.identity);
        }
    }
}
