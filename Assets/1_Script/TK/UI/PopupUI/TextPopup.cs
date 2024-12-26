using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Swift_Blade.UI
{
    public class TextPopup : PopupUI
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private CanvasGroup _cG;
        [SerializeField] private float _fadeTime;

        public void SetText(string text)
        {
            _text.text = text;
        }
        
        public override void Popup()
        {
            transform.DOScaleX(1, _fadeTime)
                .SetEase(Ease.InCirc);
        }

        public override void PopDown()
        {
            transform.DOScaleX(0, _fadeTime)
                .SetEase(Ease.OutCirc);
        }
    }
}
