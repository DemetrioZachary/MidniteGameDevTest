using UnityEngine;

[CreateAssetMenu(menuName = "Sandwich/LevelData")]
public class LevelData : ScriptableObject
{
    public int size = 4;
    public ElementData[] pieces;
}
