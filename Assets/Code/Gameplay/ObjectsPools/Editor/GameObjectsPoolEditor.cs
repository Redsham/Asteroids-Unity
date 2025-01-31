using UnityEditor;
using UnityEngine;

namespace Gameplay.ObjectsPools.Editor
{
    [CustomPropertyDrawer(typeof(GameObjectsPool<>))]
    public class GameObjectsPoolEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            
            SerializedProperty m_Prefab = property.FindPropertyRelative("m_Prefab");
            SerializedProperty m_InitialSize = property.FindPropertyRelative("m_InitialSize");
            
            position.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(position, m_Prefab);
            position.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(position, m_InitialSize);
            
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 2;
        }
    }
}