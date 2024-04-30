using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRaio : MonoBehaviour
{
    [SerializeField] private GameObject laserPrefab; // Prefab do raio laser
    [SerializeField] private Transform[] pontos; // Pontos pelos quais o raio deve passar
    [SerializeField] private float velocidade = 5.0f; // Velocidade de movimento do raio
    private GameObject laserInstance; // Inst�ncia do raio laser
    private int indiceAtual = 0; // �ndice atual no array de pontos
    private bool indo = true; // Dire��o do movimento

    void Start()
    {
        StartCoroutine(AtivarLaser());
    }

    private IEnumerator AtivarLaser()
    {
        laserInstance = Instantiate(laserPrefab, pontos[0].position, Quaternion.identity);
        // Ajustar a rota��o inicial do raio
        laserInstance.transform.up = (pontos[1].position - pontos[0].position).normalized;

        while (true)
        {
            // Mover o raio laser para o pr�ximo ponto
            while (Vector3.Distance(laserInstance.transform.position, pontos[indiceAtual].position) > 0.1f)
            {
                laserInstance.transform.position = Vector3.MoveTowards(laserInstance.transform.position, pontos[indiceAtual].position, velocidade * Time.deltaTime);
                // Manter o raio alinhado com a dire��o de movimento
                if (indo && indiceAtual < pontos.Length - 1)
                    laserInstance.transform.up = (pontos[indiceAtual + 1].position - laserInstance.transform.position).normalized;
                else if (!indo && indiceAtual > 0)
                    laserInstance.transform.up = (pontos[indiceAtual - 1].position - laserInstance.transform.position).normalized;

                yield return null;
            }

            // Atualizar o �ndice para o pr�ximo ponto ou para o ponto anterior dependendo da dire��o
            if (indo)
            {
                if (indiceAtual < pontos.Length - 1)
                    indiceAtual++;
                else
                {
                    indo = false; // Inverter a dire��o no �ltimo ponto
                    laserInstance.transform.up = (pontos[indiceAtual - 1].position - laserInstance.transform.position).normalized;
                    yield return new WaitForSeconds(3); // Esperar antes de come�ar a mover de volta
                }
            }
            else
            {
                if (indiceAtual > 0)
                    indiceAtual--;
                else
                {
                    indo = true; // Inverter a dire��o no primeiro ponto
                    laserInstance.transform.up = (pontos[1].position - pontos[0].position).normalized;
                    yield return new WaitForSeconds(3); // Esperar antes de come�ar a mover para frente novamente
                }
            }
        }
    }
}
