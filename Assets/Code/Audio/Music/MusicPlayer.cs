using System;
using System.Threading;
using Audio.Assets;
using Audio.Sources;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Audio.Music
{
    public class MusicPlayer : MonoBehaviour
    {
        #region Fields

        [SerializeField] private float          m_TransitionTime  = 1.0f;
        [SerializeField] private AnimationCurve m_TransitionCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
        
        [SerializeField] private float          m_StopTransitionTime  = 1.0f;
        [SerializeField] private AnimationCurve m_StopTransitionCurve = AnimationCurve.Linear(0.0f, 1.0f, 1.0f, 0.0f);
        
        [SerializeField] private bool          m_PlayOnStart = true;
        [SerializeField] private PlaylistAsset m_Playlist    = null;
        
        private UniAudioSource<InterfaceAudioAsset> m_Source;
        private CancellationTokenSource             m_Cts;
        private UniTask                             m_PlayTask;

        public float Volume
        {
            get => m_Volume;
            set
            {
                m_Volume = value;
                m_Source.Volume = m_ControlledVolume * m_Volume;
            }
        }
        private float ControlledVolume
        {
            get => m_ControlledVolume;
            set
            {
                m_ControlledVolume = value;
                m_Source.Volume = m_ControlledVolume * m_Volume;
            }
        }

        private float m_Volume           = 0.1f;
        private float m_ControlledVolume = 1.0f;

        #endregion


        private void Awake()
        {
            m_Source = gameObject.AddComponent<InterfaceAudioSource>();
        }
        private void Start()
        {
            if (m_PlayOnStart)
            {
                if (m_Playlist == null)
                    throw new NullReferenceException("Playlist is not set");

                m_PlayTask = PlayRoutine();
            }
        }
        
        public void Play() => m_PlayTask = PlayRoutine();
        public void Stop() => StopRoutine().Forget();
        public void ChangePlaylist(PlaylistAsset playlist) => ChangePlaylistRoutine(playlist).Forget();
        
        private async UniTask PlayRoutine()
        {
            if (m_Playlist == null)
                throw new NullReferenceException("Playlist is not set");
            
            m_Cts = new CancellationTokenSource();
            CancellationToken token = m_Cts.Token;
            
            while (!token.IsCancellationRequested)
            {
                InterfaceAudioAsset audioAsset = m_Playlist.GetNext();
                
                m_Source.Asset = audioAsset;
                m_Source.Play();

                ControlledVolume = 1.0f;
                
                // Wait for the audio to finish
                float duration = audioAsset.Clip.length - m_TransitionTime;
                do
                {
                    duration -= Time.deltaTime;
                    await UniTask.Yield();
                }
                while (duration > 0.0f && !token.IsCancellationRequested);
                
                float transition = 0.0f;
                do
                {
                    transition       += Time.deltaTime / m_TransitionTime;
                    ControlledVolume =  m_TransitionCurve.Evaluate(transition);
                    
                    await UniTask.Yield();
                } while (transition < 1.0f && !token.IsCancellationRequested);
            }
        }
        private async UniTask StopRoutine()
        {
            if(m_PlayTask.Status != UniTaskStatus.Pending)
                return;
            
            float transition = 0.0f;
            do
            {
                transition       += Time.deltaTime / m_StopTransitionTime;
                ControlledVolume =  m_StopTransitionCurve.Evaluate(transition);
                await UniTask.Yield();
            } while (transition < 1.0f);
            
            m_Source.Stop();
            m_Cts.Cancel();
        }
        private async UniTaskVoid ChangePlaylistRoutine(PlaylistAsset playlist)
        {
            if (playlist == null)
                throw new NullReferenceException("Playlist is not set");
            
            await StopRoutine();
            m_PlayTask = PlayRoutine();
        }
    }
}