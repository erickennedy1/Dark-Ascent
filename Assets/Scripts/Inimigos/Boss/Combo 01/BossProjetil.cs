using UnityEngine;

public class BossProjetil : MonoBehaviour
{
    [SerializeField] private GameObject projetilPrefab;
    [SerializeField] private GameObject projetilDestrutivelPrefab;
    [SerializeField] private int quantidadeProjetis = 30;
    [SerializeField] private float velocidadeProjetil = 5f;
    private float deslocamentoInicial = 4f;
    private float anguloInicio = 15f;
    private float anguloFim = -195f;
    public int vezesParaDisparar = 3;
    private float intervaloDisparos = 3f;

    [SerializeField] private Orbe orbe;
    public BossController bossController;

    private int vezesDisparadas = 0;

    void Start()
    {
        InvokeRepeating(nameof(VerificarDisparo), 0f, intervaloDisparos);
    }

    private void VerificarDisparo()
    {
        if (orbe.prontoParaAtacar)
        {
            DispararEmTodasAsDirecoes();
        }
    }

    private void DispararEmTodasAsDirecoes()
    {
        if (vezesDisparadas < vezesParaDisparar)
        {
            float angulo = anguloInicio;
            float anguloIncremento = (anguloFim - anguloInicio) / quantidadeProjetis;

            int indiceDestrutivel = Random.Range(0, quantidadeProjetis - 1);

            for (int i = 0; i < quantidadeProjetis; i++)
            {
                GameObject projetil;
                Vector2 direcao = new Vector2(Mathf.Cos(angulo * Mathf.Deg2Rad), Mathf.Sin(angulo * Mathf.Deg2Rad));
                Vector2 posicaoProjetil = (Vector2)transform.position + direcao * deslocamentoInicial;

                if (i == indiceDestrutivel || i == (indiceDestrutivel + 1) % quantidadeProjetis)
                {
                    projetil = Instantiate(projetilDestrutivelPrefab, posicaoProjetil, Quaternion.identity);
                }
                else
                {
                    projetil = Instantiate(projetilPrefab, posicaoProjetil, Quaternion.identity);
                }

                Rigidbody2D rb = projetil.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.velocity = direcao * velocidadeProjetil;
                }

                angulo += anguloIncremento;
            }
            vezesDisparadas++;
        }
        else
        {
            Debug.Log("Oiii");
            CancelInvoke(nameof(VerificarDisparo));
            bossController.Teste(false);
            orbe.gameObject.SetActive(false);
        }
    }
}
