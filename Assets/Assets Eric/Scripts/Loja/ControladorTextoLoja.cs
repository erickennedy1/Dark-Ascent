using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ControladorTextoLoja : MonoBehaviour
{
    public TMP_Text textoLojaTMP;
    public GameObject painelTextoLoja;
    public List<ItemDaLoja> itens;

    private void Start()
    {
        painelTextoLoja.SetActive(false);
    }

    public void AtualizarTextoLoja(int idItem)
    {
        if (idItem >= 0 && idItem < itens.Count)
        {
            ItemDaLoja item = itens[idItem];
            string textoParaComprar = "\nPressione F para comprar"; 
            textoLojaTMP.text = $"Nome: {item.nome}\nDescrição: {item.descricao}\nAtributos: {item.atributos}\nPreço: {item.preco}{textoParaComprar}";
            painelTextoLoja.SetActive(true);
        }
    }

    public void AtualizarFalaVendedor(int idItem)
    {
        if (idItem >= 0 && idItem < itens.Count)
        {
            ItemDaLoja item = itens[idItem];
            textoLojaTMP.text = $"Nome: {item.nome}\nDescrição: {item.descricao}";
            painelTextoLoja.SetActive(true);
        }
    }

    public void DesativarPainel()
    {
        painelTextoLoja.SetActive(false);
    }
}
