using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "2048/Level Generator")]
public class Tiles2048Generator : LevelGenerator
{
    [SerializeField] TileValue endValue = TileValue._1024;
    [SerializeField, Range(2, 14)] int maxTilesCount = 5;
    [SerializeField, Tooltip("Insert Tiles in order from 2 to 2048")] TileData[] tiles;

    List<int> used;
    List<int> frontier;
    int[] values;

    public override LevelData Generate()
    {
        endValue = (TileValue)Mathf.Max((int)endValue, 1);
        int depth = Mathf.Min(maxTilesCount, size * size);

        used = new List<int>();
        frontier = new List<int>();
        values = new int[size * size];

        var data = CreateInstance<LevelData>();
        data.size = size;
        data.pieces = new TileData[size * size];

        int start = Random.Range(0, size * size);
        values[start] = (int)endValue;
        frontier.Add(start);
        used.Add(start);

        while (--depth > 0 && CheckFrontier())
        {
            int index = frontier[Random.Range(0, frontier.Count)];
            values[index]--;
            FindNeighbor(index, out int next);
            values[next] = values[index];
            frontier.Add(next);
            used.Add(next);
        }

        for (int i = 0; i < values.Length; i++)
        {
            if (!used.Contains(i)) { continue; }
            int v = values[i];
            TileData t = tiles[v];
            data.pieces[i] = t;
        }

        used.Clear();
        frontier.Clear();
        used = null;
        frontier = null;
        values = null;

        return data;
    }

    bool CheckFrontier()
    {
        for (int i = frontier.Count - 1; i >= 0; i--)
        {
            int index = frontier[i];
            if (values[index] <= 0)
            {
                frontier.RemoveAt(i);
                continue;
            }

            bool hasRight = (index + 1) % size != 0 && !used.Contains(index + 1);
            bool hasLeft = index % size != 0 && !used.Contains(index - 1);
            bool hasUp = index < size * (size - 1) && !used.Contains(index + size);
            bool hasDown = index >= size && !used.Contains(index - size);

            bool blocked = !(hasRight || hasLeft || hasUp || hasDown);
            if (blocked)
            {
                frontier.RemoveAt(i);
            }
        }
        return frontier.Count > 0;
    }

    bool FindNeighbor(int index, out int neighbor)
    {
        List<int> neighbors = new List<int>();

        bool hasRight = (index + 1) % size != 0 && !used.Contains(index + 1);
        bool hasLeft = index % size != 0 && !used.Contains(index - 1);
        bool hasUp = index < size * (size - 1) && !used.Contains(index + size);
        bool hasDown = index >= size && !used.Contains(index - size);

        bool blocked = !(hasRight || hasLeft || hasUp || hasDown);
        if (blocked)
        {
            frontier.Remove(index);
        }
        else
        {
            if (hasRight) { neighbors.Add(index + 1); }
            if (hasLeft) { neighbors.Add(index - 1); }
            if (hasUp) { neighbors.Add(index + size); }
            if (hasDown) { neighbors.Add(index - size); }

            neighbor = neighbors[Random.Range(0, neighbors.Count)];
            return true;
        }
        neighbor = -1;
        return false;
    }
}
