using UnityEngine;

[System.Serializable]
public class Dialogue
{
    [Tooltip("Nome do NPC")]
    public string name;

    [Tooltip("Cada sentença aparecerá sepadaradamente")]
    [TextArea(3, 10)]
    public string sentence;
}
