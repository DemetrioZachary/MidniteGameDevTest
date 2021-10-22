using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelGenerator))]
public class Editor_LevelGenerator : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
    }
}
