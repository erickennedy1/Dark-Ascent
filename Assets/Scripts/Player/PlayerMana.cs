using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMana : MonoBehaviour
{
    [Header("Configurações de Mana")]
    public int maxMana = 100;
    public int currentMana;
    private Animator manaAnimator;
    private string manaAnimationStateName = "Mana_animacao";
    private GameObject manaPowerUP;

    void Start()
    {
        manaAnimator = GameObject.Find("UI_vida_e_mana").GetComponent<Animator>();
        currentMana = maxMana;
        SetManaAnimation(currentMana);

        manaPowerUP = GameObject.Find("ManaPowerUP");
        if (manaPowerUP != null)
        {
            manaPowerUP.SetActive(false); 
        }

        //DontDestroyOnLoad(manaAnimator);
    }

    public void UsarMana(int amount)
    {
        if (currentMana >= amount)
        {
            currentMana -= amount;
            SetManaAnimation(currentMana);
            UpdateManaPowerUPState();
        }
    }

    public void RecuperarMana(int quantidade)
    {
        currentMana = Mathf.Min(currentMana + quantidade, maxMana);
        SetManaAnimation(currentMana);
        UpdateManaPowerUPState();
    }

    private void SetManaAnimation(int mana)
    {
        float normalizedTime = 1f - (float)mana / maxMana;
        manaAnimator.Play(manaAnimationStateName, 0, normalizedTime);
    }

    public void ResetMana()
    {
        maxMana = 100;
        currentMana = maxMana;
        SetManaAnimation(currentMana);
        UpdateManaPowerUPState();
    }

    void OnEnable()
    {
        SetManaAnimation(currentMana);
        UpdateManaPowerUPState();
    }

    public void UpdateManaPowerUPState()
    {
        if (manaPowerUP != null)
        {
            if (maxMana > 110)
            {
                manaPowerUP.SetActive(true);
            }
            else
            {
                manaPowerUP.SetActive(false);
            }
        }
    }
}
