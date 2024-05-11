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

    public bool isGamePaused = false;

    //Função chamada antes do Start, Caso já exista um GameController na Scene, o novo GameController se auto Destroy
    //Caso contrário ele não é destruído ao carregar uma nova Scene
    void Awake(){
        if (instance == null){
            instance = this;
            DontDestroyOnLoad(this);
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
        if (isGamePaused)
            PlayerAcao(false);
        else
            PlayerAcao(true);
    }

    public void NextLevel()
    {
        //Se a cena atual for Hub
        if(currentWorldLevel == 0)
        {
            currentWorldLevel = 1;
            currentLevel = 1;
            currentWorldName = worldNames[currentWorldLevel];
            //Carrega o próximo nível
            SceneManager.LoadScene("Dungeon");
        }
        //Se a cena atual não for o Hub
        else{
            //Unload todas as cenas adicionais
            for(int i=1;i<SceneManager.sceneCount;i++)
                SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i));

            switch(currentLevel)
            {
                case 1:
                    currentLevel++;
                    //Carrega o próximo nível
                    SceneManager.LoadScene("Dungeon");
                    player.ResetPlayer();
                    break;
                case 2:
                    currentLevel++;
                    SceneManager.LoadScene("Boss"+currentWorldName);
                    player.ResetPlayer();
                    break;
                case 3:
                    //Depois de Derrotar o Boss
                    //Se houver mais Mundos, passa para o próximo
                    if(currentWorldLevel+1 < worldNames.Count)
                    {
                        currentWorldLevel++;
                        currentLevel = 1;
                    }else{
                        //Se não houver, volta para o Hub
                        //PS.: Preciso adicionar uma cena final
                        GoToHub();
                        return;
                    }
                    break;
                default:
                    break;
            }
        }
    }

    public void GoToHub()
    {
        //Unload todas as cenas adicionais
        for(int i=1;i<SceneManager.sceneCount;i++)
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i));
        
        currentWorldLevel = 0;
        currentLevel = 0;
        currentWorldName = worldNames[currentWorldLevel];
        SceneManager.LoadScene("Hub");
        player.ResetPlayer();
    }

    private void FindPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public void SceneCountTotal(){
        Debug.Log("Total de Scenes: "+SceneManager.sceneCount);
    }

    public void PlayerAcao(bool estado)
    {
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        PlayerAttack playerAttack = player.GetComponent<PlayerAttack>();

        if (playerMovement != null)
            playerMovement.canMove = estado;
        if (playerAttack != null)
            playerAttack.canAttack = estado;

        foreach (var enemy in FindObjectsOfType<EnemyMovementAndHealth>())
        {
            enemy.canMove = estado;
        }

        foreach (var enemy in FindObjectsOfType<EnemyAttack>())
        {
            enemy.canAttack = estado;
        }

        foreach (var enemy in FindObjectsOfType<PlantaCarnivoraAttack>())
        {
            enemy.canAttack = estado;  
        }
    }


}