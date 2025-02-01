using UnityEngine;

namespace UI.Elements.Joysticks
{
    public class StaticJoystick : Joystick
    {
        [SerializeField] private RectTransform m_Area;
        [SerializeField] private RectTransform m_Handle;

        private float m_Radius;


        public override void Initialize()
        {
            m_Handle.gameObject.SetActive(false);
            m_Radius = m_Area.sizeDelta.x * 0.5f;
        }

        public override void OnBeginDrag() => m_Handle.gameObject.SetActive(true);
        public override void HandleInput()
        {
            Value                  = Vector2.ClampMagnitude((Point - (Vector2)m_Area.position) / (m_Radius * m_Area.lossyScale.x), 1.0f);
            m_Handle.localPosition = Value * m_Radius;
        }
        public override void OnEndDrag() => m_Handle.gameObject.SetActive(false);
    }
}