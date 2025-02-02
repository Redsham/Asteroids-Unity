using System;
using Cysharp.Threading.Tasks;
using Gameplay.Asteroids;
using Gameplay.Projectiles;
using UnityEngine;

namespace Gameplay.Player
{
    public class PlayerBehaviour : MonoBehaviour, IProjectileCollision, IAsteroidCollision
    {
        public int Lives
        {
            get => m_Lives;
            set
            {
                if (m_Lives == value)
                    return;
                
                OnLivesChanged(value, m_Lives);
                m_Lives = value;
                
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
        
        public bool IsAlive      => Lives > 0;

        private int  m_Lives = 4;
        private bool m_Invulnerable;

        #region Events

        /// <summary>
        /// Invokes when player's invulnerability state changes.
        /// </summary>
        public event Action<bool>     OnInvulnerabilityChanged = delegate { };
        /// <summary>
        /// Invokes when player's lives count changes (new value, old value).
        /// </summary>
        public event Action<int, int> OnLivesChanged           = delegate { };
        /// <summary>
        /// Invokes when player dies.
        /// </summary>
        public event Action           OnDeath                  = delegate { };

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