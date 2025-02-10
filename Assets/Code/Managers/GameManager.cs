using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Gameplay.Player;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Managers
{
    public class GameManager : IStartable, IDisposable
    {
        [Inject] private PlayerBehaviour m_Player;
        [Inject] private WavesManager    m_WavesManager;
        
        private CancellationTokenSource m_WaveTokenSource;
        
        
        public void Start()
        {
            m_Player.OnDeath += OnPlayerDeath;
            GameLoop().Forget();
        }
        public void Dispose()
        {
            m_Player.OnDeath -= OnPlayerDeath;
            m_WaveTokenSource?.Cancel();
        }
        
        private async UniTask GameLoop()
        {
            while (m_Player.IsAlive)
            {
                m_WaveTokenSource = new CancellationTokenSource();
                await m_WavesManager.StartWave(m_WaveTokenSource.Token);
            }

            m_WavesManager.ClearWave();
            
            await UniTask.WaitForSeconds(2.0f);

            m_Player.Lives++;
            Debug.Log($"Player revived. Lives: {m_Player.Lives}");
        }
        private void OnPlayerDeath()
        {
            m_WaveTokenSource?.Cancel();
        }
    }
}