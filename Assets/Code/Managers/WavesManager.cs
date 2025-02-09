using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Gameplay.Asteroids;
using Gameplay.Enemies;
using Gameplay.Enemies.Variants;
using Gameplay.Player;
using Gameplay.UnboundedSpace;
using UnityEngine;
using VContainer;
using Random = UnityEngine.Random;

namespace Managers
{
    public class WavesManager
    {
        #region Constants

        public const int MAX_ASTEROIDS = 12;
        public const int MAX_BIG_UFOS  = 6;
        public const int MAX_SMALL_UFOS = 3;

        #endregion
        
        #region Fields

        [Inject] private readonly AsteroidsManager      m_Asteroids;
        [Inject] private readonly EnemiesManager        m_Enemies;
        [Inject] private readonly UnboundedSpaceManager m_UnboundedSpace;

        [Inject] private readonly PlayerBehaviour m_Player;
        
        public  int Wave { get; private set; }

        #endregion
        
        #region Events
        
        public event Action<int> OnWaveStarted = delegate { };
        public event Action      OnWaveEnded = delegate { };
        
        #endregion


        public async UniTask StartWave(CancellationToken token)
        {
            Wave++;
            OnWaveStarted.Invoke(Wave);
            
            await UniTask.WaitForSeconds(3.0f, cancellationToken: token);
            
            Compute(Wave, out int asteroidsCount, out int bigUfosCount, out int smallUfosCount);

            await UniTask.WhenAll(
                SpawnAsteroids(asteroidsCount, MAX_ASTEROIDS, token),
                SpawnUfos(typeof(BigUfo), bigUfosCount, MAX_BIG_UFOS, token),
                SpawnUfos(typeof(SmallUfo), bigUfosCount, MAX_SMALL_UFOS, token)
            );

            await UniTask.WaitUntil(() => m_Asteroids.Count == 0 && m_Enemies.Count == 0, cancellationToken: token);
            
            OnWaveEnded.Invoke();
        }
        public void ClearWave()
        {
            m_Asteroids.Clear();
            m_Enemies.Clear();
        }
        
        private static void Compute(int wave, out int asteroidsCount, out int bigUfosCount, out int smallUfosCount)
        {
            smallUfosCount = wave / 10;
            bigUfosCount   = wave / 5 - smallUfosCount;
            asteroidsCount = wave * 2 - smallUfosCount * 8 - bigUfosCount * 10;
        }
        private Vector2 GetSpawnPosition(float safeRadius)
        {
            Vector2 position;
            Vector2 player = m_Player.Position;
            
            do
            {
                position = m_UnboundedSpace.Bounds.RandomPointOnEdge();
            } while (Vector2.Distance(player, position) < safeRadius);
            
            return position;
        }
        
        private async UniTask SpawnAsteroids(int count, int limit, CancellationToken token = default)
        {
            for (int i = 0; i < count && !token.IsCancellationRequested; i++)
            {
                m_Asteroids.Spawn(GetSpawnPosition(5.0f), Random.insideUnitCircle.normalized * Random.Range(3.0f, 5.0f), AsteroidLevel.Large);
                await UniTask.WaitUntil(() => m_Asteroids.Count < limit, cancellationToken: token);
            }
        }
        private async UniTask SpawnUfos(Type type, int count, int limit, CancellationToken token = default)
        {
            for (int i = 0; i < count && !token.IsCancellationRequested; i++)
            {
                m_Enemies.Spawn(type, GetSpawnPosition(5.0f), m_Player);
                await UniTask.WaitUntil(() => m_Asteroids.Count < limit, cancellationToken: token);
            }
        }
    }
}