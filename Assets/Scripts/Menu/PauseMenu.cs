using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;

    public TextMeshProUGUI danoDoJogador;
    public TextMeshProUGUI contadorDeMortes;
    public TextMeshProUGUI tempoDeJogo;

    private bool isPaused = false;
    private PlayerHealth playerHealth;
    private PlayerAttack playerAttack;

    private static PauseMenu instance;

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

        ResetPlayerInputs(true);
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
            ResetPlayerInputs(false);
        }
        Time.timeScale = 0f;
        isPaused = true;
    }

    private void ResetPlayerInputs(bool state)
    {
        PlayerAttack playerController = FindObjectOfType<PlayerAttack>();
        if (playerController != null)
        {
            playerController.canAttack = state;
        }

        if (!state)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
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
        float gameTime = Time.time;
        tempoDeJogo.text = "Tempo de jogo: " + gameTime.ToString("F2") + "s";
    }
}
