using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEspinhos : MonoBehaviour
{
    [SerializeField] private GameObject espinhosPrefab; // Prefab dos espinhos que surgirão abaixo do jogador
    [SerializeField] private float intervaloEntreEspinhos = 0.5f; // Intervalo de tempo entre cada surgimento de espinhos
    [SerializeField] private float duracaoAtiva = 10f; // Duração em segundos que os espinhos continuam surgindo
    [SerializeField] private float duracaoPausa = 5f; // Duração da pausa entre as séries de surgimentos

    private Transform jogador;

    void Start()
    {
        jogador = GameObject.FindGameObjectWithTag("Player").transform; // Encontrando o jogador pela tag
        StartCoroutine(AtivarEspinhos());
    }

    private IEnumerator AtivarEspinhos()
    {
        while (true) // Loop para repetir o comportamento indefinidamente
        {
            InvokeRepeating(nameof(CriarEspinhos), 0f, intervaloEntreEspinhos); // Começa a invocar os espinhos
            yield return new WaitForSeconds(duracaoAtiva); // Espera pelo período de atividade
            CancelInvoke(nameof(CriarEspinhos)); // Cancela a invocação dos espinhos
            yield return new WaitForSeconds(duracaoPausa); // Espera pelo período de pausa
        }
    }

    private void CriarEspinhos()
    {
        if (jogador != null)
        {
            // Cria espinhos diretamente abaixo da posição atual do jogador
            Instantiate(espinhosPrefab, new Vector3(jogador.position.x, jogador.position.y, jogador.position.z), Quaternion.identity);
        }
    }
}
