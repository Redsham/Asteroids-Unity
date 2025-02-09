using System;
using Cysharp.Threading.Tasks;
using Gameplay.Asteroids;
using Gameplay.Enemies;
using Gameplay.Projectiles;
using UnityEngine;

namespace Gameplay.Player
{
    public class PlayerBehaviour : MonoBehaviour, IProjectileCollision, IAsteroidCollision, IEnemyCollision, IEnemyTarget
    {
        public int Lives
        {
            get => m_Lives;
            set
            {
                if (m_Lives == value)
                    return;

                m_Lives = value;
                OnLivesChanged(value);

                if (m_Lives <= 0)
                    OnDeath();
            }
        }
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

        public Vector2 Position => Movement.Position;
        public Vector2 Velocity => Movement.Velocity;
        
        public ProjectileLayer ProjectileLayer => ProjectileLayer.Player;
        
        public bool IsAlive      => Lives > 0;

        private int  m_Lives = 3;
        private bool m_Invulnerable;
        
        #region Components

        public PlayerMovement Movement { get; private set; }
        public PlayerGunner   Shooting { get; private set; }

        #endregion
        
        #region Events

        /// <summary>
        /// Invokes when player's invulnerability state changes.
        /// </summary>
        public event Action<bool>     OnInvulnerabilityChanged = delegate { };
        /// <summary>
        /// Invokes when player's lives count changes (new value, old value).
        /// </summary>
        public event Action<int> OnLivesChanged           = delegate { };
        /// <summary>
        /// Invokes when player dies.
        /// </summary>
        public event Action           OnDeath                  = delegate { };

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
        
        public void GiveInvulnerability(float duration) => InvulnerabilityRoutine(duration).Forget();
        private async UniTaskVoid InvulnerabilityRoutine(float duration)
        {
            Invulnerable = true;
            await UniTask.Delay(TimeSpan.FromSeconds(duration));
            Invulnerable = false;
        }
    }
}