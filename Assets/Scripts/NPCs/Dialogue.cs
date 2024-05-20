using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    [Tooltip("Nome do NPC")]
    public string name;

    [Tooltip("Indica se o diálogo pode ser repetido após terminar")]
    public bool loop = false;

    [Tooltip("Cada sentença aparecerá sepadaradamente")]
    [TextArea(3, 10)]
    public string[] sentences;
}
