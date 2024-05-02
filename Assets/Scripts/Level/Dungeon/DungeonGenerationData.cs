using UnityEngine;

[CreateAssetMenu(fileName = "DungeonGenerationData.asset", menuName = "LevelData/Dungeon Data", order = 0)]

public class DungeonGenerationData : ScriptableObject
{
    public int numberOfCrawlers;
    public int roomsMin;
    public int roomsMax;
}