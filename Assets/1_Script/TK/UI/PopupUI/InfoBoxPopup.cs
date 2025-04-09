using Swift_Blade.UI;
using UnityEngine;
using DG.Tweening;
using TMPro;

namespace Swift_Blade
{
    public class InfoBoxPopup : PopupUI
    {
        [SerializeField] private float minPosY;
        [SerializeField] private float maxPosY;

        [SerializeField] [Range(0.1f, 1)] private float upDuration;
        
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
                
                rectTrans.DOLocalMoveY(maxPosY, upDuration)
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
                cG.DOFade(0f, fadeTime).SetEase(Ease.InCirc).OnComplete(() =>
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
