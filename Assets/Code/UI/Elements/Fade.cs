using System;
using Cysharp.Threading.Tasks;
using LitMotion;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace UI.Elements
{
    [RequireComponent(typeof(Image))]
    public class Fade : MonoBehaviour
    {
        private Image m_Image;
        
        
        [Inject]
        public void Construct()
        {
            gameObject.SetActive(false);
            m_Image = GetComponent<Image>();
        }
        
        
        public async UniTask Show(Func<UniTask> fadeTask)
        {
            gameObject.SetActive(true);
            
            await LMotion.Create(0.0f, 1.0f, 0.25f)
                .Bind(value => m_Image.color = new Color(0.0f, 0.0f, 0.0f, value));
            
            await fadeTask.Invoke();
            
            await LMotion.Create(1.0f, 0.0f, 0.25f)
                .Bind(value => m_Image.color = new Color(0.0f, 0.0f, 0.0f, value));
            
            gameObject.SetActive(false);
        }
    }
}