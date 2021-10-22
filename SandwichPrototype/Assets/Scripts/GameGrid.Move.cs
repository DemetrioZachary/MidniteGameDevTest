using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameGrid : MonoBehaviour
{
    class Move
    {
        public GridCell startCell;
        public GridCell endCell;
    }
    Queue<Move> moves;
    bool inAnimation;

    bool AnimationQueueFull => moves != null && moves.Count == GameUtility.movesQueueLenght;

    void AddAnimationToQueue(GridCell startCell, GridCell endCell)
    {
        if (moves == null) { moves = new Queue<Move>(GameUtility.movesQueueLenght); }

        moves.Enqueue(new Move() {
            startCell = startCell,
            endCell = endCell
        });

        if (inAnimation)
        {
            soAnimation.DurationScale *= GameUtility.animationSpeedUpFactor;
        }
        else
        {
            soAnimation.DurationScale = 1f;
            StartCoroutine(AnimationQueue());
        }
    }

    IEnumerator AnimationQueue()
    {
        inAnimation = true;
        while (moves.Count > 0)
        {
            Move move = moves.Dequeue();
            yield return soAnimation.Play(move.startCell, move.endCell);

            move.startCell.MoveToCell(move.endCell);
            moveEvent.Invoke();
        }
        CheckCells();
        inAnimation = false;
    }
}
