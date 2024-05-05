using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    [Tooltip("Lista de Nomes dos níveis - Começando pelo Hub")]
    public List<string> worldNames = new();
    public string currentWorldName;
    public int currentWorldLevel;
    public int currentLevel;

    private Player player;
    private GameObject UI;

    public bool isGamePaused = false;

    //Função chamada antes do Start, Caso já exista um GameController na Scene, o novo GameController se auto Destroy
    //Caso contrário ele não é destruído ao carregar uma nova Scene
    void Awake(){
        UI = GameObject.Find("UI");
        if (instance == null){
            instance = this;
            DontDestroyOnLoad(this);
            DontDestroyOnLoad(UI);
        }
        else{
            Destroy(gameObject);
        }
    }

    void Start()
    {
        currentWorldLevel = 0;
        currentLevel = 0;
        FindPlayer();
    }

    void Update()
    {
        //Se a cena atual não for Hub
        if(currentWorldLevel != 0)
        {
            //Evitar o erro do GameController Tentar encontrar a CameraController antes de ser carregada
            if(CameraController.instance != null)
            {
                //O game fica pausado se a camera estiver em transição
                if(CameraController.instance.isTransitioning)
                    isGamePaused = true;
                else
                    isGamePaused = false;
            }
        }
        //Se a cena atual for Hub
        else
            //Podem ser criados condições para que não se mova no hub
            isGamePaused = false;

        //Caso o jogo estaja pausado
        if(isGamePaused)
            player.canMove = false;
        else
            player.canMove = true;
    }

    public void NextLevel()
    {
        //Se a cena atual for Hub
        if(currentWorldLevel == 0)
        {
            currentWorldLevel = 1;
            currentLevel = 1;
        }
        //Se a cena atual não for o Hub
        else{
            //Unload todas as cenas adicionais
            for(int i=1;i<SceneManager.sceneCount;i++)
                SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i));

            //Próximo nível
            currentLevel++;
            //Se o nível for 2 passa para o próximo cenário
            if(currentLevel > 2)
            {
                if(currentWorldLevel+1 < worldNames.Count)
                {
                    currentLevel = 1;
                    currentWorldLevel++;
                }else{
                    currentWorldLevel = 0;
                    currentLevel = 0;
                    currentWorldName = worldNames[currentWorldLevel];
                    SceneManager.LoadScene("HubScene");
                    FindPlayer();
                    player.ResetPlayer();
                    return;
                }                
            }  
        }

        currentWorldName = worldNames[currentWorldLevel];
        //Carrega o próximo nível
        SceneManager.LoadScene("Dungeon");
        Debug.Log("Dungeon Loaded");
    }

    private void FindPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public void SceneCountTotal(){
        Debug.Log("Total de Scenes: "+SceneManager.sceneCount);
    }
}