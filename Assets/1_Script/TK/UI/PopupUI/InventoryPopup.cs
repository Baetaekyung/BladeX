using DG.Tweening;
using Swift_Blade.UI;

namespace Swift_Blade
{
    public class InventoryPopup : PopupUI
    {
        public override void Popup()
        {
            cG.DOFade(1, fadeTime)
                .SetEase(Ease.OutCirc)
                .SetLink(gameObject, LinkBehaviour.KillOnDestroy);

            _raycaster.enabled = true;
        }

        public override void PopDown()
        {
            cG.DOFade(0, fadeTime).SetEase(Ease.InCirc)
                .SetLink(gameObject, LinkBehaviour.KillOnDestroy);

            _raycaster.enabled = false;
        }

        private void OnDisable()
        {
            InventoryManager.Instance.UpdateAllSlots();
        }
    }
}
