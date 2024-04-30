using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRaio : MonoBehaviour
{
    [SerializeField] private GameObject laserPrefab; // Prefab do raio laser
    [SerializeField] private Transform[] pontos; // Pontos pelos quais o raio deve passar
    [SerializeField] private float velocidade = 5.0f; // Velocidade de movimento do raio
    private GameObject laserInstance; // Instância do raio laser
    private int indiceAtual = 0; // Índice atual no array de pontos
    private bool indo = true; // Direção do movimento

    void Start()
    {
        StartCoroutine(AtivarLaser());
    }

    private IEnumerator AtivarLaser()
    {
        laserInstance = Instantiate(laserPrefab, pontos[0].position, Quaternion.identity);
        // Ajustar a rotação inicial do raio
        laserInstance.transform.up = (pontos[1].position - pontos[0].position).normalized;

        while (true)
        {
            // Mover o raio laser para o próximo ponto
            while (Vector3.Distance(laserInstance.transform.position, pontos[indiceAtual].position) > 0.1f)
            {
                laserInstance.transform.position = Vector3.MoveTowards(laserInstance.transform.position, pontos[indiceAtual].position, velocidade * Time.deltaTime);
                // Manter o raio alinhado com a direção de movimento
                if (indo && indiceAtual < pontos.Length - 1)
                    laserInstance.transform.up = (pontos[indiceAtual + 1].position - laserInstance.transform.position).normalized;
                else if (!indo && indiceAtual > 0)
                    laserInstance.transform.up = (pontos[indiceAtual - 1].position - laserInstance.transform.position).normalized;

                yield return null;
            }

            // Atualizar o índice para o próximo ponto ou para o ponto anterior dependendo da direção
            if (indo)
            {
                if (indiceAtual < pontos.Length - 1)
                    indiceAtual++;
                else
                {
                    indo = false; // Inverter a direção no último ponto
                    laserInstance.transform.up = (pontos[indiceAtual - 1].position - laserInstance.transform.position).normalized;
                    yield return new WaitForSeconds(3); // Esperar antes de começar a mover de volta
                }
            }
            else
            {
                if (indiceAtual > 0)
                    indiceAtual--;
                else
                {
                    indo = true; // Inverter a direção no primeiro ponto
                    laserInstance.transform.up = (pontos[1].position - pontos[0].position).normalized;
                    yield return new WaitForSeconds(3); // Esperar antes de começar a mover para frente novamente
                }
            }
        }
    }
}
