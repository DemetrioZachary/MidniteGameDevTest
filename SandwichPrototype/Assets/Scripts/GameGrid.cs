using UnityEngine;
using PrsdTech.SO.Events;

public partial class GameGrid : MonoBehaviour
{
    [SerializeField] GridData level;
    [Header("Config")]
    [SerializeField] LevelGenerator generator;
    [SerializeField] PiecePooler piecePooler;
    [SerializeField] WinCondition winCondition;
    [SerializeField] SOAnimation soAnimation;
    [Header("Events")]
    [SerializeField] SOEventListener inputBegunListener;
    [SerializeField] SOEventListener inputDragListener;
    [SerializeField] SOEvent winEvent;
    [SerializeField] SOEvent loseEvent;
    [SerializeField] SOEvent moveEvent;

    int size;
    bool dragValid;
    Vector2Int startCoords;

    GridCell[] cells;

    void OnEnable()
    {
        InitializeGrid();

        inputBegunListener.Enable(OnInputBegan);
        inputDragListener.Enable(OnInputDrag);
    }

    void OnDisable()
    {
        inputBegunListener.Disable();
        inputDragListener.Disable();
    }

    public void InitializeGrid()
    {
        piecePooler.RecycleAll();

        level = generator.Generate();

        size = level.size;
        transform.position = new Vector3(size * 0.5f, 0f, size * 0.5f);
        transform.localScale = Vector3.one * 0.1f * size;

        cells = new GridCell[size * size];
        for (int i = 0; i < cells.Length; i++)
        {
            Vector3 position = new Vector3(i % size + 0.5f, 0f, i / size + 0.5f);
            var cell = new GridCell(position);
            var pieceData = level.pieces[i];
            if (pieceData)
            {
                Transform t = piecePooler.Get(pieceData, position, Quaternion.identity).transform;
                cell.AddToCell(new Piece { data = pieceData, transform = t });
            }
            cells[i] = cell;
        }
    }

    GridCell GetCell(Vector2Int coords)
    {
        return cells[coords.x + level.size * coords.y];
    }

    bool GetCoordsFromInput(Vector3 position, out Vector2Int coords)
    {
        coords = Vector2Int.zero;
        var ray = Camera.main.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 100f, GameUtility.gridMask))
        {
            Vector3 p = hitInfo.point;
            coords = new Vector2Int((int)p.x, (int)p.z);
            return true;
        }
        return false;
    }

    void OnInputBegan(SOEventArgs args)
    {
        if (!(args is InputEventArgs)) { return; }

        if (GetCoordsFromInput((args as InputEventArgs).position, out Vector2Int coords))
        {
            if (!GetCell(coords).IsEmpty)
            {
                startCoords = coords;
                dragValid = true;
            }
        }
    }

    void OnInputDrag(SOEventArgs args)
    {
        if (!dragValid || !(args is InputEventArgs)) { return; }

        if (GetCoordsFromInput((args as InputEventArgs).position, out Vector2Int coords))
        {
            if (coords != startCoords)
            {
                GridCell endCell = GetCell(coords);
                if (!endCell.IsEmpty && !AnimationQueueFull)
                {
                    GridCell startCell = GetCell(startCoords);
                    AddAnimationToQueue(startCell, endCell);
                }
                dragValid = false;
            }
        }
        else
        {
            dragValid = false;
        }
    }

    void CheckCells()
    {
        int count = 0;
        int index = -1;
        bool allCellsIsolated = true;

        for (int i = 0; i < cells.Length; i++)
        {
            if (!cells[i].IsEmpty)
            {
                count++;
                index = i;

                bool missingRightCell = (i + 1) % size == 0 || cells[i + 1].IsEmpty;
                bool missingUpCell = i >= size * (size - 1) || cells[i + size].IsEmpty;
                bool isolated = missingRightCell && missingUpCell;
                allCellsIsolated &= isolated;
            }
        }

        if (count == 1 || allCellsIsolated)
        {
            if (winCondition.Check(cells[index].Pieces))
            {
                Debug.Log("Win");
                winEvent.Invoke();
            }
            else
            {
                Debug.Log("Lose");
                loseEvent.Invoke();
            }
        }
    }
}
