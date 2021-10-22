using System.Collections;
using UnityEngine;

public abstract class SOAnimation : ScriptableObject
{
    [SerializeField] float duration = 1f;

    public float DurationScale { get; set; } = 1f;

    protected float ScaledDuration => duration * DurationScale;

    public abstract IEnumerator Play(GridCell startCell, GridCell endCell);
}
