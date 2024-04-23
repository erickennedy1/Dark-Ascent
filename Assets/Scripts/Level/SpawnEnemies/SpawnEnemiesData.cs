using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemiesList", menuName = "LevelData/Enemies List", order = 1)]
public class SpawnEnemiesData : ScriptableObject
{
    public List<GameObject> Enemies;
}
