using UnityEngine;
using PrsdTech.SO.Events;

public partial class GameGrid : MonoBehaviour
{
    //[SerializeField] LevelData level;
    [Header("Config")]
    //[SerializeField] LevelGenerator generator;
    [SerializeField] LevelPack levelPack;
    [SerializeField] PiecePooler piecePooler;
    [SerializeField] WinCondition winCondition;
    [SerializeField] SOAnimation soAnimation;
    [Header("Events")]
    [SerializeField] SOEventListener inputBegunListener;
    [SerializeField] SOEventListener inputDragListener;
    [SerializeField] SOEvent winEvent;
    [SerializeField] SOEvent loseEvent;
    [SerializeField] SOEvent packCompletedEvent;
    [SerializeField] SOEvent moveEvent;

    LevelData level;
    int size;
    bool dragValid;
    GridCell startCell;

    GridCell[] cells;

    void OnEnable()
    {
        LoadNextLevel();

        inputBegunListener.Enable(OnInputBegan);
        inputDragListener.Enable(OnInputDrag);
    }

    void OnDisable()
    {
        inputBegunListener.Disable();
        inputDragListener.Disable();
    }

    public void LoadNextLevel()
    {
        if (!levelPack.GetNextLevel(out level))
        {
            packCompletedEvent.Invoke();
            return;
        }
        InitializeGrid();
    }

    public void InitializeGrid()
    {
        piecePooler.RecycleAll();
        //level = generator.Generate();

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

    bool GetCellFromInput(Vector3 position, out GridCell cell)
    {
        cell = null;
        var ray = Camera.main.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 100f, GameUtility.gridMask))
        {
            Vector3 p = hitInfo.point;
            cell = cells[(int)p.x + size * (int)p.z];
            return true;
        }
        return false;
    }

    void OnInputBegan(SOEventArgs args)
    {
        if (!(args is InputEventArgs)) { return; }

        if (GetCellFromInput((args as InputEventArgs).position, out GridCell cell))
        {
            if (!cell.IsEmpty)
            {
                startCell = cell;
                dragValid = true;
            }
        }
    }

    void OnInputDrag(SOEventArgs args)
    {
        if (!dragValid || !(args is InputEventArgs)) { return; }

        if (GetCellFromInput((args as InputEventArgs).position, out GridCell cell))
        {
            if (cell != startCell)
            {
                if (!cell.IsEmpty)
                {
                    AddAnimationToQueue(startCell, cell);
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

        if (allCellsIsolated) // WinCondition can be extended to a wider control level
        {
            if (count == 1)
            {
                if (winCondition.Check(cells[index].Pieces))
                {
                    winEvent.Invoke();
                }
                else
                {
                    loseEvent.Invoke();
                }
            }
            else
            {
                loseEvent.Invoke();
            }
        }
    }
}
