using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;

    public TextMeshProUGUI danoDoJogador;
    public TextMeshProUGUI contadorDeMortes;
    public TextMeshProUGUI tempoDeJogo;

    private bool isPaused = false;
    private PlayerHealth playerHealth;
    private PlayerAttack playerAttack;
    private PlayerMana playerMana;

    private static PauseMenu instance;
    private float startTime; 

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }

        playerHealth = FindObjectOfType<PlayerHealth>();
        playerAttack = FindObjectOfType<PlayerAttack>();
        playerMana = FindObjectOfType<PlayerMana>();    

        startTime = Time.time; 
        UpdateUI();
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            Destroy(gameObject);
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        if (!isPaused)
        {
            UpdateGameTime();
        }
    }

    public void Resume()
    {
        if (pauseMenuUI != null)
        {
            ButtonUI buttonUI = FindObjectOfType<ButtonUI>();
            buttonUI.ResetToOriginalSprite();
            pauseMenuUI.SetActive(false);
        }
        Time.timeScale = 1f;
        isPaused = false;

        GameController.instance.SetPlayerInput(true);
    }

    public void Quit()
    {
        ButtonUI buttonUI = FindObjectOfType<ButtonUI>();
        buttonUI.ResetToOriginalSprite();
    }

    void Pause()
    {
        if (pauseMenuUI != null)
        {
            UpdateUI();
            pauseMenuUI.SetActive(true);
            GameController.instance.SetPlayerInput(false);
        }
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void mainMenu()
    {
        Time.timeScale = 1f;
        startTime = Time.time; 
        playerAttack.danoAtaque = 1;
        playerHealth.contadorMortes = 0;
        playerHealth.maxHealth = 5;
        playerMana.maxMana = 100;

        DialogueManager.EventEndDialogue -= GameController.instance.EnablePlayerInput;
        GameController.instance.LoadScene("Menu");
    }


    private void UpdateUI()
    {
        if (playerHealth != null)
        {
            contadorDeMortes.text = playerHealth.contadorMortes.ToString();
        }

        if (playerAttack != null)
        {
            danoDoJogador.text = playerAttack.danoAtaque.ToString();
        }
    }

    private void UpdateGameTime()
    {
        float gameTime = Time.time - startTime;
        int minutes = Mathf.FloorToInt(gameTime / 60f);
        tempoDeJogo.text = "Tempo de jogo: " + minutes + "m";
    }

}
