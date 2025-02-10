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
        [Inject] private readonly PlayerBehaviour m_Player;
        [Inject] private readonly WavesManager    m_WavesManager;
        
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
        }
        private void OnPlayerDeath() => m_WaveTokenSource?.Cancel();

        public void Revive()
        {
            m_WavesManager.Wave--;
            m_Player.Lives++;
            
            GameLoop().Forget();
        }
    }
}