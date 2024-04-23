using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    up = 0,
    left,
    down,
    right
}

public class DungeonCrawlerController
{
    public static readonly Dictionary<Direction, Vector2Int> directionMovementMap = new Dictionary<Direction, Vector2Int>
    {
        {Direction.up, Vector2Int.up},
        {Direction.left, Vector2Int.left},
        {Direction.down, Vector2Int.down},
        {Direction.right, Vector2Int.right},
    };

    public static List<Vector2Int> GenerateDungeon(DungeonGenerationData dungeonData)
    {
        List<DungeonCrawler> dungeonCrawlers = new List<DungeonCrawler>();
        List<Vector2Int> positionVisited = new List<Vector2Int>();

        for(int i = 0; i < dungeonData.numberOfCrawlers; i++)
        {
            dungeonCrawlers.Add(new DungeonCrawler(Vector2Int.zero));
        }

        //Número de interações
        int totalRooms = Random.Range(dungeonData.roomsMin, dungeonData.roomsMax);        
        //Loop que faz os crawlers se moverem e adicionarem novas coordenadas
        Debug.Log("Total Commom Rooms: "+totalRooms);
        while(positionVisited.Count < totalRooms)
        {
            foreach(DungeonCrawler dungeonCrawler in dungeonCrawlers)
            {
                Vector2Int newPosition = dungeonCrawler.Move(directionMovementMap);
                //Essa condição pode mudar se eu sempre adicionar a sala inicial a positionVIsited, e tratar cada tipo de sala separadamente
                if((newPosition != Vector2Int.zero) && !positionVisited.Exists(pos => pos.x == newPosition.x && pos.y == newPosition.y))
                {
                    // Debug.Log("Nova Position X "+newPosition.x+" Y "+newPosition.y);
                    positionVisited.Add(newPosition);
                    if(positionVisited.Count >= totalRooms){
                        break;
                    }
                }
            }
        }

        return positionVisited;
    }
}
