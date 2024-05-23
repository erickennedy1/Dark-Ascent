using UnityEngine;

public class Room : MonoBehaviour
{
    //Variables
    public int width, height, offset, x, y;
    public string doorsDirection, type;

    public MinimapIcon minimapIcon;
    public bool isKnown = false; //Usado pelo minimapa, para identificar salas conhecias (Que se sabe da existencia)
    public bool isClear = false; //Identifica se a sala foi explorada
    public bool hasBattle = false; //Identifica salas com batalha.
    private Player player;

    private SpawnEnemiesController spawnEnemiesController;

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
        player = FindObjectOfType<Player>();
        spawnEnemiesController = gameObject.GetComponentInChildren<SpawnEnemiesController>();
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
            RandomTilemapEmpty();
            if(hasBattle){
                spawnEnemiesController.SpawnEnemies();
            }else{
                spawnEnemiesController.gameObject.SetActive(false);
            }
        }
        else{
            RandomTilemap(type);
            spawnEnemiesController.gameObject.SetActive(false);
            switch(type)
            {   
                case "Start":
                    //Prepara a sala Start para ser a primeira sala
                    RoomController.instance.UpdateCamera(this);
                    RoomController.instance.UpdateRoomStates(this);
                    break;
                case "Gate":
                    //Adiciona o Gate
                    Instantiate(Resources.Load("Prefabs/Gate", typeof(GameObject)), transform);
                    break;
                default:
                    Debug.LogError("Type of Room invalid!");
                    break;
            }
        }         
    }

    private void RandomTilemapEmpty(){
        //25% de chance de ser uma sala específica
        //75% de chance de ser uma sala Genérica
        int chanceOfGeneric = 75;
        int randomType = Random.Range(0,100);
        randomType = randomType < chanceOfGeneric ? 0 : 1;

        switch(randomType)
        {
            //Tipo genérico
            case 0:
                RandomTilemap("Generic");
                break;
            //Tipo Especifico baseado nas portas
            case 1:
                RandomTilemap(doorsDirection);
                break;
            default:
                Debug.LogError("randomType Invalid!");
                break;
        }
            
    }

    private void RandomTilemap(string type){
        TilesListData tilesList = (TilesListData)Resources.Load("Prefabs/Tilemaps/"+type+"/List"+type);
        Instantiate(tilesList.tiles[Random.Range(0,tilesList.tiles.Count)], transform);
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

    public void CloseDoors()
    {
        Door[] doors = GetComponentsInChildren<Door>();
        foreach(Door door in doors){
            door.CloseDoors();
        }
    }

    public void OpenDoors()
    {
        Door[] doors = GetComponentsInChildren<Door>();
        foreach(Door door in doors){
            door.OpenDoors();
        }
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