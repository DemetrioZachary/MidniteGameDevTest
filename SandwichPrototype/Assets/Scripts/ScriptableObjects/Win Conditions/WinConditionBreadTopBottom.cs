using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Sandwich/Win Conditions/Bread Top-Bottom")]
public class WinConditionBreadTopBottom : WinCondition
{
    public override bool Check(List<Piece> pieces)
    {
        return pieces[0].data.type == PieceType.Bread && pieces[pieces.Count - 1].data.type == PieceType.Bread;
    }
}
