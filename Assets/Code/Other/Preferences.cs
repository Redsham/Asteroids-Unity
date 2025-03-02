using System.Threading;
using Audio.Music;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Localization.Settings;
using VContainer;
using VContainer.Unity;

namespace Other
{
    public class Preferences : IAsyncStartable
    {
        #region Fields

        #region Preferences

        public float GlobalVolume = 1.0f;
        public float MusicVolume  = 0.1f;
        public int   Language     = -1;

        #endregion

        #region Dependencies

        [Inject] private readonly MusicPlayer m_MusicPlayer;

        #endregion
        
        public bool IsInitialized { get; private set; }

        #endregion

        
        public async UniTask StartAsync(CancellationToken cancellation = default)
        {
            await LocalizationSettings.InitializationOperation;
            
            if (HasSaved())
            {
                Load();
                IsInitialized = true;
                return;
            }
            
            Language      = LocalizationSettings.AvailableLocales.Locales.IndexOf(LocalizationSettings.SelectedLocale);
            IsInitialized = true;
        }
        
        public void Load()
        {
            // Deserialize the preferences from JSON
            string json = PlayerPrefs.GetString("Preferences", "{}");
            JsonUtility.FromJsonOverwrite(json, this);
        
            Apply();
        
            Debug.Log($"[Preferences] Preferences successfully loaded");
        }
        public void Save()
        {
            // Serialize the preferences to JSON
            string json = JsonUtility.ToJson(this);
            PlayerPrefs.SetString("Preferences", json);
        
            PlayerPrefs.Save();
        
            Debug.Log($"[Preferences] Preferences successfully saved");
        }
        public bool HasSaved() => PlayerPrefs.HasKey("Preferences");

        public void Apply()
        {
            // Audio
            AudioListener.volume = GlobalVolume;
            m_MusicPlayer.Volume = MusicVolume;
        
            // Localization
            Language                            = Mathf.Clamp(Language, 0, LocalizationSettings.AvailableLocales.Locales.Count - 1);
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[Language];
        }
    }
}