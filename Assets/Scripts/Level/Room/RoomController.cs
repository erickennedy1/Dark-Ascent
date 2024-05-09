using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomInfo
{
    public string name;
    public int x, y;
}

public class RoomController : MonoBehaviour
{
    //Váriavel para que esta sala possa ser encontrada de forma pública
    //Chamada pela função Awake
    public static RoomController instance;
    string currentWorldName;
    RoomInfo currentLoadRoomData;
    public Room currentRoom;

    Queue<RoomInfo> loadRoomQueue = new Queue<RoomInfo>();
    public List<Room> loadedRooms = new List<Room>();

    bool isLoadingRoom = false;

    //Depois de carregar todas as Rooms retira as portas
    bool updateRooms = false;

    public GameObject player;
    [SerializeField]MinimapCamera minimapCamera;

    //**Assim que a sala for iniciada** a sala é considerada uma instancia publica
    void Awake() //Awake é chamado primeiro que todos
    {
        instance = this;
    }

    void Start(){
        currentWorldName = GameController.instance.currentWorldName;
    }

    void Update(){
        UpdateRoomQueue();
    }

    void UpdateRoomQueue ()
    {
        if(updateRooms || isLoadingRoom)
            return;

        //Quando todas as Rooms forem carregadas, chama a função pós carregamento
        if(loadRoomQueue.Count == 0){
            AfterLoad();
        }else{
            currentLoadRoomData = loadRoomQueue.Dequeue();
            //Observação: o carregamento só termina quando carregar a Room e a Room chamar a função ResgisterRoom()
            isLoadingRoom = true;

            StartCoroutine(LoadRoomRoutine(currentLoadRoomData));
        }        
    }

    //Função que carrega uma sala em paralelo
    IEnumerator LoadRoomRoutine(RoomInfo info)
    {
        AsyncOperation loadingRoom = SceneManager.LoadSceneAsync("EmptyRoom", LoadSceneMode.Additive);

        while(loadingRoom.isDone == false)
            yield return null;
    }

    public void LoadRoom (string name, int x, int y)
    {
        if(DoesRoomExist(x, y))
            return;

        RoomInfo newRoomData = new RoomInfo{
            name = name,
            x = x,
            y = y
        };

        loadRoomQueue.Enqueue(newRoomData);
    }

    //Função chamada no momento que uma sala é carregada (Pela própria Room no Start())
    public void RegisterRoom(Room room)
    {
        //Verifica se a Room já existe
        if(DoesRoomExist(currentLoadRoomData.x, currentLoadRoomData.y))
        {
            Debug.LogError("Sala X:"+currentLoadRoomData.x+" Y:"+currentLoadRoomData.y+" Já existe");
            Destroy(room.gameObject);
            isLoadingRoom = false;
            return;
        }

        //Nome, x e y, adicionados a RoomInfo da fila (currentLoadRoomData)
        room.name = currentWorldName + "-" + currentLoadRoomData.name + " " + room.x + ", " + room.y;
        room.type = currentLoadRoomData.name;
        room.x = currentLoadRoomData.x;
        room.y = currentLoadRoomData.y;

        //Adiciona a posição baseado na Coord e Tamanho
        room.transform.position = new Vector3(
            room.x * (room.width + room.offset),
            room.y * (room.height + room.offset),
            0
        );
        //Torna a posição relativa ao RoomController
        room.transform.parent = transform;

        //Defini se a Room tem batalha
        if(room.type == "Empty" && Random.Range(0,100) >= 50)
            room.hasBattle = true;
        else
            room.hasBattle = false;

        //Termina de carregar
        isLoadingRoom = false;

        //Posiciona a camera na sala Start (A primeira)
        if(loadedRooms.Count == 0)
            CameraController.instance.currentRoom = room;

        //Adiciona a lista de salas carregadas
        loadedRooms.Add(room);
    }
    
    //Verifica se uma sala existe, baseado na posição
    public bool DoesRoomExist ( int x, int y)
    {
        return loadedRooms.Exists( item => item.x == x && item.y == y);
    }

    //Encontra e retorna uma sala, baseado na posição
    public Room FindRoom (int x, int y)
    {
        return loadedRooms.Find( item => item.x == x && item.y == y);
    }

    //Função que Retorna a direção que possuem portas
    public string GetDoorsDirection(int x, int y){
        string doorsDirection = "";
        //Top
        if(DoesRoomExist(x, y+1))
            doorsDirection += "1";
        else
            doorsDirection += "0";
        //Right
        if(DoesRoomExist(x+1, y))
            doorsDirection += "1";
        else
            doorsDirection += "0";
        //Bottom
        if(DoesRoomExist(x, y-1))
            doorsDirection += "1";
        else
            doorsDirection += "0";
        //Left
        if(DoesRoomExist(x-1, y))
            doorsDirection += "1";
        else
            doorsDirection += "0";

        return doorsDirection;
    }

    //Função chamada quando o player entra na sala
    public void OnPlayerEnterRoom(Room room)
    {
        CameraController.instance.currentRoom = room;
        if (currentRoom != null)
            currentRoom.OnLeave();
        currentRoom = room;
        if (updateRooms)
        {
            UpdateMinimap(room);
        }
    }

    public void UpdateMinimap(Room room)
    {
        if(!room.isKnown)
            room.OnKnow();
        if(!room.isClear)
            room.OnClear();
        
        room.OnEnterRoom();

        //Defini como conhecido todas as salas próximas
        List<Room> nearRooms = new List<Room>
        {
            FindRoom(room.x, room.y + 1),
            FindRoom(room.x + 1, room.y),
            FindRoom(room.x, room.y - 1),
            FindRoom(room.x - 1, room.y)
        };
                
        foreach(Room r in nearRooms){
            if(r != null && !r.isKnown)
                r.OnKnow();
        }

        //Atualiza a Camera
        if(updateRooms)
            minimapCamera.UpdateMinimap();
    }

    //Função que Identifica a próxima sala, baseado na porta tocada, e posiciona o player nessa nova sala
    public void OnPlayerTouchDoor(Room room, Door.DoorType direction)
    {
        Room nextRoom = null;
        Vector3 position = Vector3.zero;
        float offset = 2.5f;
        switch(direction)
        {
            case Door.DoorType.top:
                nextRoom = FindRoom(room.x, room.y+1);
                position = new Vector3(0,-nextRoom.height/2+offset,0);
                break;
            case Door.DoorType.right:
                nextRoom = FindRoom(room.x+1, room.y);
                position = new Vector3(-nextRoom.width/2+offset,0,0);
                break;
            case Door.DoorType.bottom:
                nextRoom = FindRoom(room.x, room.y-1);
                position = new Vector3(0,nextRoom.height/2-offset,0);
                break;
            case Door.DoorType.left:
                nextRoom = FindRoom(room.x-1, room.y);
                position = new Vector3(nextRoom.width/2-offset,0,0);
                break;
            default:
                Debug.LogError("Invalid direction - OnPlayerTouchDoor");
                return;
        }

        //Confirma que a sala foi limpa
        if(!currentRoom.isClear)
            currentRoom.OnClear();

        //Transfere o player de position
        player.transform.position = nextRoom.transform.position + position;
    }

    void AfterLoad(){
        //Atualiza todas as Rooms
        foreach(Room room in loadedRooms)
            room.UpdateRoom();

        //Tudo que precisa ser feito depois de atualizar todas as salas
        //Carrega Minimap_Camera
        Instantiate(Resources.Load("Prefabs/Minimap_Camera", typeof(Camera)));
        minimapCamera = FindObjectOfType<Camera>().GetComponent<MinimapCamera>();

        //Identifica o player
        player = GameObject.FindGameObjectWithTag("Player");
        //Posiciona o player no centro da sala Start
        player.transform.position = loadedRooms.Find(item => item.type == "Start").GetRoomCenter();

        updateRooms = true;
    }
}