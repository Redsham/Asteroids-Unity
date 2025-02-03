using UnityEngine;

namespace Audio
{
    public abstract class UniAudioSource<T> : MonoBehaviour where T : UniAudioAsset
    {
        public T Asset
        {
            get => m_Asset;
            set
            {
                m_Asset        = value;
                AudioSource.clip = m_Asset.Clip;
            }
        }
        
        public float Volume
        {
            get => InstanceVolume;
            set
            {
                InstanceVolume   = value;
                ApplyVolumeAndPitch();
            }
        }
        public float Pitch
        {
            get => InstancePitch;
            set
            {
                InstancePitch   = value;
                ApplyVolumeAndPitch();
            }
        }
        
        [SerializeField] private T m_Asset;

        [SerializeField] protected float InstanceVolume        = 1.0f;
        [SerializeField] protected float InstancePitch         = 1.0f;
        [SerializeField] protected bool  InstancePlayOnAwake   = true;

        protected AudioSource AudioSource { get; private set; }

        
        private void Awake()
        {
            if(m_Asset.PlayOnManager)
                return;
            
            AudioSource = gameObject.AddComponent<AudioSource>();
            Setup(AudioSource);
        }
        private void Start()
        {
            if (m_Asset.PlayOnAwake && InstancePlayOnAwake)
                Play();
        }
        private void OnValidate()
        {
            if (AudioSource == null)
                return;
            
            ApplyVolumeAndPitch();
        }
        
        protected void ApplyVolumeAndPitch()
        {
            if(m_Asset.PlayOnManager)
                return;
            
            AudioSource.volume = m_Asset.Volume * Volume;
            AudioSource.pitch  = m_Asset.Pitch * Pitch;
        }

        protected virtual void Setup(AudioSource audioSource)
        {
            audioSource.clip         = m_Asset.Clip;
            audioSource.playOnAwake  = m_Asset.PlayOnAwake && InstancePlayOnAwake;
            audioSource.loop         = m_Asset.Loop;
            audioSource.rolloffMode  = AudioRolloffMode.Linear;
        }
        
        public void Play()
        {
            if (m_Asset.PlayOnManager)
                PlayOnManager();
            else
                PlaySelf();
        }
        protected abstract void PlaySelf();
        protected abstract void PlayOnManager();
    }
}