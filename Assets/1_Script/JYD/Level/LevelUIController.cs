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
        public TextMeshProUGUI clearText;

        [SerializeField] private GameObject[] elements;
        
        public float headerMaxSize = 200;
        [Range(0.1f, 2)] public float headerFadeInDuration;
        [Range(0.1f ,2)] public float headerSizeUpDuration;
        [Range(0.1f , 2)] public float clearTextFadeInDuration;

        private StringBuilder currentSceneName = new StringBuilder();
        
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            
            levelEvent.SceneMoveEvent += StartFade;
            levelEvent.LevelClearEvent += SetActiveClearPanel;
            levelEvent.SceneChangeEvent += NextOtherScene;

            foreach (var element in elements)
            {
                element.transform.localScale = Vector3.zero;
            }
            
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
                foreach (var elem in elements)
                {
                    elem.transform.DOKill();
                    elem.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBounce);
                }
            });
        }
                
        private void SetActiveClearPanel()
        {
            header.gameObject.SetActive(true);

            header.DOKill();
            header.transform.DOScaleY(1, headerSizeUpDuration);
        }
        private void ResetClearPanel()
        {
            header.DOKill();
            header.transform.DOScaleY(0, headerSizeUpDuration).OnComplete(() =>
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
