using UnityEngine;

public class TriggerLoja : MonoBehaviour
{
    public ControladorTextoLoja controladorTextoLoja;
    public int idItem;
    public bool eFalaDoVendedor; 
    private bool jogadorDentro = false; 

    private void Update()
    {
        if (jogadorDentro && Input.GetKeyDown(KeyCode.F) && !eFalaDoVendedor)
        {
            ComprarItem();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorDentro = true; 

            if (eFalaDoVendedor)
            {
                controladorTextoLoja.AtualizarFalaVendedor(idItem);
            }
            else
            {
                controladorTextoLoja.AtualizarTextoLoja(idItem);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorDentro = false;
            controladorTextoLoja.DesativarPainel(); 
        }
    }

    void ComprarItem()
    {
        PlayerAttack playerAttack = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>();
        if (playerAttack != null)
        {
            playerAttack.IncreaseDamage(5); 
        }
        gameObject.SetActive(false); 
    }
}
