using System.Collections.Generic;
using UnityEngine;

public class DungeonGeneration : MonoBehaviour
{
    public DungeonGenerationData dungeonGenerationData;
    private List<Vector2Int> dungeonRooms;

    private void Start(){
        //Carrega os dados daquele andar
        dungeonGenerationData = (DungeonGenerationData)Resources.Load("Data/DungeonData/Data"+GameController.instance.currentWorldName+GameController.instance.currentLevel);

        //Gera a lista de coordenadas das Salas a partir dos Crawlers
        dungeonRooms = DungeonCrawlerController.GenerateDungeon(dungeonGenerationData);
        //Gera as Rooms
        SpawnRooms(dungeonRooms);
    }

    private void SpawnRooms(IEnumerable<Vector2Int> rooms){
        //--------Variaveis
        //Organiza a lista pela distancia em realação a coordenada (0,0), de forma decrescente.
        dungeonRooms.Sort((v1,v2) => (v2-Vector2Int.zero).sqrMagnitude.CompareTo((v1-Vector2Int.zero).sqrMagnitude));
        //gateRoom já foi criada?
        bool gateRoom = false; 
        //Crawler usado para cria a sala gate
        DungeonCrawler dungeonCrawler;

        //--------Carrega as Rooms
        //Start Rooms
        RoomController.instance.LoadRoom("Start",0,0);

        //Empty and Gate Rooms
        foreach(Vector2Int roomLocation in rooms)
        {
            //Empty Room
            RoomController.instance.LoadRoom("Empty", roomLocation.x, roomLocation.y);

            //Gate Room
            if(!gateRoom)
            {
                dungeonCrawler = new DungeonCrawler(roomLocation);
                List<Vector2Int> gatePosition = dungeonCrawler.GetNearPositions(DungeonCrawlerController.directionMovementMap);
                while(gatePosition.Count > 0 && !gateRoom){
                    if(CanBeGateRoom(gatePosition[0]))
                    {
                        //Gate Room
                        gateRoom = true;
                            RoomController.instance.LoadRoom("Gate", gatePosition[0].x, gatePosition[0].y);
                    }else{
                        gatePosition.RemoveAt(0);
                    }
                }
            }  
        }
    
    }

    //Função que verifica se aquela posição pode ser a Sala com Portão
    private bool CanBeGateRoom(Vector2Int position){
        //Recebe quais entradas possuem portas
        string type = GetTypeRoom(position);
        //Retorna verdadeiro se:
        //1. A Distancia da coord for maior que 1 (Ou seja, nem a sala Start nem conectada a ela)
        //2. Se já não possue uma sala ali
        //3. Se caso a sala for criada só esteja conectada a uma única sala
        return (Vector2.Distance(position, Vector2.zero) > 1) && !dungeonRooms.Exists(pos => pos.x == position.x && pos.y == position.y) && (type == "1000" || type == "0100" || type == "0010" || type == "0001");
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