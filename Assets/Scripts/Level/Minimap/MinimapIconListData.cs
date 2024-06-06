using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MinimapIconListData.asset", menuName = "Minimap/IconList Data", order = 0)]

public class MinimapIconListData : ScriptableObject
{
    public List<Sprite> icons;
}
