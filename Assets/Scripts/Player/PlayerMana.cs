using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMana : MonoBehaviour
{
    [Header("Configurações de Mana")]
    public int maxMana = 100;
    public int currentMana;
    private Animator manaAnimator;
    private string manaAnimationStateName = "Mana_animacao"; 

    void Start()
    {
        manaAnimator = GameObject.Find("UI_vida_e_mana").GetComponent<Animator>();
        currentMana = maxMana;
        SetManaAnimation(currentMana);

        //DontDestroyOnLoad(manaAnimator);
    }

    public void UsarMana(int amount)
    {
        if (currentMana >= amount)
        {
            currentMana -= amount;
            SetManaAnimation(currentMana);
        }
    }

    public void RecuperarMana(int quantidade)
    {
        currentMana = Mathf.Min(currentMana + quantidade, maxMana);
        SetManaAnimation(currentMana);
    }

    private void SetManaAnimation(int mana)
    {
        float normalizedTime = 1f - (float)mana / maxMana;
        Debug.Log("Normalized time: " + normalizedTime); // Verifique este valor
        manaAnimator.Play(manaAnimationStateName, 0, normalizedTime);
    }

    public void ResetMana()
    {
        currentMana = maxMana;
        SetManaAnimation(currentMana);
        Debug.Log("Mana resetada para o máximo");
    }

    void OnEnable()
    {
        SetManaAnimation(currentMana);
    }

}
