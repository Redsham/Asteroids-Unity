using Audio;
using Audio.Music;
using UI.Elements;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

namespace Infrastructure
{
    public class RootScope : LifetimeScope
    {
        [SerializeField] private InputActionAsset m_InputActionAsset;
        
        protected override void Configure(IContainerBuilder builder)
        {
            // Register the InputActionAsset instance to the container
            builder.RegisterInstance(m_InputActionAsset);
            
            // Register AudioManager
            builder.RegisterEntryPoint<UniAudioManager>().As<IUniAudioManager>();
            
            // Register fade
            builder.RegisterComponentInHierarchy<Fade>();
            
            // Register Music Player
            builder.RegisterComponentInHierarchy<MusicPlayer>();
            
            // Register Preferences
            builder.RegisterEntryPoint<Preferences>().AsSelf();
        }
    }
}