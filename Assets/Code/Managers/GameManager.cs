using System.Threading;
using Cysharp.Threading.Tasks;
using Gameplay.Player;
using VContainer;
using VContainer.Unity;

namespace Managers
{
    public class GameManager : IStartable
    {
        [Inject] private PlayerBehaviour m_Player;
        [Inject] private WavesManager    m_WavesManager;
        
        private CancellationTokenSource m_WaveTokenSource;
        
        public void Start()
        {
            m_Player.OnDeath += OnPlayerDeath;
            GameLoop().Forget();
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
        private void OnPlayerDeath()
        {
            m_WaveTokenSource?.Cancel();
        }
    }
}