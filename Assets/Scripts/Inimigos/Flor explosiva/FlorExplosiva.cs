using UnityEngine;

public class FlorExplosiva : MonoBehaviour
{
    [SerializeField] private float distancia = 10.0f;
    [SerializeField] private int danoExplosao = 2;
    [SerializeField] private float tempoParaExplodir = 3.5f;

    private Animator animator;
    private Transform player;
    private bool dentroDoTrigger;
    private float tempoInicial;

    private ISoundEnemy soundController;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        dentroDoTrigger = false;
        soundController = GetComponent<ISoundEnemy>();
    }

    void Update()
    {
        if (dentroDoTrigger)
        {
            if (Time.time - tempoInicial > tempoParaExplodir)
            {
                animator.SetBool("Explodindo", true);
                soundController.PlayAttack();
                dentroDoTrigger = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !dentroDoTrigger)
        {
            animator.SetBool("Carregando", true);
            soundController.PlayIdle();
            dentroDoTrigger = true;
            tempoInicial = Time.time; 
        }
    }

    public void CauseDamage()
    {
        if (Vector2.Distance(transform.position, player.position) <= distancia)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(danoExplosao);
            }
        }
    }

    public void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}
