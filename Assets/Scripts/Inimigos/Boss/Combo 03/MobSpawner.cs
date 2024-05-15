using UnityEngine;
using System.Collections;

public class InvocacaoChefe : MonoBehaviour
{
    public Transform jogador;
    public GameObject prefabMonstro;
    public float distanciaInvocacao = 5.0f;
    public float duracaoDesvanecimento = 1.0f;

    public int numeroDeMonstros = 5;
    public float tempoEntreInvocacoes = 2.0f;


    public void IniciarInvocacao(int quantidadeMonstros, float atraso)
    {
        numeroDeMonstros = quantidadeMonstros;
        tempoEntreInvocacoes = atraso;
        StartCoroutine(InvocarMonstros(quantidadeMonstros, atraso));
    }

    private IEnumerator InvocarMonstros(int quantidadeMonstros, float atraso)
    {
        for (int i = 0; i < quantidadeMonstros; i++)
        {
            Vector2 posicaoInvocacao = jogador.position + (Vector3)Random.insideUnitCircle.normalized * distanciaInvocacao;
            GameObject monstro = Instantiate(prefabMonstro, posicaoInvocacao, Quaternion.identity);
            StartCoroutine(AparecerMonstro(monstro));
            yield return new WaitForSeconds(atraso);
        }
    }

    private IEnumerator AparecerMonstro(GameObject monstro)
    {
        SpriteRenderer renderizadorMonstro = monstro.GetComponent<SpriteRenderer>();
        EnemyMovementAndHealth movimentoMonstro = monstro.GetComponent<EnemyMovementAndHealth>();
        Color cor = renderizadorMonstro.color;
        float tempoDecorrido = 0f;

        movimentoMonstro.canMove = false;

        while (tempoDecorrido < duracaoDesvanecimento)
        {
            cor.a = Mathf.Lerp(0f, 1f, tempoDecorrido / duracaoDesvanecimento);
            renderizadorMonstro.color = cor;
            tempoDecorrido += Time.deltaTime;
            yield return null;
        }

        cor.a = 1f;
        renderizadorMonstro.color = cor;

        movimentoMonstro.canMove = true;
    }
}
