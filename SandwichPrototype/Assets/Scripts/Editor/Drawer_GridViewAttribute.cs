using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridData))]
public class Drawer_GridViewAttribute : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.Space();

        serializedObject.Update();

        int size = serializedObject.FindProperty("size").intValue;
        var pieces = serializedObject.FindProperty("pieces");

        if (pieces.arraySize != size * size)
        {
            pieces.arraySize = size * size;
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
                var p = (target as GridData).pieces[x + size * y];
                string l = (x + size * y).ToString() + " " + (p ? p.name : "");
                EditorGUI.LabelField(rect, l, EditorStyles.helpBox);
                rect.x += step;
            }
            rect.x -= size * step;
            rect.y -= step;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
