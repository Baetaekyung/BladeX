using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Swift_Blade.UI
{
    public class SettingPopup : PopupUI
    {
        public override void Popup()
        {
            cG.alpha = 1f;
            cG.transform.DOScaleX(1, fadeTime).SetEase(Ease.OutCirc);
            _raycaster.enabled = true;
        }

        public override void PopDown()
        {
            cG.transform.DOScaleX(0, fadeTime).SetEase(Ease.OutCirc);
            _raycaster.enabled = false;
        }
    }
}
