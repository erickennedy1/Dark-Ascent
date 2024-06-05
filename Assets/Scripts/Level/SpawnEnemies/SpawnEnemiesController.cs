using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemiesController : MonoBehaviour
{
    public SpawnEnemiesData enemiesData; // Referência ao prefab do inimigo
    public Collider2D spawnArea = null; // Collider que define a área de spawn
    public LayerMask forbiddenAreaLayer; // Camada dos obstáculos para detecção de colisão
    public int MaxEnemies = 3; // Número total de inimigos
    private int currentEnemies = 0;

    private bool hasEnemies;

    void Start()
    {
        spawnArea = GetComponent<Collider2D>();
    }

    void Update()
    {
        if(hasEnemies)
            CheckEnemiesAlive();
    }

    public void SpawnEnemies()
    {
        hasEnemies = true;
        Vector3 spawnPosition;
        int totalEnemies = Random.Range(1,MaxEnemies+1);
        // Verifica se o número atual de inimigos é menor que o máximo
        while (currentEnemies < totalEnemies)
        {
            // Gera uma posição de spawn aleatória
            spawnPosition = GetRandomSpawnPosition();

            // Verifica se há colisões na posição de spawn
            if (!IsColliding(spawnPosition))
            {
                // Instancia um inimigo na posição de spawn
                Instantiate(enemiesData.Enemies[Random.Range(0,enemiesData.Enemies.Count)], spawnPosition, Quaternion.identity, transform);
                currentEnemies++;
            }
        }
    }

    void CheckEnemiesAlive()
    {
        if(transform.childCount == 0){
            RoomController.instance.EndBattle();
            gameObject.SetActive(false);
        }
    }

    Vector2 GetRandomSpawnPosition()
    {
        float spawnOffSet = 1f;
        //Gera uma posição aleatória dentro da área de spawn
        return new Vector2(
            Random.Range(spawnArea.bounds.min.x+spawnOffSet, spawnArea.bounds.max.x-spawnOffSet),
            Random.Range(spawnArea.bounds.min.y+spawnOffSet, spawnArea.bounds.max.y-spawnOffSet)
        );
    }

    bool IsColliding(Vector2 position)
    {
        return Physics2D.OverlapCircle(position, 1f, forbiddenAreaLayer) != null;
    }

    public void SetEnemiesState(bool state){
        foreach (EnemyMovementAndHealth enemy in GetComponentsInChildren<EnemyMovementAndHealth>())
        {
            enemy.SetCanMove(state);
        }

        foreach (EnemyAttack enemy in GetComponentsInChildren<EnemyAttack>())
        {
            enemy.SetAttack(state);
        }

        foreach (PlantaCarnivoraAttack enemy in GetComponentsInChildren<PlantaCarnivoraAttack>())
        {
            enemy.SetAttack(state);
        }
    }

    void OnDrawGizmos(){
        Room room = GetComponentInParent<Room>();
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(room.GetRoomCenter(), room.GetInnerArea());

        float spawnOffSet = 1f;
        Gizmos.DrawWireCube(spawnArea.bounds.center, spawnArea.bounds.size - new Vector3(spawnOffSet,spawnOffSet));
    }
}