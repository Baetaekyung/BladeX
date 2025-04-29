using DG.Tweening;
using UnityEngine;

namespace Swift_Blade.UI
{
    public class SettingPopup : PopupUI
    {
        [SerializeField] private GameObject normalSettingPanel;
        [SerializeField] private GameObject keymapSettingPanel;

        protected override void Awake()
        {
            base.Awake();

            SetToNormalPanel();
        }

        public override void Popup()
        {
            if(cG != null)
            {
                cG.alpha = 1f;
                cG.transform
                    .DOScaleX(1, fadeTime)
                    .SetEase(Ease.OutCirc)
                    .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
            }
            _raycaster.enabled = true;
        }

        public override void PopDown()
        {
            if(cG != null)
            {
                cG.transform
                    .DOScaleX(0, fadeTime)
                    .SetEase(Ease.OutCirc)
                    .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
            }
            _raycaster.enabled = false;
        }

        public void SetToNormalPanel()
        {
            normalSettingPanel.SetActive(true);
            keymapSettingPanel.SetActive(false);
        }

        public void SetToKeymapPanel()
        {
            normalSettingPanel.SetActive(false);
            keymapSettingPanel.SetActive(true);
        }
    }
}
