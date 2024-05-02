using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DungeonGenerationData.asset", menuName = "LevelData/TileList Data", order = 0)]
public class TilesListData : ScriptableObject
{
    public List<GameObject> tiles;
}
