using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Swift_Blade.Level
{
    public class FadeController : MonoBehaviour
    {
        [SerializeField] private Image fadeImage;

        [Range(0.1f, 10)] public float fadeInTime;
        [Range(0.1f, 10)] public float fadeOutTime;
        private bool isFading;
        
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void StartFade(Action sceneMove,Action onComplete)
        {
            if (isFading) return;
    
            isFading = true;

            fadeImage.DOFade(1, fadeInTime).OnComplete(() =>
            {
                sceneMove.Invoke();
                FadeOut(onComplete);
            });
        }

        private void FadeOut(Action onComplete)
        {
            fadeImage.DOFade(0, fadeOutTime).OnComplete(() =>
            {
                isFading = false;
                onComplete?.Invoke();
            });
        }

    }
}
