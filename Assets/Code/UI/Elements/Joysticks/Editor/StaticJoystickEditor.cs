using UI.Elements.Joysticks;
using UnityEditor;

namespace UI.Joysticks.Editor
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