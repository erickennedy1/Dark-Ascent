using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Olhos : MonoBehaviour
{
    public Transform player; // Refer�ncia ao Transform do jogador
    public float maxDistance = 0.4f; // Dist�ncia m�xima que a pupila pode se mover do centro

    private Vector2 originalPosition; // Posi��o original do centro da pupila

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        originalPosition = transform.localPosition; // Guarda a posi��o local original da pupila
    }

    void Update()
    {
        Vector2 direction = player.position - transform.parent.position; // Dire��o do jogador
        direction = Vector2.ClampMagnitude(direction, maxDistance); // Limita o movimento da pupila

        transform.localPosition = originalPosition + direction; // Atualiza a posi��o local da pupila
    }
}

