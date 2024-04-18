using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    public GameObject projectilePrefab;
    public List<Transform> spawnPoints;
    public float attackTime = 2.0f; // Tempo total de ataque
    public float pauseTime = 3.0f; // Tempo de pausa entre os ciclos de ataque
    public float fireRate = 0.1f; // Tempo entre disparos durante o ataque

    private bool isAttacking = false;

    void Start()
    {
        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        while (true)
        {
            if (isAttacking)
            {
                float timePassed = 0f;
                while (timePassed < attackTime)
                {
                    foreach (Transform spawnPoint in spawnPoints)
                    {
                        Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation);
                    }
                    yield return new WaitForSeconds(fireRate);
                    timePassed += fireRate;
                }
                isAttacking = false;
            }
            else
            {
                yield return new WaitForSeconds(pauseTime);
                isAttacking = true;
            }
        }
    }

    public void AddSpawnPoint(Transform newSpawnPoint)
    {
        spawnPoints.Add(newSpawnPoint);
    }

    public void RemoveSpawnPoint(Transform spawnPoint)
    {
        spawnPoints.Remove(spawnPoint);
    }
}
