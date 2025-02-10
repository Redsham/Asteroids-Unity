using System;
using Gameplay.Asteroids;
using Gameplay.Enemies;
using Gameplay.Player;
using VContainer;
using VContainer.Unity;

namespace Managers.Gameplay
{
    public class ScoreManager : IInitializable, IDisposable
    {
        #region Fields

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
        
        [Inject] private readonly AsteroidsManager m_Asteroids;
        [Inject] private readonly EnemiesManager   m_Enemies;
        [Inject] private readonly WavesManager     m_Waves;
        [Inject] private readonly PlayerBehaviour  m_Player;
        
        #endregion

        public event Action<uint> OnScoreChanged;

        
        #region Lifecycle

        public void Initialize()
        {
            m_Asteroids.OnAsteroidDestroyed += OnAsteroidDestroyed;
            m_Enemies.OnEnemyDestroyed      += OnEnemyDestroyed;
        }
        public void Dispose()
        {
            m_Asteroids.OnAsteroidDestroyed -= OnAsteroidDestroyed;
            m_Enemies.OnEnemyDestroyed      -= OnEnemyDestroyed;
        }

        #endregion

        #region Handlers

        private void OnAsteroidDestroyed(AsteroidBehaviour asteroid)
        {
            switch (asteroid.Level)
            {
                case AsteroidLevel.Small:
                    Score += 100;
                    break;
                case AsteroidLevel.Medium:
                    Score += 50;
                    break;
                case AsteroidLevel.Large:
                    Score += 20;
                    break;
            }
        }
        private void OnEnemyDestroyed(EnemyBehaviour enemy) => Score += enemy.Score;

        #endregion
    }
}