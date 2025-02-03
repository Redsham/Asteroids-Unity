using Gameplay.Player;
using UnityEngine;
using VContainer;

namespace UI.Gameplay
{
    public class LivesDrawer : MonoBehaviour
    {
        [SerializeField] private RectTransform[] m_Lives;
        
        [Inject] private PlayerBehaviour m_Player;


        [Inject]
        public void Construct()
        {
            m_Player.OnLivesChanged += HandleLivesChanged;
        }
        private void OnDestroy() => m_Player.OnLivesChanged -= HandleLivesChanged;
        
        private void HandleLivesChanged(int lives)
        {
            for (int i = 0; i < m_Lives.Length; i++)
                m_Lives[i].gameObject.SetActive(i < lives);
        }
    }
}