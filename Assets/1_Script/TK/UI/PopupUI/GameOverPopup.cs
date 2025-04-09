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
            if(cG != null)
            {
                cG.DOFade(1, fadeInTime)
                    .SetEase(Ease.OutSine)
                    .OnComplete(() => HandleButtonActive())
                    .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
            }
        }

        private void HandleButtonActive()
        {
            if(_restartButtonCG != null)
                _restartButtonCG.DOFade(1f, 1f)
                    .SetLink(gameObject, LinkBehaviour.KillOnDestroy);

            if(_titleButtonCG != null)
                _titleButtonCG.DOFade(1f, 1f)
                    .OnComplete(() => _raycaster.enabled = true)
                    .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
        }

        public override void PopDown()
        {
            _raycaster.enabled = false;

            _restartButtonCG.alpha = 0f;
            _titleButtonCG.alpha = 0f;

            if(cG != null)
            {
                cG.DOFade(0, fadeOutTime)
                    .SetEase(Ease.OutSine)
                    .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
            }
        }

        public void GoToTitle()
        {
            sceneManager.LoadScene("Title");
        }

        public void Resume()
        {
            if(cG != null)
                cG.DOKill();

            sceneManager.LoadScene("Menu");
        }
    }
}
