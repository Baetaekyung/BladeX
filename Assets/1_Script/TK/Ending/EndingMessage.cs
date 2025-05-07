using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Swift_Blade
{
    public class EndingMessage : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI creditMessage;

        private RectTransform _rtrm;

        private void OnEnable()
        {
            _rtrm = transform as RectTransform;
        }

        private void Start()
        {
            _rtrm.DOAnchorPosY(1500f, 10f)
                .OnComplete(() => Destroy(this.gameObject))
                .SetLink(gameObject, LinkBehaviour.KillOnDestroy)
                .SetEase(Ease.Linear);
        }

        public void SetText(string message)
        {
            creditMessage.text = message;
        }
    }
}
