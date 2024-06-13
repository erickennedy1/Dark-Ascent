using System.Collections.Generic;
using UnityEngine;

public class DungeonCrawler
{
    private Vector2Int Position { get; set;}
    public DungeonCrawler(Vector2Int startPosition)
    {
        Position = startPosition;
    }

    public Vector2Int Move(Dictionary<Direction, Vector2Int> directionMovementMap)
    {
        Direction toMove = (Direction)Random.Range(0, directionMovementMap.Count);
        Position += directionMovementMap[toMove];
        return Position;
    }

    public List<Vector2Int> GetNearPositions(Dictionary<Direction, Vector2Int> directionMovementMap)
    {
        List<Vector2Int> allPositions = new List<Vector2Int>
        {
            Position + directionMovementMap[Direction.up],
            Position + directionMovementMap[Direction.right],
            Position + directionMovementMap[Direction.down],
            Position + directionMovementMap[Direction.left]
        };

        List<Vector2Int> temp = new List<Vector2Int>();
        temp.AddRange(allPositions);

        List<Vector2Int> shuffled = new List<Vector2Int>();

        for(int i = 0; i<allPositions.Count;i++)
        {
            int index = Random.Range(0, temp.Count);
            shuffled.Add(temp[index]);
            temp.RemoveAt(index);
        }
        
        return shuffled;
    }
}