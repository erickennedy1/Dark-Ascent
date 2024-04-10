using System.Collections.Generic;
using UnityEngine;

public class DungeonGeneration : MonoBehaviour
{
    public DungeonGenerationData dungeonGenerationData;
    private List<Vector2Int> dungeonRooms;

    private void Start(){
        //Carrega os dados daquele andar
        dungeonGenerationData = (DungeonGenerationData)Resources.Load("DungeonData/Data"+GameController.instance.currentWorldName+GameController.instance.currentLevel);

        //Gera a lista de coordenadas das Salas a partir dos Crawlers
        dungeonRooms = DungeonCrawlerController.GenerateDungeon(dungeonGenerationData);
        //Gera as Rooms
        SpawnRooms(dungeonRooms);
    }

    private void SpawnRooms(IEnumerable<Vector2Int> rooms){
        //--------Variaveis
        //Organiza a lista pela distancia em realação a coordenada (0,0), de forma decrescente.
        dungeonRooms.Sort((v1,v2) => (v2-Vector2Int.zero).sqrMagnitude.CompareTo((v1-Vector2Int.zero).sqrMagnitude));
        //BossRoom já foi criada?
        bool gateRoom = false; 
        //Crawler usado para cria a sala do boss
        DungeonCrawler dungeonCrawler;

        //--------Carrega as Rooms
        //Start Rooms
        RoomController.instance.LoadRoom("Start",0,0);

        //Empty and Boss Rooms
        foreach(Vector2Int roomLocation in rooms)
        {
            if(!gateRoom)
            {
                dungeonCrawler = new DungeonCrawler(roomLocation);
                List<Vector2Int> bossPosition = dungeonCrawler.GetNearPositions(DungeonCrawlerController.directionMovementMap);
                while(bossPosition.Count > 0 && !gateRoom){
                    if(CanBeGateRoom(bossPosition[0]))
                    {
                        //Boss Room
                        gateRoom = true;
                        if(dungeonGenerationData.isBossLevel)
                            RoomController.instance.LoadRoom("Boss", bossPosition[0].x, bossPosition[0].y);
                        else
                            RoomController.instance.LoadRoom("Gate", bossPosition[0].x, bossPosition[0].y);
                    }else{
                        bossPosition.RemoveAt(0);
                    }
                }
            }                
            //Default Room
            RoomController.instance.LoadRoom("Empty", roomLocation.x, roomLocation.y);
        }
    
    }

    //Função que verifica se aquela posição pode ser a Sala com Portão
    private bool CanBeGateRoom(Vector2Int position){
        //Recebe quais entradas possuem portas
        string type = GetTypeRoom(position);
        //Retorna verdadeiro se:
        //1. Não é a Coordenada Zero (0,0)
        //2. Se já não possue uma sala ali
        //3. Se caso a sala for criada só esteja conectada a uma única sala
        return (position != Vector2Int.zero) && !dungeonRooms.Exists(pos => pos.x == position.x && pos.y == position.y) && (type == "1000" || type == "0100" || type == "0010" || type == "0001");
    }

    //Função que retorna uma string equivalente ao binário de entradas da sala em relação as demais
    //Sendo a ordem: Topo -> Direita -> Baixo -> Esquerda
    private string GetTypeRoom(Vector2Int room){
        string typeRoom = "";
        //Top
        if(dungeonRooms.Exists(pos => pos.x == room.x && pos.y == room.y+1))
            typeRoom += "1";
        else
            typeRoom += "0";
        //Right
        if(dungeonRooms.Exists(pos => pos.x == room.x+1 && pos.y == room.y))
            typeRoom += "1";
        else
            typeRoom += "0";
        //Bottom
        if(dungeonRooms.Exists(pos => pos.x == room.x && pos.y == room.y-1))
            typeRoom += "1";
        else
            typeRoom += "0";
        //Left
        if(dungeonRooms.Exists(pos => pos.x == room.x-1 && pos.y == room.y))
            typeRoom += "1";
        else
            typeRoom += "0";

        return typeRoom;
    }
}