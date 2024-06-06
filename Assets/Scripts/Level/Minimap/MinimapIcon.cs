using UnityEngine;

public class MinimapIcon : MonoBehaviour
{
    [Header("SpriteRenderes")]
    [SerializeField] private SpriteRenderer background;
    [SerializeField] private SpriteRenderer border;
    [SerializeField] private SpriteRenderer icon;

    [Header("IconList")]
    [SerializeField] private MinimapIconListData iconList;
    private Material m_Background;
    private Sprite iconDefault;
    // Start is called before the first frame update
    void Start()
    {
        SetSprites(false);
        m_Background = background.material;
    }

    private void SetSprites(bool state)
    {
        background.enabled = state;
        border.enabled = state;
    }

    public void SetDefaultIcon(string roomType)
    {
        switch (roomType)
        {
            case "Reward":
                iconDefault = iconList.icons[2];
                break;
            case "Gate":
                iconDefault = iconList.icons[3];
                break;
            default:
                iconDefault = iconList.icons[0];
                break;
        }
    }

    public void Onknow()
    {
        SetSprites(true);
        background.color = new Color(0.1f,0.1f,0.1f, 1f); //Preto
        m_Background.SetColor("_Color", background.color);
    }

    public void OnClear()
    {
        //Background
        background.color = new Color(0.5f,0.5f,0.5f, 1f); //Cinza Claro
        m_Background.SetColor("_Color", background.color);

        //Icon
        icon.sprite = iconDefault;
    }

    public void OnEnterRoom()
    {
        //Background
        background.color = new Color(0.63f,0.67f,0.44f, 1f); //Verde
        m_Background.SetColor("_Color", background.color);

        //Icon
        icon.sprite = iconList.icons[1];
    }

    public void OnLeaveRoom()
    {
        //Background
        background.color = new Color(0.5f,0.5f,0.5f, 1f); //Cinza Claro
        m_Background.SetColor("_Color", background.color);

        //Icon
        icon.sprite = iconDefault;
    }
}