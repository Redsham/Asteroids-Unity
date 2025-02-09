using Gameplay.Asteroids;
using Gameplay.Enemies;
using Gameplay.Enemies.Variants;
using Gameplay.Player;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Managers
{
    public class GameManager : IStartable
    {
        [Inject] private readonly PlayerBehaviour  m_PlayerBehaviour;
        
        [Inject] private readonly AsteroidsManager m_Asteroids;
        [Inject] private readonly EnemiesManager   m_Enemies;
        
        public void Start()
        {
            m_Enemies.Spawn(typeof(SmallUfo), Vector2.left * 10.0f, m_PlayerBehaviour);
            m_Enemies.Spawn(typeof(BigUfo), Vector2.right * 10.0f, m_PlayerBehaviour);
        }
    }
}