using Swift_Blade.UI;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System;

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
            Popup(null);
        }

        public void Popup(float time, Action callback)
        {
            if (rectTrans != null)
            {
                rectTrans.DOKill();

                rectTrans.DOLocalMoveY(maxPosY, time)
                    .OnComplete(() => callback?.Invoke())
                    .SetEase(Ease.OutCirc)
                    .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
            }

            _raycaster.enabled = true;
        }

        public void Popup(Action callback)
        {
            Popup(upDuration, callback);
        }

        public override void PopDown()
        {
            PopDown(null);
        }

        public void PopDown(Action callback)
        {
            PopDown(fadeTime, callback);
        }

        public void PopDown(float timer, Action callback = null)
        {
            if (rectTrans != null)
                rectTrans.DOKill();

            if (cG != null)
            {
                cG.DOFade(0f, timer).SetEase(Ease.InCirc).OnComplete(() =>
                {
                    rectTrans.localPosition = new Vector3(
                    rectTrans.localPosition.x,
                    minPosY,
                    rectTrans.localPosition.z);

                    callback?.Invoke();
                }).SetLink(gameObject, LinkBehaviour.KillOnDestroy);
            }

            _raycaster.enabled = false;
        }

        public void SetInfoBox(string message)
        {
            cG.alpha = 1.0f;
            infoText.text = message;
        }
    }
}
