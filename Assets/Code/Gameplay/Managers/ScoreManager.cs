using System;
using Gameplay.Asteroids;
using VContainer;
using VContainer.Unity;

namespace Gameplay.Managers
{
    public class ScoreManager : IInitializable, IDisposable
    {
        public uint Score
        {
            get => m_Score;
            private set
            {
                m_Score = value;
                OnScoreChanged?.Invoke(m_Score);
            }
        }
        private uint m_Score;
        
        [Inject] private readonly AsteroidsManager m_AsteroidsManager;
        
        public event System.Action<uint> OnScoreChanged;
        
        
        public void Initialize()
        {
            m_AsteroidsManager.OnAsteroidDestroyed += OnAsteroidsManagerOnOnAsteroidDestroyed;
        }
        public void Dispose()
        {
            m_AsteroidsManager.OnAsteroidDestroyed -= OnAsteroidsManagerOnOnAsteroidDestroyed;
        }

        private void OnAsteroidsManagerOnOnAsteroidDestroyed(AsteroidBehaviour asteroid)
        {
            switch (asteroid.Level)
            {
                case 0:
                    Score += 100;
                    break;
                case 1:
                    Score += 50;
                    break;
                case 2:
                    Score += 20;
                    break;
            }
        }
    }
}