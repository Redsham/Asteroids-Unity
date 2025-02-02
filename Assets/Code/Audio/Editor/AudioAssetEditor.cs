using UnityEditor;
using UnityEngine;

namespace Audio.Editor
{
    [CustomEditor(typeof(UniAudioAsset), true)]
    public class AudioAssetEditor : UnityEditor.Editor
    {
        private UniAudioAsset m_Target;
        
        private void OnEnable()
        {
            m_Target = target as UniAudioAsset;
        }
        
        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Clip"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Volume"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Pitch"));

            if (m_Target is WorldAudioAsset worldAudioAsset)
            {
                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Spatial", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_SpatialBlend"));
                
                GUI.enabled = worldAudioAsset.SpatialBlend > 0.0f;
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Distance"));
                GUI.enabled = true;
            }
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}