using System.Collections;
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

    public Player player;

    public bool isGamePaused = false;

    //Função chamada antes do Start, Caso já exista um GameController na Scene, o novo GameController se auto Destroy
    //Caso contrário ele não é destruído ao carregar uma nova Scene
    void Awake(){
        SceneManager.sceneLoaded += OnSceneLoaded;

        if (instance == null){
            instance = this;
            DontDestroyOnLoad(this);
        }
        else{
            Destroy(gameObject);
            return;
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        currentWorldLevel = 0;
        currentLevel = 0;
        FindPlayer();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Menu")
        {
            Destroy(gameObject);
            return;
        }
    }

    public void PauseGame()
    {
        //Retorna se já estiver pausado
        if(isGamePaused)
            return;
        
        isGamePaused = true;
        PlayerAcao(false);
    }

    public void UnpauseGame(){
        //Retorna se não estiver pausado
        if(!isGamePaused)
            return;
        
        isGamePaused = false;
        PlayerAcao(true);
    }

    [ContextMenu("Next Level")]
    public void NextLevel()
    {
        //Se a cena atual for Hub
        if(currentWorldLevel == 0)
        {
            //Setup world level
            currentWorldLevel = 1;
            currentLevel = 1;
            currentWorldName = worldNames[currentWorldLevel];

            //Set player
            player.SetLightState(true);

            //Carrega o próximo nível
            LoadScene("Dungeon");
            player.ResetPlayer();
        }
        //Se a cena atual não for o Hub
        else{
            switch(currentLevel)
            {
                case 1:
                    currentLevel++;
                    //Carrega o próximo nível
                    LoadScene("Dungeon");
                    player.ResetPlayer();
                    break;
                case 2:
                    currentLevel++;
                    StartCoroutine(WaitLoadScene("Boss"+currentWorldName, "EnableBoss"));
                    break;
                case 3:
                    //Depois de Derrotar o Boss
                    //Se houver mais Mundos, passa para o próximo
                    if(currentWorldLevel+1 < worldNames.Count)
                    {
                        currentWorldLevel++;
                        currentLevel = 1;
                    }else{
                        LoadScene("EndScene");
                        Destroy(player.gameObject);
                        Destroy(FindAnyObjectByType<PauseMenu>().gameObject);
                        Destroy(FindAnyObjectByType<UI_Controll>().gameObject);
                        Destroy(gameObject);
                        return;
                    }
                    break;
                default:
                    break;
            }
        }
    }

    public void LoadScene(string sceneName)
    {
        //Unload todas as cenas adicionais
        for(int i=1;i<SceneManager.sceneCount;i++)
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i));

        SceneManager.LoadScene(sceneName);
    }

    IEnumerator WaitLoadScene(string sceneName, string methodName)
    {
        //Unload todas as cenas adicionais
        for(int i=1;i<SceneManager.sceneCount;i++)
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i));

        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        while(!async.isDone){
            Debug.Log("Loading Scene");
            yield return null;
        }

        Invoke(methodName, 0.1f);
    }

    private void EnableBoss()
    {
        player.ResetPlayer();
        player.transform.position = new Vector3(0,-5.5f,0);
        FindObjectOfType<BossController>().enabled = true;
    }

    public void GoToHub()
    {
        //Reseta Fase
        currentWorldLevel = 0;
        currentLevel = 0;
        currentWorldName = worldNames[currentWorldLevel];

        //Carrega a cena Hub
        LoadScene("Hub");

        //Reseta o Player
        player.ResetPlayer();
        player.SetLightState(false);
        UnpauseGame();
    }

    private void FindPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public void PlayerAcao(bool estado)
    {
        foreach (PlayerMovement playerMovement in player.GetComponentsInChildren<PlayerMovement>())
        {
            playerMovement.canMove = estado;
        }

        foreach (PlayerAttack playerAttack in player.GetComponentsInChildren<PlayerAttack>())
        {
            playerAttack.canAttack = estado;
        }

        foreach (var enemy in FindObjectsOfType<EnemyMovementAndHealth>())
        {
            enemy.canMove = estado;
        }

        foreach (var enemy in FindObjectsOfType<EnemyAttack>())
        {
            enemy.canAttack = estado;
        }
    }

    public void EnablePlayerInput()
    {
        foreach (PlayerMovement playerMovement in player.GetComponentsInChildren<PlayerMovement>())
        {
            playerMovement.canMove = true;
        }

        foreach (PlayerAttack playerAttack in player.GetComponentsInChildren<PlayerAttack>())
        {
            playerAttack.canAttack = true;
        }
    }

    public void DisablePlayerInput()
    {
        foreach (PlayerMovement playerMovement in player.GetComponentsInChildren<PlayerMovement>())
        {
            playerMovement.canMove = false;
        }

        foreach (PlayerAttack playerAttack in player.GetComponentsInChildren<PlayerAttack>())
        {
            playerAttack.canAttack = false;
        }
    }

}