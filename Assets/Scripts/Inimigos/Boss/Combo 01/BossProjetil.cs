using UnityEngine;

public class BossProjetil : MonoBehaviour
{
    [SerializeField] private GameObject projetilPrefab;
    [SerializeField] private GameObject projetilDestrutivelPrefab;
    [SerializeField] private int quantidadeProjetis = 30;
    [SerializeField] private float velocidadeProjetil = 5f;
    private float deslocamentoInicial = 5f;
    private float anguloInicio = 15f;
    private float anguloFim = -195f;
    public int vezesParaDisparar = 3;
    public float intervaloDisparos = 3f;
    public int vezesDisparadas = 0;

    private void DispararEmTodasAsDirecoes()
    {
        if (vezesDisparadas < vezesParaDisparar)
        {
            float angulo = anguloInicio;
            float anguloIncremento = (anguloFim - anguloInicio) / quantidadeProjetis;
            int indiceDestrutivel = Random.Range(0, quantidadeProjetis - 1);
            int indiceDestrutivelOpsto = (indiceDestrutivel + quantidadeProjetis / 2) % quantidadeProjetis;

            for (int i = 0; i < quantidadeProjetis; i++)
            {
                GameObject projetil;
                Vector2 direcao = new Vector2(Mathf.Cos(angulo * Mathf.Deg2Rad), Mathf.Sin(angulo * Mathf.Deg2Rad));
                Vector2 posicaoProjetil = (Vector2)transform.position + direcao * deslocamentoInicial;

                if (i == indiceDestrutivel || i == (indiceDestrutivel + 1) % quantidadeProjetis ||
                    i == indiceDestrutivelOpsto || i == (indiceDestrutivelOpsto + 1) % quantidadeProjetis)
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
            CancelarDisparos();
        }
    }

    public void ResetarDisparos()
    {
        vezesDisparadas = 0;
    }

    public void AtivarCombo(int disparos, int intervalo, int quantidade)
    {
        vezesParaDisparar = disparos;
        intervaloDisparos = intervalo;
        quantidadeProjetis = quantidade;

        InvokeRepeating(nameof(DispararEmTodasAsDirecoes), 0f, intervaloDisparos);
    }

    public void CancelarDisparos()
    {
        CancelInvoke(nameof(DispararEmTodasAsDirecoes));
    }

    public void CancelarDisparosImediatamente()
    {
        CancelarDisparos();
        ResetarDisparos();
    }
}
