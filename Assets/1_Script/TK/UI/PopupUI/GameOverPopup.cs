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
        
        public override void Popup()
        {
            cG.DOFade(1, fadeInTime)
                .SetEase(Ease.OutSine)
                .OnComplete(() => _raycaster.enabled = true);
        }

        public override void PopDown()
        {
            _raycaster.enabled = false;

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
