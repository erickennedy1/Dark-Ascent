using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEspinhos : MonoBehaviour
{
    [SerializeField] private GameObject espinhosPrefab; // Prefab dos espinhos que surgir�o abaixo do jogador
    [SerializeField] private float intervaloEntreEspinhos = 0.5f; // Intervalo de tempo entre cada surgimento de espinhos
    [SerializeField] private float duracaoAtiva = 10f; // Dura��o em segundos que os espinhos continuam surgindo
    [SerializeField] private float duracaoPausa = 5f; // Dura��o da pausa entre as s�ries de surgimentos

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
            InvokeRepeating(nameof(CriarEspinhos), 0f, intervaloEntreEspinhos); // Come�a a invocar os espinhos
            yield return new WaitForSeconds(duracaoAtiva); // Espera pelo per�odo de atividade
            CancelInvoke(nameof(CriarEspinhos)); // Cancela a invoca��o dos espinhos
            yield return new WaitForSeconds(duracaoPausa); // Espera pelo per�odo de pausa
        }
    }

    private void CriarEspinhos()
    {
        if (jogador != null)
        {
            // Cria espinhos diretamente abaixo da posi��o atual do jogador
            Instantiate(espinhosPrefab, new Vector3(jogador.position.x, jogador.position.y, jogador.position.z), Quaternion.identity);
        }
    }
}
