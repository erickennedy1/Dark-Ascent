using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Olhos : MonoBehaviour
{
    public Transform player; // Referência ao Transform do jogador
    public float maxDistance = 0.4f; // Distância máxima que a pupila pode se mover do centro

    private Vector2 originalPosition; // Posição original do centro da pupila

    void Start()
    {
        originalPosition = transform.localPosition; // Guarda a posição local original da pupila
    }

    void Update()
    {
        Vector2 direction = player.position - transform.parent.position; // Direção do jogador
        direction = Vector2.ClampMagnitude(direction, maxDistance); // Limita o movimento da pupila

        transform.localPosition = originalPosition + direction; // Atualiza a posição local da pupila
    }
}

