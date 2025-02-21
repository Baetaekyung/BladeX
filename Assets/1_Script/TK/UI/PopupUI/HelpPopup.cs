using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Swift_Blade.UI
{
    public class HelpPopup : PopupUI
    {
        //todo : Áßº¹µÅ´Â°Å¤±¤±
        [SerializeField] private CanvasGroup _cG;
        [SerializeField] private float _fadeTime;
        private GraphicRaycaster _raycaster;

        private void Awake()
        {
            _raycaster = GetComponent<GraphicRaycaster>();
        }

        public override void Popup()
        {
            _cG.DOFade(1, _fadeTime)
                .SetEase(Ease.OutCirc);
            _raycaster.enabled = true;
        }

        public override void PopDown()
        {
            _raycaster.enabled = false;
            _cG.DOFade(0, _fadeTime)
                .SetEase(Ease.OutCirc);
        }
    }
}
