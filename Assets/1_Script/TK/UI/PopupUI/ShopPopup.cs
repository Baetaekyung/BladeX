using DG.Tweening;
using Swift_Blade.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Swift_Blade
{
    public class ShopPopup : PopupUI
    {
        public override void Popup()
        {
            cG.alpha = 1f;
            cG.transform.DOScaleX(1, fadeTime)
                .SetEase(Ease.OutCirc);

            _raycaster.enabled = true;
        }

        public override void PopDown()
        {
            cG.transform.DOScaleX(0, fadeTime)
                .SetEase(Ease.OutCirc)
                .OnComplete(() => InventoryManager.Instance.UpdateAllSlots());

            _raycaster.enabled = false;
        }
    }
}
