using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct Character
{
    public string name;
    public Sprite icon;
    public RectTransform iconRect;
}

[CreateAssetMenu(fileName = "Characters.asset", menuName = "Dialogue/Characters Data", order = 0)]

public class CharactersData : ScriptableObject
{
    public List<Character> characters;
}