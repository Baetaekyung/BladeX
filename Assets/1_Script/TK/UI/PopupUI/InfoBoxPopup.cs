using DG.Tweening;
using Swift_Blade.UI;
using System.Text;
using TMPro;
using UnityEngine;

namespace Swift_Blade
{
    public class InfoBoxPopup : PopupUI
    {
        [SerializeField] private float minPosY;
        [SerializeField] private float maxPosY;

        [Header("Info")]
        [SerializeField] private TextMeshProUGUI infoText;

        private RectTransform rectTrans;

        protected override void Awake()
        {
            base.Awake();

            rectTrans = GetComponent<RectTransform>();
        }

        public override void Popup()
        {
            if(rectTrans != null)
            {
                rectTrans.DOKill();

                rectTrans.DOLocalMoveY(maxPosY, fadeTime)
                    .SetEase(Ease.OutCirc)
                    .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
            }

            _raycaster.enabled = true;
        }

        public override void PopDown()
        {
            if(rectTrans != null)
                rectTrans.DOKill();

            if(cG != null)
            {
                cG.DOFade(0f, 0.5f).SetEase(Ease.InCirc).OnComplete(() =>
                {
                    rectTrans.localPosition = new Vector3(
                    rectTrans.localPosition.x,
                    minPosY,
                    rectTrans.localPosition.z);
                }).SetLink(gameObject, LinkBehaviour.KillOnDestroy);
            }

            PopupManager.Instance.InfoBoxRemain = false;
            _raycaster.enabled = false;
        }

        public void SetInfoBox(string message)
        {
            cG.alpha = 1.0f;
            infoText.text = message;
        }
    }
}
