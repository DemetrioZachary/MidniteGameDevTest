using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Sandwich/Level Generator")]
public class SandwichLevelGenerator : LevelGenerator
{
    [SerializeField, Range(1, 10)] int ingredientsCount = 3;
    [SerializeField] PieceData bread;
    [SerializeField] PieceData[] ingredients;

    List<int> used;
    List<int> frontier;

    public override LevelData Generate()
    {
        int depth = Mathf.Min(ingredientsCount, size * size - 2);
        int start = Random.Range(0, size * size);

        var data = CreateInstance<LevelData>();
        data.size = size;
        data.pieces = new PieceData[size * size];

        used = new List<int>();
        frontier = new List<int>();

        data.pieces[start] = bread;
        used.Add(start);
        frontier.Add(start);

        bool otherBread = true;
        while (depth > 0)
        {
            FindNewFrontier();
            int index = frontier[Random.Range(0, frontier.Count)];
            used.Add(index);

            if (otherBread)
            {
                data.pieces[index] = bread;
                otherBread = false;
                continue;
            }

            PieceData ingredient = ingredients[Random.Range(0, ingredients.Length)];
            data.pieces[index] = ingredient;

            depth--;
        }

        used.Clear();
        frontier.Clear();
        used = null;
        frontier = null;

        return data;
    }

    void FindNewFrontier()
    {
        List<int> newFrontier = new List<int>();
        for (int i = 0; i < frontier.Count; i++)
        {
            int index = frontier[i];
            if (!used.Contains(index))  // Branching only from used cells
            {
                newFrontier.Add(index);
                continue;
            }

            bool hasRight = (index + 1) % size != 0 && !used.Contains(index + 1) && !newFrontier.Contains(index + 1);
            bool hasLeft = index % size != 0 && !used.Contains(index - 1) && !newFrontier.Contains(index - 1);
            bool hasUp = index < size * (size - 1) && !used.Contains(index + size) && !newFrontier.Contains(index + size);
            bool hasDown = index >= size && !used.Contains(index - size) && !newFrontier.Contains(index - size);

            if (hasRight) { newFrontier.Add(index + 1); }
            if (hasLeft) { newFrontier.Add(index - 1); }
            if (hasUp) { newFrontier.Add(index + size); }
            if (hasDown) { newFrontier.Add(index - size); }
        }
        frontier.Clear();
        frontier.AddRange(newFrontier);
    }

}
