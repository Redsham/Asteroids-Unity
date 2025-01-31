using UnityEditor;
using UnityEngine;

namespace Utils.ObjectsPools.Editor
{
    [CustomPropertyDrawer(typeof(GameObjectsPool<>))]
    public class GameObjectsPoolEditor : PropertyDrawer
    {
        private bool m_IsExpanded;
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            
            Rect foldoutRect = new(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            m_IsExpanded = EditorGUI.Foldout(foldoutRect, m_IsExpanded, label);
            
            if (m_IsExpanded)
            {
                EditorGUI.indentLevel++;
                
                Rect prefabRect = new(position.x, position.y + EditorGUIUtility.singleLineHeight, position.width, EditorGUIUtility.singleLineHeight);
                EditorGUI.PropertyField(prefabRect, property.FindPropertyRelative("m_Prefab"));
                
                Rect sizeRect = new(position.x, position.y + EditorGUIUtility.singleLineHeight * 2, position.width, EditorGUIUtility.singleLineHeight);
                EditorGUI.PropertyField(sizeRect, property.FindPropertyRelative("m_InitialSize"));
                
                Rect parentRect = new(position.x, position.y + EditorGUIUtility.singleLineHeight * 3, position.width, EditorGUIUtility.singleLineHeight);
                EditorGUI.PropertyField(parentRect, property.FindPropertyRelative("m_Parent"));
                
                EditorGUI.indentLevel--;
            }
            
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * (m_IsExpanded ? 4 : 1);
        }
    }
}