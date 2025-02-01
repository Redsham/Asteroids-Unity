using UI.Elements.Joysticks;
using UnityEditor;
using Application = UnityEngine.Device.Application;

namespace UI.Joysticks.Editor
{
    public abstract class JoystickEditor<T> : UnityEditor.Editor where T : Joystick
    {
        protected T Joystick { get; private set; }
        
        private bool m_BaseFoldout;
        private bool m_ContextFoldout;
        
        private void OnEnable()
        {
            Joystick = target as T;
            Joystick.OnValueChanged.AddListener(value => Repaint());
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            DrawDebug();
            DrawBase();
            IternalDrawContext();
            
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawBase()
        {
            EditorGUILayout.BeginVertical("helpbox");
            EditorGUI.indentLevel++;
            
            m_BaseFoldout = EditorGUILayout.Foldout(m_BaseFoldout, "Base");
            if (!m_BaseFoldout)
            {
                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
                return;
            }

            EditorGUILayout.Space();
            
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_IsActive"));
            
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Events");
            
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_OnBeginDragEvent"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_OnEndDragEvent"));
            
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_OnValueChanged"));
            
            EditorGUI.indentLevel--;
            
            EditorGUILayout.EndVertical();
        }
        private void DrawDebug()
        {
            if(!Application.isPlaying) return;
            EditorGUILayout.BeginVertical("helpbox");
            EditorGUILayout.LabelField("Debug", EditorStyles.boldLabel);
            
            EditorGUILayout.Space();
            
            EditorGUILayout.LabelField("IsActive", Joystick.IsActive.ToString());
            EditorGUILayout.LabelField("IsDragging", Joystick.IsDragging.ToString());
            EditorGUILayout.LabelField("Value", Joystick.Value.ToString());
            EditorGUILayout.LabelField("Direction", Joystick.Direction.ToString());
            
            EditorGUILayout.EndVertical();
        }
        private void IternalDrawContext()
        {
            EditorGUILayout.BeginVertical("helpbox");
            EditorGUI.indentLevel++;
            
            m_ContextFoldout = EditorGUILayout.Foldout(m_ContextFoldout, "Other");
            if (!m_ContextFoldout)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.EndVertical();
                return;
            }

            EditorGUILayout.Space();
            
            DrawContext();
            
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
        }
        
        protected abstract void DrawContext();
    }
}