using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Sandwich/Animations/Flip")]
public class FlipAnimation : SOAnimation
{
    static Transform pivot;

    public override IEnumerator Play(GridCell startCell, GridCell endCell)
    {
        if (!pivot) { pivot = new GameObject("Pivot").transform; }

        Vector3 startPosition = (endCell.Position + startCell.Position) * 0.5f;
        startPosition.y = startCell.Depth * GameUtility.pieceHeight;
        Vector3 endPosition = startPosition;
        endPosition.y = endCell.Depth * GameUtility.pieceHeight;

        Vector3 endRotation = Vector3.Cross(startCell.Position - endCell.Position, Vector3.up).normalized * 180f;

        pivot.position = startPosition;
        pivot.eulerAngles = Vector3.zero;
        var pieces = startCell.Pieces.ToArray();
        for (int i = 0; i < pieces.Length; i++)
        {
            pieces[i].transform.SetParent(pivot, true);
        }

        float time = 0f;
        while (time < ScaledDuration)
        {
            float t = Mathf.SmoothStep(0f, 1f, time / ScaledDuration);
            pivot.position = Vector3.Lerp(startPosition, endPosition, t);
            pivot.eulerAngles = Vector3.Lerp(Vector3.zero, endRotation, t);
            yield return null;
            time += Time.deltaTime;
        }
        pivot.position = endPosition;
        pivot.eulerAngles = endRotation;

        for (int i = 0; i < pieces.Length; i++)
        {
            pieces[i].transform.SetParent(null, true);
        }
    }
}
