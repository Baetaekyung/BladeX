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
        
        public void SetText(string text)
        {
            _text.text = text;
        }
        
        public override void Popup()
        {
            if(transform != null)
            {
                transform.DOScaleX(1, fadeTime)
                    .SetEase(Ease.OutCirc)
                    .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
            }
        }

        public override void PopDown()
        {
            if(transform != null)
            {
                transform.DOScaleX(0, fadeTime)
                    .SetEase(Ease.OutCirc)
                    .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
            }
        }
    }
}
