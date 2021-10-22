using UnityEngine;

public enum PieceType { Bread, Ingredient }

[CreateAssetMenu(menuName = "Sandwich/Piece")]
public class PieceData : ElementData
{
    [SerializeField] PieceType type;

    public PieceType Type => type;
}
