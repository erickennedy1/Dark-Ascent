using UnityEngine;

public class ToggleSliderVisibility : MonoBehaviour
{
    public void ToggleVisibility()
    {

        EnemyMovementAndHealth.useHealthSlider = !EnemyMovementAndHealth.useHealthSlider;

        EnemyMovementAndHealth[] allEnemies = FindObjectsOfType<EnemyMovementAndHealth>();
        foreach (var enemy in allEnemies)
        {
            if (enemy.healthSlider != null)
            {
                enemy.healthSlider.gameObject.SetActive(EnemyMovementAndHealth.useHealthSlider);
            }
        }
    }
}
