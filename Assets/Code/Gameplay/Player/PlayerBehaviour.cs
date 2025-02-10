using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Gameplay.Asteroids;
using Gameplay.Enemies;
using Gameplay.Projectiles;
using UnityEngine;

namespace Gameplay.Player
{
    public class PlayerBehaviour : MonoBehaviour, IProjectileCollision, IAsteroidCollision, IEnemyCollision, IEnemyTarget
    {
        #region Fields

        public int Lives
        {
            get => m_Lives;
            set
            {
                if (value < 0)
                    value = 0;
                
                if (m_Lives == value)
                    return;
                
                if(value < Lives)
                    OnDamaged.Invoke();
                else
                    OnRegenerated.Invoke();
                
                if(!IsAlive && value > 0)
                    Revive();

                m_Lives = value;
                OnLivesChanged(value);

                if (m_Lives <= 0)
                {
                    m_DeathTokenSource = new CancellationTokenSource();
                    Death(m_DeathTokenSource.Token).Forget();
                }
            }
        }
        private int  m_Lives = 3;
        public  bool IsAlive      => Lives > 0;

        public bool Invulnerable
        {
            get => m_Invulnerable;
            private set
            {
                gameObject.layer = LayerMask.NameToLayer(value ? "Invulnerable" : "Default");

                m_Invulnerable   = value;
                OnInvulnerabilityChanged(value);
            }
        }
        private bool m_Invulnerable;

        public Vector2 Position => Movement.Position;
        public Vector2 Velocity => Movement.Velocity;
        
        public ProjectileLayer ProjectileLayer => ProjectileLayer.Player;
        
        private CancellationTokenSource m_DeathTokenSource;
        
        #region Components

        public PlayerMovement Movement { get; private set; }
        public PlayerGunner   Shooting { get; private set; }

        #endregion
        
        #endregion

        #region Events

        /// <summary>
        /// Invokes when player's invulnerability state changes.
        /// </summary>
        public event Action<bool> OnInvulnerabilityChanged = delegate { };

        /// <summary>
        /// Invokes when player's lives count changes (new value, old value).
        /// </summary>
        public event Action<int> OnLivesChanged = delegate { };
        
        /// <summary>
        /// Invokes when player takes damage.
        /// </summary>
        public event Action OnDamaged = delegate { };
        
        /// <summary>
        /// Invokes when player regenerates health.
        /// </summary>
        public event Action OnRegenerated = delegate { };

        /// <summary>
        /// Invokes when player dies.
        /// </summary>
        public event Action OnDeath = delegate { };

        /// <summary>
        /// Invokes when player explodes.
        /// </summary>
        public event Action OnExploded = delegate { };
        
        /// <summary>
        /// Invokes when player revives.
        /// </summary>
        public event Action OnRevived = delegate { };

        #endregion


        #region Unity Methods

        private void Awake()
        {
            Movement = GetComponentInChildren<PlayerMovement>();
            Shooting = GetComponentInChildren<PlayerGunner>();
        }

        #endregion
        
        #region Collision

        public void OnAsteroidCollision(AsteroidBehaviour asteroid)
        {
            if (Invulnerable)
                return;
            
            TakeDamage();
        }
        public void OnProjectileCollision(ProjectileCollisionData projectile)
        {
            if (Invulnerable)
            {
                projectile.Ignore();
                return;
            }
            
            TakeDamage();
        }
        public void OnEnemyCollision(EnemyBehaviour enemy)
        {
            if (Invulnerable)
                return;
            
            TakeDamage();
        }

        #endregion
        
        private void TakeDamage()
        {
            Lives--;

            if (IsAlive)
                GiveInvulnerability(2.0f);
        }
        private void Revive()
        {
            m_DeathTokenSource?.Cancel();
            
            Movement.IsControllable = true;
            Shooting.IsControllable = true;
            
            gameObject.SetActive(true);
            
            OnRevived.Invoke();
        }
        
        public void GiveInvulnerability(float duration) => InvulnerabilityRoutine(duration).Forget();
        private async UniTaskVoid InvulnerabilityRoutine(float duration)
        {
            Invulnerable = true;
            await UniTask.Delay(TimeSpan.FromSeconds(duration));
            Invulnerable = false;
        }
        
        private async UniTaskVoid Death(CancellationToken token)
        {
            Movement.IsControllable = false;
            Shooting.IsControllable = false;
            
            OnDeath.Invoke();
            await UniTask.WaitForSeconds(3.0f, cancellationToken: token);
            
            OnExploded.Invoke();
            gameObject.SetActive(false);
        }
    }
}