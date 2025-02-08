using System;
using Gameplay.Player;
using LitMotion;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace UI.Gameplay
{
    public class LivesDrawer : MonoBehaviour
    {
        [SerializeField] private Image[] m_Lives;
        
        [Inject] private PlayerBehaviour m_Player;
        private Live[] m_LivesData = Array.Empty<Live>();


        [Inject]
        public void Construct()
        {
            m_Player.OnLivesChanged += HandleLivesChanged;
            
            m_LivesData = new Live[m_Lives.Length];
            for (int i = 0; i < m_Lives.Length; i++)
                m_LivesData[i] = new Live(m_Lives[i]);
        }
        private void OnDestroy() => m_Player.OnLivesChanged -= HandleLivesChanged;
        
        private void HandleLivesChanged(int lives)
        {
            for (int i = 0; i < m_Lives.Length; i++)
                m_LivesData[i].SetActive(i < lives);
        }

        
        private class Live
        {
            public Live(Image image)
            {
                m_Image         = image;
                m_IsActive      = true;
                
                m_InitialPosition = RectTransform.anchoredPosition;
            }


            private readonly Image         m_Image;
            private          RectTransform RectTransform => m_Image.rectTransform;
            
            private          bool          m_IsActive;
            
            private          MotionHandle m_MotionHandle;
            private readonly Vector2      m_InitialPosition;
            
            
            public void SetActive(bool isActive)
            {
                if (m_IsActive == isActive)
                    return;
                
                m_IsActive = isActive;
                m_MotionHandle.TryComplete();
                
                
                Vector2 startPosition = (isActive ? -Vector2.up : Vector2.zero) * 50.0f + m_InitialPosition;
                Vector2 endPosition   = (isActive ? Vector2.zero : Vector2.up) * 50.0f + m_InitialPosition;
                
                Color startColor = m_Image.color;
                Color endColor   = startColor;
                endColor.a = isActive ? 1.0f : 0.0f;
                
                
                if (isActive)
                    m_Image.gameObject.SetActive(true);

                m_MotionHandle = LMotion.Create(0.0f, 1.0f, 0.25f)
                                        .WithOnComplete(() =>
                                        {
                                            if (!isActive)
                                                m_Image.gameObject.SetActive(false);
                                        })
                                        .Bind(time =>
                                        {
                                            RectTransform.anchoredPosition = Vector2.Lerp(startPosition, endPosition, EaseUtility.OutQuad(time));
                                            m_Image.color = Color.Lerp(startColor, endColor, EaseUtility.OutExpo(time));
                                        });
            }
        }
    }
}