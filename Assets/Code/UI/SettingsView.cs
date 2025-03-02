using LitMotion;
using Other;
using TMPro;
using UI.Elements;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace UI
{
    public class SettingsView : ModalWindow
    {
        [Header("Settings")]
        [SerializeField] private Slider       m_MasterVolumeSlider;
        [SerializeField] private Slider       m_MusicVolumeSlider;
        [SerializeField] private TMP_Dropdown m_LanguageDropdown;

        [Inject] private readonly Preferences m_Preferences;
        
        private void Awake()
        {
            m_MasterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
            m_MusicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
            m_LanguageDropdown.onValueChanged.AddListener(OnLanguageChanged);
        }
        private void Start()
        {
            m_MasterVolumeSlider.SetValueWithoutNotify(m_Preferences.GlobalVolume);
            m_MusicVolumeSlider.SetValueWithoutNotify(m_Preferences.MusicVolume);
            m_LanguageDropdown.SetValueWithoutNotify(m_Preferences.Language);
        }
        
        public override void Hide()
        {
            m_Preferences.Save();
            base.Hide();
        }
        
        #region Event Handlers

        private void OnMasterVolumeChanged(float value)
        {
            m_Preferences.GlobalVolume = value;
            m_Preferences.Apply();
        }
        private void OnMusicVolumeChanged(float value)
        {
            m_Preferences.MusicVolume = value;
            m_Preferences.Apply();
        }
        private void OnLanguageChanged(int value)
        {
            m_Preferences.Language = value;
            m_Preferences.Apply();
        }

        #endregion
    }
}