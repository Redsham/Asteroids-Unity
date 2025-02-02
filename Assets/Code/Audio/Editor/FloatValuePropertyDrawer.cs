using Audio.Values;
using UnityEditor;
using UnityEngine;

namespace Audio.Editor
{
    [CustomPropertyDrawer(typeof(FloatValue))]
    public class FloatValuePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            SerializedProperty useRandomProp = property.FindPropertyRelative("m_UseRandom");
            SerializedProperty minProp       = property.FindPropertyRelative("m_Min");
            SerializedProperty maxProp       = property.FindPropertyRelative("m_Max");

            const float TOGGLE_WIDTH = 20.0f;  
            float       fieldWidth  = (position.width - TOGGLE_WIDTH - EditorGUIUtility.labelWidth) / 2.0f;

            position = EditorGUI.PrefixLabel(position, label);

            Rect toggleRect = new(position.x, position.y, TOGGLE_WIDTH, position.height);
            useRandomProp.boolValue = EditorGUI.Toggle(toggleRect, useRandomProp.boolValue);
            
            if (useRandomProp.boolValue)
            {
                Rect firstFieldRect  = new(toggleRect.xMax + 5.0f, position.y, fieldWidth - 5.0f, position.height);
                Rect secondFieldRect = new(firstFieldRect.xMax + 5.0f, position.y, fieldWidth - 5.0f, position.height);

                EditorGUI.PropertyField(firstFieldRect, minProp, GUIContent.none);
                EditorGUI.PropertyField(secondFieldRect, maxProp, GUIContent.none);
            }
            else
            {
                Rect constFieldRect  = new(toggleRect.xMax + 5.0f, position.y, position.width - toggleRect.width - 5.0f, position.height);
                EditorGUI.PropertyField(constFieldRect, minProp, GUIContent.none);
            }

            EditorGUI.EndProperty();
        }
    }
}