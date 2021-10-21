using System.Collections;
using UnityEngine;

public abstract class SOAnimation : ScriptableObject
{
    public float duration = 1f;

    public abstract IEnumerator Play(GridCell startCell, GridCell endCell);
}
