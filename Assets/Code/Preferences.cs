using Audio.Music;
using UnityEngine;
using UnityEngine.Localization.Settings;
using VContainer;
using VContainer.Unity;

public class Preferences : IStartable
{
    #region Fields

    public float GlobalVolume = 1.0f;
    public float MusicVolume  = 0.1f;
    public int   Language     = 0;

    #region Dependencies

    [Inject] private readonly MusicPlayer m_MusicPlayer;

    #endregion

    #endregion


    public void Start()
    {
        if (HasSaved())
        {
            Load();
            return;
        }

        Language = LocalizationSettings.AvailableLocales.Locales.IndexOf(LocalizationSettings.SelectedLocale);
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
        Language = Mathf.Clamp(Language, 0, LocalizationSettings.AvailableLocales.Locales.Count - 1);
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[Language];
    }
}