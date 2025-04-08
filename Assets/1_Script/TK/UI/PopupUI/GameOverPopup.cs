using DG.Tweening;
using Swift_Blade.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Swift_Blade
{
    public class GameOverPopup : PopupUI
    {
        [SerializeField] private float          fadeInTime;
        [SerializeField] private float          fadeOutTime;
        [SerializeField] private SceneManagerSO sceneManager;
        [SerializeField] private Button         restartButton;
        [SerializeField] private Button         titleButton;

        private CanvasGroup _restartButtonCG;
        private CanvasGroup _titleButtonCG;

        protected override void Awake()
        {
            base.Awake();

            _restartButtonCG = restartButton.GetComponent<CanvasGroup>();
            _titleButtonCG = titleButton.GetComponent<CanvasGroup>();
        }

        public override void Popup()
        {
            cG.DOFade(1, fadeInTime)
                .SetEase(Ease.OutSine)
                .OnComplete(() => HandleButtonActive());
        }

        private void HandleButtonActive()
        {
            _restartButtonCG.DOFade(1f, 1f);
            _titleButtonCG.DOFade(1f, 1f).OnComplete(() => _raycaster.enabled = true);
        }

        public override void PopDown()
        {
            _raycaster.enabled = false;

            _restartButtonCG.alpha = 0f;
            _titleButtonCG.alpha = 0f;

            cG.DOFade(0, fadeOutTime)
                .SetEase(Ease.OutSine);
        }

        public void GoToTitle()
        {
            sceneManager.LoadScene("Title");
        }

        public void Resume()
        {
            cG.DOKill();
            sceneManager.LoadScene("Menu");
        }
    }
}
