using System;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelData))]
public class Editor_LevelData : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.Space();
        DrawGridInInspector(target as LevelData);
    }

    public static void DrawGridInInspector(LevelData data)
    {
        int size = data.size;

        if (data.pieces.Length != size * size)
        {
            Array.Resize(ref data.pieces, size * size);
        }

        Rect rect = EditorGUILayout.GetControlRect(false, Screen.width * 0.7f);
        float step = rect.height / size;
        rect.y += rect.height - step;
        rect.width = step - 2f;
        rect.height = step - 2f;

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                var p = data.pieces[x + size * y];
                string l = (x + size * y).ToString() + " " + (p ? p.name : "");
                EditorGUI.LabelField(rect, l, EditorStyles.helpBox);
                rect.x += step;
            }
            rect.x -= size * step;
            rect.y -= step;
        }
    }
}
