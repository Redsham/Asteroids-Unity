using System;
using Cysharp.Threading.Tasks;
using LitMotion;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Elements
{
    public class ModalWindow : MonoBehaviour
    {
        public bool               IsVisible { get; private set; }
        public event Action<bool> OnVisibilityChanged = delegate {  };
        
        [Header("Modal Window")]
        [SerializeField] protected Image       BackgroundComponent;
        [SerializeField] protected CanvasGroup BodyComponent;
        
        protected MotionHandle  WindowAnimation;
        protected RectTransform BodyRect => BodyComponent.transform as RectTransform;
        
        
        public virtual void Show()
        {
            gameObject.SetActive(true);
            
            IsVisible = true;
            OnVisibilityChanged.Invoke(IsVisible);
            
            WindowAnimation.TryComplete();
            WindowAnimation = LMotion.Create(0.0f, 1.0f, 0.25f)
                                    .WithEase(Ease.OutSine)
                                    .Bind(time =>
                                    {
                                        BackgroundComponent.color = new Color(0.0f, 0.0f, 0.0f, time * 0.99f);
                                        
                                        BodyComponent.alpha    = time;
                                        BodyRect.anchoredPosition = new Vector2(0.0f, Mathf.Lerp(-100.0f, 0.0f, time));
                                    });
        }
        public virtual void Hide()
        {
            IsVisible = false;
            OnVisibilityChanged.Invoke(IsVisible);
            
            WindowAnimation.TryComplete();
            WindowAnimation = LMotion.Create(0.0f, 1.0f, 0.1f)
                                     .WithEase(Ease.OutSine)
                                     .WithOnComplete(() => gameObject.SetActive(false))
                                     .Bind(time =>
                                     {
                                         float invTime = 1.0f - time;
                                        
                                         BackgroundComponent.color = new Color(0.0f, 0.0f, 0.0f, invTime * 0.99f);
                                        
                                         BodyComponent.alpha    = invTime;
                                         BodyRect.anchoredPosition = new Vector2(0.0f, Mathf.Lerp(0.0f, 100.0f, time));
                                     });
        }

        public async UniTask ShowAndWait()
        {
            Show();
            await UniTask.WaitWhile(() => IsVisible);
        }
    }
}