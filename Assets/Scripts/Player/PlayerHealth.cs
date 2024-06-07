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
    public int currentHealth;
    private Transform healthLayoutGroup;
    private PlayerMana playerMana;
    private Animator animator;
    private PlayerAttack playerAttack;
    private Collider2D playerCollider;

    [SerializeField] private RadialFadeController fadeController;

    private bool isInvencible = false;
    private float timeInvencible = 0.5f;

    public int contadorMortes = 0;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Material materialDefault;
    [SerializeField] private Material materialDamage;

    void Start()
    {
        healthLayoutGroup = GameObject.Find("Vida_Layout").transform;
        playerMana = GetComponent<PlayerMana>();
        playerAttack = GetComponent<PlayerAttack>();
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (playerMana == null)
        {
            Debug.LogError("Componente PlayerMana nao encontrado!");
        }
        if (healthLayoutGroup == null)
        {
            Debug.LogError("Nao foi poss�vel encontrar o objeto 'Vida_Layout'!");
            return;
        }
        currentHealth = maxHealth;
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
        //Se o player estiver incencivel, não recebe dano
        if(isInvencible)
            return;

        isInvencible = true;
        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

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
            isInvencible = false;
            Die();
        }else{
            StartCoroutine(InvencibleTime());
        }
    }

    private IEnumerator InvencibleTime()
    {
        isInvencible = true;
        spriteRenderer.material = materialDamage;
        yield return new WaitForSeconds(timeInvencible);
        spriteRenderer.material = materialDefault;
        isInvencible = false;
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
        GameController.instance.SetPlayerInput(false);
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
        GameController.instance.SetPlayerInput(true);
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
        }
    }


    void OnEnable()
    {
        currentHealth = maxHealth;
        ResetHealthIcons();
        InitializeHealthIcons();
    }
}
