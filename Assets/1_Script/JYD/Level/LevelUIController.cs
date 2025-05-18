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
                
        protected void Awake()
        {
            LevelUIController existingInstance = FindFirstObjectByType<LevelUIController>();
            
            if (existingInstance != null && existingInstance != this)
            {
                Destroy(existingInstance.gameObject);
            }
            
            DontDestroyOnLoad(gameObject);
        }
        
        private void Start()
        {
            levelEvent.SceneLoadEvent += StartFade;
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
            }).SetLink(gameObject, LinkBehaviour.KillOnDestroy);
        }
        private void FadeOut(Action onComplete)
        {
            fadeImage.DOFade(0, fadeOutTime).OnComplete(() =>
            {
                onComplete?.Invoke();
                isFading = false;
            }).SetLink(gameObject, LinkBehaviour.KillOnDestroy);
        }
    }
}
