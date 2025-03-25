using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Swift_Blade.Level
{
    public class LevelUIController : MonoBehaviour
    {
        public SceneManagerSO levelEvent;
        
        [Header("Fade info")]
        [SerializeField] private Image fadeImage;
        [Range(0.1f, 10)] public float fadeInTime;
        [Range(0.1f, 10)] public float fadeOutTime;
        private bool isFading;
        
        [Header("Next Level info")]
        [SerializeField] private Image header;
        [SerializeField] private GameObject[] elements;
        [SerializeField] private TextMeshProUGUI[] buttonTexts;
        
        [SerializeField] private float elementsFadeInTime;
        [Range(0.1f ,2)] [SerializeField] private float headerSizeUpDuration;
        
        
        protected void Awake()
        {
            LevelUIController existingInstance = FindObjectOfType<LevelUIController>();
            
            if (existingInstance != null && existingInstance != this)
            {
                Destroy(existingInstance.gameObject);
            }
            
            DontDestroyOnLoad(gameObject);
        }
        
        private void Start()
        {
            levelEvent.SceneLoadEvent += StartFade;
            
            ResetClearPanel();
        }

        protected  void OnDestroy()
        {
            levelEvent.SceneLoadEvent -= StartFade;
        }
        private void StartFade(string sceneName,Action onComplete)
        {
            if (isFading) return;
            
            isFading = true;
            fadeImage.DOFade(1, fadeInTime).OnComplete(() =>
            {
                SceneManager.LoadScene(sceneName);
                FadeOut(onComplete);
            });
        }
        private void FadeOut(Action onComplete)
        {
            onComplete?.Invoke();
            fadeImage.DOFade(0, fadeOutTime).OnComplete(() =>
            {
                isFading = false;
            });
        }
        
        private void SetActiveClearPanel()
        {
            header.gameObject.SetActive(true);
            header.DOKill();
                        
             Sequence sequence = DOTween.Sequence();
            
            sequence.Append(header.rectTransform.DOSizeDelta(new Vector2(header.rectTransform.sizeDelta.x, 20), headerSizeUpDuration).SetDelay(1.2f))
                .Append(header.rectTransform.DOSizeDelta(new Vector2(header.rectTransform.sizeDelta.x, 930), headerSizeUpDuration))
                .OnComplete(FadeInElements);

            sequence.SetUpdate(UpdateType.Normal);
        }
        
        private void ResetClearPanel()
        {
            header.DOKill();
            
            Sequence sequence = DOTween.Sequence();
            sequence.Append(FadeOutElements());
            sequence.Append(header.rectTransform.DOSizeDelta(new Vector2(header.rectTransform.sizeDelta.x, 0),headerSizeUpDuration));
            sequence.AppendCallback(()=>header.gameObject.SetActive(false));                
            
        }
        
        private void FadeInElements()
        {
            Sequence sequence = DOTween.Sequence();

            foreach (var item in elements)
            {
                if (item.TryGetComponent(out TextMeshProUGUI text))
                {
                    sequence.Join(text.DOFade(1, elementsFadeInTime));
                }

                if (item.TryGetComponent(out Image image))
                {
                    sequence.Join(image.DOFade(1, elementsFadeInTime));
                }
            }

            sequence.AppendInterval(0.2f);
    
            foreach (var item in buttonTexts)
            {
                sequence.Join(item.DOFade(1, elementsFadeInTime));
            }
        }

        private Tween FadeOutElements()
        {
            Sequence fadeSequence = DOTween.Sequence();
            
            foreach (var item in buttonTexts)
            {
                fadeSequence.Join(item.DOFade(0, elementsFadeInTime));
            }

            foreach (var item in elements)
            {
                if (item.TryGetComponent(out TextMeshProUGUI text))
                {
                    fadeSequence.Join(text.DOFade(0, elementsFadeInTime));
                }

                if (item.TryGetComponent(out Image image))
                {
                    fadeSequence.Join(image.DOFade(0, elementsFadeInTime));
                }
            }

            return fadeSequence;
        }

        
    }
}
