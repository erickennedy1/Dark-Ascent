using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Configura��es de Vida")]
    public int maxHealth = 5;
    public GameObject healthIconPrefab;
    private List<GameObject> healthIcons = new List<GameObject>();
    private int currentHealth;
    private Transform healthLayoutGroup;
    private PlayerMana playerMana;
    private Animator animator;

    void Start()
    {
        healthLayoutGroup = GameObject.Find("Vida_Layout").transform;
        playerMana = GetComponent<PlayerMana>();

        animator = GetComponent<Animator>();

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

    private void ResetHealthIcons()
    {
        foreach (GameObject icon in healthIcons)
        {
            Destroy(icon);
        }
        healthIcons.Clear();
    }

    private void InitializeHealthIcons()
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
        Debug.Log("Player Died");
        animator.SetTrigger("Morrendo");
    }

    public void Morrendo()
    {
        playerMana.ResetMana();                 
        GameController.instance.GoToHub();        
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
