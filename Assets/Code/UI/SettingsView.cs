using LitMotion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace UI
{
    public class SettingsView : MonoBehaviour
    {
        [Header("View")]
        [SerializeField] private Image m_Background;
        [SerializeField] private CanvasGroup m_Content;
        
        [Header("Settings")]
        [SerializeField] private Slider       m_MasterVolumeSlider;
        [SerializeField] private Slider       m_MusicVolumeSlider;
        [SerializeField] private TMP_Dropdown m_LanguageDropdown;

        [Inject] private readonly Preferences m_Preferences;

        private RectTransform m_ContentRect;
        private MotionHandle  m_MotionHandle;

        private void Awake()
        {
            m_ContentRect = m_Content.transform as RectTransform;
            
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
        
        public void Show()
        {
            gameObject.SetActive(true);
            
            m_MotionHandle.TryComplete();
            m_MotionHandle = LMotion.Create(0.0f, 1.0f, 0.25f)
                                     .WithEase(Ease.OutSine)
                                     .Bind(time =>
                                     {
                                         m_Background.color = new Color(0.0f, 0.0f, 0.0f, time * 0.99f);
                                         
                                         m_Content.alpha    = time;
                                         m_ContentRect.anchoredPosition = new Vector2(0.0f, Mathf.Lerp(-100.0f, 0.0f, time));
                                     });
        }
        public void Hide()
        {
            m_Preferences.Save();

            m_MotionHandle.TryComplete();
            m_MotionHandle = LMotion.Create(0.0f, 1.0f, 0.1f)
                                    .WithEase(Ease.OutSine)
                                    .WithOnComplete(() => gameObject.SetActive(false))
                                    .Bind(time =>
                                    {
                                        float invTime = 1.0f - time;
                                        
                                        m_Background.color = new Color(0.0f, 0.0f, 0.0f, invTime * 0.99f);
                                        
                                        m_Content.alpha    = invTime;
                                        m_ContentRect.anchoredPosition = new Vector2(0.0f, Mathf.Lerp(0.0f, 100.0f, time));
                                    });
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