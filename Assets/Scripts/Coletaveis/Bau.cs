using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bau : MonoBehaviour
{
    public Animator animator;
    public GameObject itemPrefab;
    public Transform spawnPoint;
    private PlayerHealth playerHealth;

    public bool bauDano = false;
    private bool playerProximo = false;
    private bool bauAberto = false;

    private float jumpHeight = 2f;
    private float jumpDistance = 2f;
    private float jumpDuration = 1.2f;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerHealth = player.GetComponent<PlayerHealth>();
        }
    }
    private void Update()
    {
        if (playerProximo && Input.GetKeyDown(KeyCode.E) && !bauAberto)
        {
            if(!bauDano)
            {
                AbrirBau();
                bauAberto = true;
                UI_Aviso.Instance.SetAviso(false, string.Empty);
            }
            else
            {
                playerHealth.TakeDamage(1);
                AbrirBau();
                bauAberto = true;
                UI_Aviso.Instance.SetAviso(false, string.Empty);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerProximo = true;
            if (!bauAberto)
            {
                UI_Aviso.Instance.SetAviso(true, UI_Aviso.Instance.text[0]);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerProximo = false;
            UI_Aviso.Instance.SetAviso(false, string.Empty);
        }
    }

    void AbrirBau()
    {
        animator.SetTrigger("Abrir");
        GameObject item = Instantiate(itemPrefab, spawnPoint.position, Quaternion.identity);
        StartCoroutine(PuloEfeito(item));
    }

    IEnumerator PuloEfeito(GameObject item)
    {
        float timer = 0;
        Vector3 startPos = item.transform.position;
        Vector3 endPos = startPos + new Vector3(jumpDistance, 0, 0);

        while (timer < jumpDuration)
        {
            float height = jumpHeight * Mathf.Sin(Mathf.PI * (timer / jumpDuration));
            item.transform.position = Vector3.Lerp(startPos, endPos, (timer / jumpDuration)) + new Vector3(0, height, 0);
            timer += Time.deltaTime;
            yield return null;
        }

        item.transform.position = endPos;
    }
}
