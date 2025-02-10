using UnityEditor;

namespace UI.Elements.Joysticks.Editor
{
    [CustomEditor(typeof(StaticJoystick))]
    public class StaticJoystickEditor : JoystickEditor<StaticJoystick>
    {
        protected override void DrawContext()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Area"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Handle"));
        }
    }
}