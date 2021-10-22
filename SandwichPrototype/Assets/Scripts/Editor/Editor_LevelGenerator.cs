using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelGenerator), true)]
public class Editor_LevelGenerator : Editor
{
    LevelData data;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.Space();
        if (GUILayout.Button("Generate New Asset"))
        {
            data = (target as LevelGenerator).Generate();
        }

        if (data)
        {
            Editor_LevelData.DrawGridInInspector(data);
            EditorGUILayout.Space();
            if (GUILayout.Button("Create Asset"))
            {
                string path = AssetDatabase.GenerateUniqueAssetPath(GameUtility.levelAssetFolderPath + "Level 0.asset");
                AssetDatabase.CreateAsset(data, path);
                data = null;
            }
        }
    }
}
