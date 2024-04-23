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
        icon.color = new Color(0.8f,0.5f,0.5f, 1f); //Vermelho
    }

    public void OnClear()
    {
        icon.color = new Color(0.5f,0.8f,0.5f, 1f); //Verde
    }

    public void OnEnterRoom()
    {
        icon.color = new Color(0.35f,0.65f,0.8f, 1f); //Azul
    }

    public void OnLeaveRoom()
    {
        icon.color = new Color(0.5f,0.8f,0.5f, 1f); //Verde
    }
}