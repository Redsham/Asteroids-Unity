using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Gameplay.Player;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using YandexSdk;

namespace Managers.Gameplay
{
    public class GameManager : IStartable, IDisposable
    {
        #region Fields

        [Inject] private readonly PlayerBehaviour m_Player;
        [Inject] private readonly WavesManager    m_WavesManager;
        
        private CancellationTokenSource m_WaveTokenSource;

        #endregion

        #region Events

        public event Action OnBeginPlay = delegate { };
        public event Action OnEndPlay   = delegate { };

        #endregion


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
            OnBeginPlay.Invoke();
            YandexGamesSdk.Playing = true;
            Debug.Log("[GameManager] Game loop started");
            
            m_Player.Movement.Position = Vector2.zero;
            m_Player.Movement.Rotation = 0.0f;
            
            while (m_Player.IsAlive)
            {
                m_WaveTokenSource = new CancellationTokenSource();
                await m_WavesManager.StartWave(m_WaveTokenSource.Token);
            }

            m_WavesManager.ClearWave();
            
            OnEndPlay.Invoke();
            YandexGamesSdk.Playing = false;
            Debug.Log("[GameManager] Game loop ended");
        }

        private void OnPlayerDeath()
        {
            m_WaveTokenSource?.Cancel();
            Debug.Log("[GameManager] Player died");
        }

        public void Revive()
        {
            m_WavesManager.Wave--;
            m_Player.Lives++;
            
            GameLoop().Forget();
            
            Debug.Log("[GameManager] Player revived");
        }
    }
}