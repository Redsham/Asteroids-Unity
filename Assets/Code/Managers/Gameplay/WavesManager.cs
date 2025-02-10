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

namespace Managers.Gameplay
{
    public class WavesManager
    {
        #region Constants

        private const uint MAX_ASTEROIDS  = 12u;
        private const uint MAX_BIG_UFOS   = 6u;
        private const uint MAX_SMALL_UFOS = 3u;

        #endregion
        
        #region Fields

        [Inject] private readonly AsteroidsManager      m_Asteroids;
        [Inject] private readonly EnemiesManager        m_Enemies;
        [Inject] private readonly UnboundedSpaceManager m_UnboundedSpace;

        [Inject] private readonly PlayerBehaviour m_Player;

        public uint Wave { get; set; }

        #endregion
        
        #region Events
        
        public event Action<uint> OnWaveStarted = delegate { };
        public event Action      OnWaveEnded = delegate { };
        
        #endregion


        public async UniTask StartWave(CancellationToken token)
        {
            Wave++;
            OnWaveStarted.Invoke(Wave);
            
            await UniTask.WaitForSeconds(3.0f, cancellationToken: token);
            
            Compute(Wave, out uint asteroidsCount, out uint bigUfosCount, out uint smallUfosCount);

            await UniTask.WhenAll(
                SpawnAsteroids(asteroidsCount, MAX_ASTEROIDS, token),
                SpawnUfos(typeof(BigUfo), bigUfosCount, MAX_BIG_UFOS, token),
                SpawnUfos(typeof(SmallUfo), smallUfosCount, MAX_SMALL_UFOS, token)
            );

            await UniTask.WaitUntil(() => (m_Asteroids.Count == 0 && m_Enemies.Count == 0) || token.IsCancellationRequested);
            
            OnWaveEnded.Invoke();
        }
        public void ClearWave()
        {
            m_Asteroids.Clear();
            m_Enemies.Clear();
        }
        
        private static void Compute(uint wave, out uint asteroidsCount, out uint bigUfosCount, out uint smallUfosCount)
        {
            smallUfosCount = wave / 10u;
            bigUfosCount   = wave / 5u - smallUfosCount;
            asteroidsCount = wave * 2u - smallUfosCount * 8u - bigUfosCount * 10u;
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
        
        private async UniTask SpawnAsteroids(uint count, uint limit, CancellationToken token = default)
        {
            for (int i = 0; i < count && !token.IsCancellationRequested; i++)
            {
                m_Asteroids.Spawn(GetSpawnPosition(5.0f), Random.insideUnitCircle.normalized * Random.Range(3.0f, 5.0f), AsteroidLevel.Large);
                await UniTask.WaitUntil(() => m_Asteroids.Count < limit || token.IsCancellationRequested);
            }
        }
        private async UniTask SpawnUfos(Type type, uint count, uint limit, CancellationToken token = default)
        {
            for (int i = 0; i < count && !token.IsCancellationRequested; i++)
            {
                m_Enemies.Spawn(type, GetSpawnPosition(5.0f), m_Player);
                await UniTask.WaitUntil(() => m_Asteroids.Count < limit || token.IsCancellationRequested);
            }
        }
    }
}