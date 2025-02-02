using System;
using Audio.Assets;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace Audio
{
    public class AudioManager : IAudioManager, IInitializable, IDisposable
    {
        private GameObject              m_Root;
        
        private ObjectPool<AudioSource> m_InterfacePool;
        private ObjectPool<AudioSource> m_SfxPool;
        
        
        public void Initialize()
        {
            m_Root = new GameObject("Audio");
            Object.DontDestroyOnLoad(m_Root);
            
            m_InterfacePool = new ObjectPool<AudioSource>(() =>
            {
                AudioSource audioSource = new GameObject("UI Audio Source").AddComponent<AudioSource>();
                audioSource.transform.SetParent(m_Root.transform);
                
                audioSource.playOnAwake           = false;
                audioSource.loop                  = false;
                audioSource.spatialize            = false;
                audioSource.spatialBlend          = 0.0f;
                audioSource.volume                = 1.0f;
                audioSource.bypassEffects         = true;
                audioSource.bypassListenerEffects = true;
                audioSource.bypassReverbZones     = true;
                audioSource.priority              = 0;
                
                return audioSource;
            });
            
            m_SfxPool = new ObjectPool<AudioSource>(() =>
            {
                AudioSource audioSource = new GameObject("SFX Audio Source").AddComponent<AudioSource>();
                audioSource.transform.SetParent(m_Root.transform);

                audioSource.playOnAwake  = false;
                audioSource.loop         = false;
                audioSource.spatialBlend = 1.0f;
                audioSource.rolloffMode  = AudioRolloffMode.Linear;
                
                return audioSource;
            });
        }
        public void Dispose()
        {
            m_InterfacePool.Dispose();
            Object.Destroy(m_Root);
        }

        public void Play(AudioAsset asset)
        {
            switch (asset)
            {
                case InterfaceAudioAsset interfaceAudioAsset:
                    PlayUIInternal(interfaceAudioAsset).Forget();
                    break;
                case SfxAudioAsset sfxAudioAsset:
                    PlaySfxInternal(sfxAudioAsset, Vector2.zero).Forget();
                    break;
            }
        }
        public void PlaySfx(SfxAudioAsset asset, Vector2 position) => PlaySfxInternal(asset, position).Forget();
        
        private async UniTaskVoid PlayUIInternal(InterfaceAudioAsset asset)
        {
            AudioSource audioSource = m_InterfacePool.Get();
            audioSource.PlayOneShot(asset.Clip);
            
            await UniTask.WaitForSeconds(asset.Clip.length);
            
            m_InterfacePool.Release(audioSource);
        }
        private async UniTaskVoid PlaySfxInternal(SfxAudioAsset asset, Vector2 position)
        {
            AudioSource audioSource = m_SfxPool.Get();
            audioSource.transform.position = position;
            audioSource.clip = asset.Clip;
            
            audioSource.volume       = asset.Volume;
            audioSource.pitch        = asset.Pitch;
            audioSource.spatialBlend = asset.SpatialBlend;
            audioSource.minDistance  = asset.Distance.Min;
            audioSource.maxDistance  = asset.Distance.Max;
            
            audioSource.Play();
            
            await UniTask.WaitForSeconds(asset.Clip.length * (1.0f / asset.Pitch));
            
            m_SfxPool.Release(audioSource);
        }
    }
}