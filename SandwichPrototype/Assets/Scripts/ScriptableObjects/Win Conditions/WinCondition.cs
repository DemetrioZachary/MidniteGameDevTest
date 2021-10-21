using System.Collections.Generic;
using UnityEngine;

public abstract class WinCondition : ScriptableObject
{
    public abstract bool Check(List<Piece> pieces);
}
