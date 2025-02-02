using Audio.Values;
using UnityEditor;
using UnityEngine;

namespace Audio.Editor
{
    [CustomPropertyDrawer(typeof(RangeValue))]
    public class RangeValueDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, label);
            
            SerializedProperty minProp       = property.FindPropertyRelative("m_Min");
            SerializedProperty maxProp       = property.FindPropertyRelative("m_Max");
            
            float fieldWidth = position.width / 2.0f;
            float spaceWidth = 5.0f;

            Rect firstFieldRect  = new(position.x, position.y, fieldWidth - spaceWidth, position.height);
            Rect secondFieldRect = new(firstFieldRect.xMax + spaceWidth, position.y, fieldWidth - spaceWidth, position.height);
            
            EditorGUI.PropertyField(firstFieldRect, minProp, GUIContent.none);
            EditorGUI.PropertyField(secondFieldRect, maxProp, GUIContent.none);

            EditorGUI.EndProperty();
        }
    }
}