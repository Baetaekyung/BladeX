using System;
using DG.Tweening;
using Swift_Blade.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Swift_Blade
{
    public class InventoryPopup : PopupUI
    {
        public override void Popup()
        {
            cG.alpha = 1f;
            cG.transform.DOScaleX(1, _fadeTime).SetEase(Ease.OutCirc);
            _raycaster.enabled = true;
        }

        public override void PopDown()
        {
            cG.transform.DOScaleX(0, _fadeTime).SetEase(Ease.OutCirc);
            _raycaster.enabled = false;
        }

        private void OnDisable()
        {
            InventoryManager.Instance.UpdateAllSlots();
        }
    }
}
