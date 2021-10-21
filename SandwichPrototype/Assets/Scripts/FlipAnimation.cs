using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Sandwich/Animations/Flip")]
public class FlipAnimation : SOAnimation
{
    static Transform pivot;

    public System.Action Callback;

    public override IEnumerator Play(GridCell startCell, GridCell endCell)
    {
        if (!pivot) { pivot = new GameObject("Pivot").transform; }

        Vector3 startPosition = (endCell.Position + startCell.Position) * 0.5f;
        startPosition.y = startCell.Depth * GlobalData.pieceHeight;
        Vector3 endPosition = startPosition;
        endPosition.y = endCell.Depth * GlobalData.pieceHeight;

        Vector3 endRotation = Vector3.Cross(startCell.Position - endCell.Position, Vector3.up).normalized * 180f;

        pivot.position = startPosition;
        pivot.eulerAngles = Vector3.zero;
        var os = startCell.GetObjects();
        for (int i = 0; i < os.Length; i++)
        {
            os[i].SetParent(pivot, true);
        }

        float time = 0f;
        while (time < duration)
        {
            float t = Mathf.SmoothStep(0f, 1f, time / duration);
            pivot.position = Vector3.Lerp(startPosition, endPosition, t);
            pivot.eulerAngles = Vector3.Lerp(Vector3.zero, endRotation, t);
            yield return null;
            time += Time.deltaTime;
        }
        pivot.position = endPosition;
        pivot.eulerAngles = endRotation;

        for (int i = 0; i < os.Length; i++)
        {
            os[i].SetParent(null, true);
        }

        Callback?.Invoke();
    }
}
