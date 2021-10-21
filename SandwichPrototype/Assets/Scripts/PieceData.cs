using UnityEngine;

public enum PieceType { Bread, Ingredient }

[CreateAssetMenu(menuName = "Sandwich/Piece")]
public class PieceData : ScriptableObject
{
    public PieceType type;
}
