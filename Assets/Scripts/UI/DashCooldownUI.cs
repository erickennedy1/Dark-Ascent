using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DashCooldownUI : MonoBehaviour
{
    public Image dashCooldownImage;
    public TMP_Text dashCooldownText;

    private float dashCooldown = 2f;
    private float cooldownTimer = 0f;
    private bool isCooldown = false;

    void Start()
    {
        dashCooldownImage.fillAmount = 0f;
        dashCooldownText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            dashCooldownImage.fillAmount = cooldownTimer / dashCooldown;
            dashCooldownText.text = Mathf.Ceil(cooldownTimer).ToString();

            if (cooldownTimer <= 0f)
            {
                isCooldown = false;
                dashCooldownImage.fillAmount = 0f;
                dashCooldownText.gameObject.SetActive(false);
            }
        }
    }

    public void StartCooldown()
    {
        isCooldown = true;
        cooldownTimer = dashCooldown;
        dashCooldownText.gameObject.SetActive(true);
    }
}
