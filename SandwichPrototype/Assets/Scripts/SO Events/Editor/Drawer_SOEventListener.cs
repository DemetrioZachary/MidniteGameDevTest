using UnityEngine;
using UnityEditor;
using System.Reflection;

namespace PrsdTech.SO.Events
{

    [CustomPropertyDrawer(typeof(SOEventListener), true)]
    public class Drawer_SOEventListener : PropertyDrawer
    {
        readonly BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position = EditorGUI.PrefixLabel(position, label);
            float w = position.width;
            position.width = 20f;

            string iconPath, iconText;
            bool hasEventReference = property.FindPropertyRelative("eventReference").objectReferenceValue != null;

            if (EditorApplication.isPlaying)
            {
                var onEventFieldInfo = typeof(SOEventListener).GetField("OnEvent", flags);
                var propertyFieldInfo = property.serializedObject.targetObject.GetType().GetField(property.name, flags);
                var onEventValue = onEventFieldInfo.GetValue(propertyFieldInfo.GetValue(property.serializedObject.targetObject));

                bool hasEventLinked = onEventValue != null;

                iconPath = hasEventReference ? hasEventLinked ? "Installed" : "Warning" : "Error";
                iconText = hasEventReference ? hasEventLinked ? "Event linked correctly." : "Missing event delegate." : "Missing event asset.";
            }
            else
            {
                iconPath = hasEventReference ? "d_console.infoicon.inactive.sml" : "Error";
                iconText = hasEventReference ? "Event linked, enter play-mode for delegate info." : "Missing event asset.";
            }
            GUI.Box(position, new GUIContent(EditorGUIUtility.Load(iconPath) as Texture, iconText), EditorStyles.label);

            position.x += 22f;
            position.width = w - 22f;
            EditorGUI.PropertyField(position, property.FindPropertyRelative("eventReference"), GUIContent.none);
        }
    }

}
