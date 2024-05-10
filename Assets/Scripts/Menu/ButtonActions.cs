using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonActions : MonoBehaviour
{
    public bool playGame;
    public bool opcoes;
    public bool creditos;
    public bool sair;

    public GameObject opcoesPainel;
    public GameObject creditosPainel;
    public GameObject telaInicial;

    private Button buttonScript;

    void Start()
    {
        buttonScript = gameObject.GetComponent<Button>();
    }

    void OnMouseDown()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.mouseClick);

        if (playGame)
        {
            PlayGame();
        }
        if (opcoes)
        {
            ToggleGameObject(opcoesPainel, telaInicial);
        }
        if (creditos)
        {
            ToggleGameObject(creditosPainel, telaInicial);
        }
        if (sair)
        {
            QuitGame();
        }
    }

    private void PlayGame()
    {
        Debug.Log("Iniciando o jogo...");
        SceneManager.LoadScene("Hub");
    }

    private void ToggleGameObject(GameObject panel, GameObject inicialPanel)
    {
        panel.SetActive(!panel.activeSelf);
        inicialPanel.SetActive(!panel.activeSelf);
        buttonScript.ResetToOriginalSprite();
    }

    private void QuitGame()
    {
        Debug.Log("Saindo do jogo...");
        Application.Quit();
    }
}
