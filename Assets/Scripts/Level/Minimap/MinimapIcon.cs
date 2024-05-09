using UnityEngine;

public class MinimapIcon : MonoBehaviour
{
    SpriteRenderer icon;
    Material material;
    // Start is called before the first frame update
    void Start()
    {
        icon = GetComponent<SpriteRenderer>();
        icon.enabled = false;
        material = icon.material;
    }

    public void Onknow()
    {
        icon.enabled = true;
        icon.color = new Color(0.8f,0.5f,0.5f, 1f); //Vermelho
        material.SetColor("_Color", icon.color);
    }

    public void OnClear()
    {
        icon.color = new Color(0.5f,0.8f,0.5f, 1f); //Verde
        material.SetColor("_Color", icon.color);
    }

    public void OnEnterRoom()
    {
        icon.color = new Color(0.35f,0.65f,0.8f, 1f); //Azul
        material.SetColor("_Color", icon.color);
    }

    public void OnLeaveRoom()
    {
        icon.color = new Color(0.5f,0.8f,0.5f, 1f); //Verde
        material.SetColor("_Color", icon.color);
    }
}