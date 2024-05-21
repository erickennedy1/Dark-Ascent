using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueData.asset", menuName = "Dialogue/Dialogue Data", order = 0)]
public class DialogueData : ScriptableObject
{
    // [Tooltip("Cada sentença aparecerá sepadaradamente, e pode variar de personagem")]
    public List<Dialogue> sentencesList;

    [Tooltip("Indica se o diálogo pode ser repetido após terminar")]
    public bool loop = false;
}
