using DG.Tweening;
using DG.Tweening.Core.Easing;
using Swift_Blade.UI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Swift_Blade
{
    public class GameOverPopup : PopupUI
    {
        [SerializeField] private CanvasGroup cG;
        [SerializeField] private float fadeInTime;
        [SerializeField] private float fadeOutTime;
        private GraphicRaycaster _raycaster;
        
        private void Awake()
        {
            _raycaster = GetComponent<GraphicRaycaster>();
        }
        
        [ContextMenu("Popup")]
        public override void Popup()
        {
            cG.DOFade(1, fadeInTime)
                .SetEase(Ease.OutSine);
            _raycaster.enabled = true;
        }

        [ContextMenu("Pop Down")]
        public override void PopDown()
        {
            _raycaster.enabled = false;
            cG.DOFade(0, fadeOutTime)
                .SetEase(Ease.OutSine);
        }
    }
}
