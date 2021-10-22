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

    bool AnimationQueueFull => moves != null && moves.Count >= GameUtility.movesQueueLenght;

    void AddAnimationToQueue(GridCell startCell, GridCell endCell)
    {
        if (moves == null) { moves = new Queue<Move>(GameUtility.movesQueueLenght); }
        if (AnimationQueueFull) { return; }

        moves.Enqueue(new Move() {
            startCell = startCell,
            endCell = endCell
        });

        if (inAnimation)
        {
            gameData.SOAnimation.DurationScale *= GameUtility.animationSpeedUpFactor;
        }
        else
        {
            gameData.SOAnimation.DurationScale = 1f;
            StartCoroutine(AnimationQueue());
        }
    }

    IEnumerator AnimationQueue()
    {
        inAnimation = true;
        while (moves.Count > 0)
        {
            Move move = moves.Dequeue();
            if (!move.startCell.IsEmpty && !move.endCell.IsEmpty)
            {
                yield return gameData.SOAnimation.Play(move.startCell, move.endCell);

                move.startCell.MoveToCell(move.endCell);
                moveEvent.Invoke(new MoveEventArgs() { endCell = move.endCell });
            }
        }
        CheckCells();
        inAnimation = false;
    }
}
