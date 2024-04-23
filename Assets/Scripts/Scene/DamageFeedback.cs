using System.Collections;
using UnityEngine;

public class DamageFeedback : MonoBehaviour
{
    [Header("Material p quando estiver normal")]
    public Material defaultMaterial;
    [Header("Material p quando sofrer dano")]
    public Material damageMaterial;
    private float damageDuration = 0.2f; 
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage()
    {
        StopCoroutine("FlashDamage"); 
        StartCoroutine("FlashDamage"); 
    }

    IEnumerator FlashDamage()
    {
        spriteRenderer.material = damageMaterial;

        yield return new WaitForSeconds(damageDuration);

        spriteRenderer.material = defaultMaterial;
    }
}
