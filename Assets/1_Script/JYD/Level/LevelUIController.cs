using System;
using System.Text;
using DG.Tweening;
using TMPro;
using Unity.Cinemachine;
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
        public Button nextLevelButton;
        
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
            Sequence sequence = DOTween.Sequence();
            sequence.Append(header.DOFade(1, headerFadeInDuration));
            sequence.Append(header.rectTransform.DOSizeDelta(new Vector2(1920 , headerMaxSize) , headerSizeUpDuration).SetEase(Ease.OutBack));
            sequence.SetDelay(0.2f);
            sequence.Append(clearText.DOFade(1,clearTextFadeInDuration));
            sequence.SetDelay(1.1f);
            sequence.AppendCallback(()=>nextLevelButton.gameObject.SetActive(true));
            sequence.Append(nextLevelButton.transform.DOScaleY(1 , 0.4f));
            
        }
        private void ResetClearPanel()
        {
            header.gameObject.SetActive(false);
            header.DOFade(0, 0);
            header.rectTransform.sizeDelta = new Vector2(1920, 4);

            clearText.DOFade(0, 0);
    
            nextLevelButton.gameObject.SetActive(false);
            nextLevelButton.transform.localScale = new Vector3(1, 0, 1);
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
        
        public void NextOtherScene(string sceneName)
        {
            StartFade(sceneName, () => {}); 
        }
    }
}
