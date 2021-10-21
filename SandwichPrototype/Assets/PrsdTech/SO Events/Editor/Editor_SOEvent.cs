using UnityEngine;
using UnityEditor;

namespace PrsdTech.SO.Events
{

    [CustomEditor(typeof(SOEvent), true)]
    public class Editor_SOEvent : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var listeners = (target as SOEvent).GetListeners();
            int size = listeners == null ? 0 : listeners.Count;
            EditorGUILayout.LabelField("Active Listeners:", size.ToString());
            if (listeners != null)
            {
                EditorGUI.indentLevel++;
                for (int i = 0; i < listeners.Count; i++)
                {
                    Object o = listeners[i].Object;
                    Rect position = EditorGUILayout.GetControlRect();
                    position.width = EditorGUIUtility.labelWidth;
                    EditorGUI.LabelField(position, o.name);
                    position.x += position.width + 1f;
                    position.width = 20f;
                    if (GUI.Button(position, EditorGUIUtility.Load("d_tab_next@2x") as Texture))
                    {
                        Selection.activeObject = o;
                    }
                }
                EditorGUI.indentLevel--;
            }
            if (GUILayout.Button("Play Event"))
            {
                (target as SOEvent).Invoke();
            }
        }
    }

}
