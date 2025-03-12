using System;
using System.Text;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Swift_Blade.Level
{
    public class LevelUIController : MonoBehaviour
    {
        public LevelClearEventSO levelEvent;
        
        [Header("Fade info")]
        [SerializeField] private Image fadeImage;
        [Range(0.1f, 10)] public float fadeInTime;
        [Range(0.1f, 10)] public float fadeOutTime;
        private bool isFading;
        
        [Header("Next Level info")]
        public Image header;
        [SerializeField] private Image[] elements;
        [SerializeField] private float elementsFadeInTime;
                
        public float headerMaxSize = 200;
        [Range(0.1f, 2)] public float headerFadeInDuration;
        [Range(0.1f ,2)] public float headerSizeUpDuration;
        [Range(0.1f , 2)] public float clearTextFadeInDuration;

        private StringBuilder currentSceneName = new StringBuilder();
        
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            levelEvent.SceneMoveEvent += StartFade;
            levelEvent.LevelClearEvent += SetActiveClearPanel;
            levelEvent.SceneChangeEvent += NextOtherScene;

            ResetClearPanel();
        }

        private void OnDestroy()
        {
            levelEvent.SceneMoveEvent -= StartFade;
            levelEvent.LevelClearEvent -= SetActiveClearPanel;
            levelEvent.SceneChangeEvent -= NextOtherScene;
        }

        private void StartFade(string sceneName,Action onComplete)
        {
            if (isFading) return;

            currentSceneName.Clear();
            currentSceneName.Append(sceneName);
            
            isFading = true;
            fadeImage.DOFade(1, fadeInTime).OnComplete(() =>
            {
                if (IsVailedScene(sceneName))
                {
                    SceneManager.LoadScene(sceneName);
                    FadeOut(onComplete);
                }
                else
                {
                    SceneManager.LoadScene("LevelMenu");
                    FadeOut(onComplete);
                }
                
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
            header.rectTransform.DOSizeDelta(new Vector2(header.rectTransform.sizeDelta.x , 20), headerSizeUpDuration).SetDelay(1.2f).OnComplete(() =>
            {
                header.rectTransform
                    .DOSizeDelta(new Vector2(header.rectTransform.sizeDelta.x, 930), headerSizeUpDuration).OnComplete(
                        () =>
                        {
                            foreach (var item in elements)
                            {
                                item.DOFade(1, elementsFadeInTime);
                            }
                        });
            });
        }
        private void ResetClearPanel()
        {
            header.DOKill();
            header.rectTransform.DOSizeDelta(new Vector2(header.rectTransform.sizeDelta.x , 0), headerSizeUpDuration).OnComplete(() =>
            {
                
                header.gameObject.SetActive(false);
            });
        }

        public void NextScene()
        {
            ResetClearPanel();
            
            char lastChar = currentSceneName[currentSceneName.Length - 1];

            if (char.IsDigit(lastChar))
            {
                int lastNumber = lastChar - '0'; 
                lastNumber++; 

                currentSceneName.Remove(currentSceneName.Length - 1, 1).Append(lastNumber);
            }
            
            StartFade(currentSceneName.ToString() , () => { });
            
        }
        
        private void NextOtherScene(string sceneName)
        {
            StartFade(sceneName, () => {}); 
        }

        private bool IsVailedScene(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName))
                return false;
            
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                string path = SceneUtility.GetScenePathByBuildIndex(i);
                string scenePath = System.IO.Path.GetFileNameWithoutExtension(path);
                if (scenePath == sceneName)
                    return true;
            }
            
            return false;
        }
        
    }
}
