using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;

public class PlayerHealth : MonoBehaviour
{
    [Header("Configura��es de Vida")]
    public int maxHealth = 5;
    public GameObject healthIconPrefab;
    private List<GameObject> healthIcons = new List<GameObject>();
    public int currentHealth;
    private Transform healthLayoutGroup;
    private PlayerMana playerMana;
    private Animator animator;
    private PlayerAttack playerAttack;
    private Rigidbody2D rd;
    private PlayerMovement playerMovement;
    private PolygonCollider2D playerCollider;

    [SerializeField]
    private RadialFadeController fadeController;

    public int contadorMortes = 0;

    void Start()
    {
        healthLayoutGroup = GameObject.Find("Vida_Layout").transform;
        playerMana = GetComponent<PlayerMana>();
        playerAttack = GetComponent<PlayerAttack>();
        playerMovement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<PolygonCollider2D>();

        if (playerMana == null)
        {
            Debug.LogError("Componente PlayerMana n�o encontrado!");
        }
        if (healthLayoutGroup == null)
        {
            Debug.LogError("N�o foi poss�vel encontrar o objeto 'Vida_Layout'!");
            return;
        }
        currentHealth = maxHealth;
        Debug.Log("Reiniciando sa�de no Start");
        ResetHealthIcons();
        InitializeHealthIcons();
    }

    public void ResetHealthIcons()
    {
        foreach (GameObject icon in healthIcons)
        {
            Destroy(icon);
        }
        healthIcons.Clear();
    }

    public void InitializeHealthIcons()
    {
        for (int i = 0; i < maxHealth; i++)
        {
            GameObject icon = Instantiate(healthIconPrefab, healthLayoutGroup);
            icon.SetActive(true);
            healthIcons.Add(icon);
        }
    }

    public void TakeDamage(int damageAmount)
    {
        Debug.Log("Dano recebido: " + damageAmount);
        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log("Sa�de atual: " + currentHealth);

        for (int i = 0; i < healthIcons.Count; i++)
        {
            if (healthIcons[i] != null) 
            {
                Animator animator = healthIcons[i].GetComponent<Animator>();
                if (animator != null) 
                {
                    if (i >= currentHealth)
                    {
                        animator.SetTrigger("VidaQuebrando");
                    }
                }
            }
        }

        if (currentHealth <= 0)
        {
            Debug.Log("Sa�de esgotada, chamando Die()");
            Die();
        }
    }


    public void GainHealth(int healAmount)
    {
        int previousHealth = currentHealth;
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        for (int i = previousHealth; i < currentHealth; i++)
        {
            healthIcons[i].SetActive(true);
        }
    }

    void Die()
    {
        // FadeController.StartFade(true);
        fadeController.SetFadeOn();
        ImageFadeController.FadeIn();
        contadorMortes++;
        playerAttack.danoAtaque = 1;
        animator.SetTrigger("Morrendo");
        GameController.instance.PlayerAcao(false);
        playerCollider.enabled = false;
        StartCoroutine(Died());
    }

    public void Morrendo()
    {
        //Espera um pouco antes de ir para o hub
        Invoke("MorrendoDelayCall", 2f);
    }

    private void MorrendoDelayCall()
    {
        GameController.instance.GoToHub();
        StartCoroutine(Reativar());
    }

    private IEnumerator Died()
    {
        yield return new WaitForSeconds(3);
        playerMana.RecuperarMana(100);
        maxHealth = 5;
    }

    private IEnumerator Reativar()
    {
        // FadeController.StartFade(false);
        fadeController.SetFadeOff();
        ImageFadeController.ResetFade();
        yield return new WaitForSeconds(1);
        animator.SetTrigger("Revivendo");
        yield return new WaitForSeconds(2.5f);
        GameController.instance.PlayerAcao(true);
        playerCollider.enabled = true;
    }

    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Hub")
        {
            currentHealth = maxHealth;
            ResetHealthIcons();
            InitializeHealthIcons();
            Debug.Log("Sa�de resetada no carregamento da cena 'Hub'");
        }
    }


    void OnEnable()
    {
        currentHealth = maxHealth;
        ResetHealthIcons();
        InitializeHealthIcons();
        Debug.Log("Sa�de resetada no OnEnable");
    }
}
