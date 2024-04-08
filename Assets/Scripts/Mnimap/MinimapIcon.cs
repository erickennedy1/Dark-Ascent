using UnityEngine;

public class MinimapIcon : MonoBehaviour
{
    SpriteRenderer icon;
    // Start is called before the first frame update
    void Start()
    {
        icon = GetComponent<SpriteRenderer>();
        icon.enabled = false;
    }

    public void Onknow()
    {
        icon.enabled = true;
        icon.color = new Color(0.8f,0.5f,0.5f, 1f);
    }

    public void OnClear()
    {
        icon.color = new Color(0.5f,0.8f,0.5f, 1f);
    }
}