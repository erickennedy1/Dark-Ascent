using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    //Variables
    public int width, height, offset, x, y;
    public string doorsDirection, type;

    public MinimapIcon minimapIcon;
    public bool isKnown = false; //Usado pelo minimapa, para identificar salas conhecias (Que se sabe da existencia)
    public bool isClear = false; //Identifica se a sala foi explorada
    public bool hasBattle = false; //Identifica salas com batalha

    //Methods
    void Start(){
        //Verifica se RoomController existe
        if(RoomController.instance == null)
        {
            Debug.LogError("No RoomController.instance founded.");
            return;
        }
        RoomController.instance.RegisterRoom(this);
        minimapIcon = GetComponentInChildren<MinimapIcon>();
    }

    //Chamado quando todas as salas forem carregadas
    public void UpdateRoom(){
        //Recebe quais portas existem
        doorsDirection = RoomController.instance.GetDoorsDirection(x, y);
        RemoveUnconnectDoors();
        UpdateByType();
        
    }

    private void UpdateByType(){
        if(type == "Empty")
        {
            // RandomTilemapEmpty();
            Instantiate(Resources.Load("Prefabs/Tilemaps/Generic0", typeof(GameObject)), transform);
            if(hasBattle){
                gameObject.GetComponentInChildren<SpawnEnemiesController>().SpawnEnemies();
            }
        }
        else if(type == "Start")
        {
            Instantiate(Resources.Load("Prefabs/Tilemaps/Start0", typeof(GameObject)), transform);
            RoomController.instance.OnPlayerEnterRoom(this);
            RoomController.instance.UpdateMinimap(this);
        }
            
        else if(type == "Gate")
        {
            Instantiate(Resources.Load("Prefabs/Tilemaps/Gate0", typeof(GameObject)), transform);
            Instantiate(Resources.Load("Prefabs/Gate", typeof(GameObject)), transform);
        }      
        else if(type == "Boss")
        {
            Instantiate(Resources.Load("Prefabs/Tilemaps/Boss0", typeof(GameObject)), transform);
            Instantiate(Resources.Load("Prefabs/Gate", typeof(GameObject)), transform);
        }
            
    }

    private void RandomTilemapEmpty(){
        //50% de chance de ser uma sala específica
        //50% de chance de ser uma sala Genérica
        int randomType = Random.Range(0,2);

        switch(randomType)
        {
            //Tipo genérico
            case 0:
                Instantiate(Resources.Load("Prefabs/Tilemaps/Generic/Empty"+Random.Range(0,8), typeof(GameObject)), transform);
                break;
            //Tipo Especifico baseado nas portas
            case 1:
                Instantiate(Resources.Load("Prefabs/Tilemaps/"+doorsDirection+"/Empty"+Random.Range(0,2), typeof(GameObject)), transform);
                break;
            default:
                Debug.LogError("randomType Invalid!");
                break;
        }
            
    }

    //Retira Doors desconexas
    private void RemoveUnconnectDoors(){
        if(doorsDirection[0]=='0')
            Instantiate(Resources.Load("Prefabs/Walls/TopWall", typeof(GameObject)), transform);
        else
            Instantiate(Resources.Load("Prefabs/Walls/TopWallDoor", typeof(GameObject)), transform);
        if(doorsDirection[1]=='0')
            Instantiate(Resources.Load("Prefabs/Walls/RightWall", typeof(GameObject)), transform);
        else
            Instantiate(Resources.Load("Prefabs/Walls/RightWallDoor", typeof(GameObject)), transform);
        if(doorsDirection[2]=='0')
            Instantiate(Resources.Load("Prefabs/Walls/BottomWall", typeof(GameObject)), transform);
        else
            Instantiate(Resources.Load("Prefabs/Walls/BottomWallDoor", typeof(GameObject)), transform);
        if(doorsDirection[3]=='0')
            Instantiate(Resources.Load("Prefabs/Walls/LeftWall", typeof(GameObject)), transform);
        else
            Instantiate(Resources.Load("Prefabs/Walls/LeftWallDoor", typeof(GameObject)), transform);
    }

    public Vector2 GetRoomCenter(){
        return new Vector2( x * (width+offset), y * (height+offset));
    }

    public Vector2 GetInnerArea(){
        return new Vector3( width-1, height-1);
    }

    public void OnKnow()
    {
        isKnown = true;
        minimapIcon.Onknow();
    }

    public void OnClear()
    {
        isClear = true;
        minimapIcon.OnClear();
    }

    public void OnLeave()
    {
        minimapIcon.OnLeaveRoom();
    }

    public void OnEnterRoom()
    {
        minimapIcon.OnEnterRoom();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player")
        {
            RoomController.instance.OnPlayerEnterRoom(this);
        }
    }

    void OnDrawGizmos(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(width+offset, height+offset, 0));
    }
}