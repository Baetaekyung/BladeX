using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Swift_Blade
{
    public class InGameUIManager : MonoSingleton<InGameUIManager>
    {
        public GameObject bossHealthBarUI;

        [SerializeField] private SceneManagerSO sceneManagerSo;

        [Header("ItemInfoBox")]
        [SerializeField] private RectTransform itemInfoRectTransform;
        [SerializeField] private CanvasGroup itemInfoCanvasGroup;

        [SerializeField] private Image iconImageBackground;
        [SerializeField] private Image iconImage;

        [SerializeField] private TextMeshProUGUI itemInfoTitle;

        private float transparencyInfo;
        private Tween transparencyTween;
        public void EnableBoss(bool enable)
        {
            EnableBossUIs(enable);
        }
        public void EnableBossUIs(bool enable)
        {
            if (enable)
            {
                bossHealthBarUI.gameObject.SetActive(true);
                bossHealthBarUI.GetComponent<RectTransform>().DOAnchorPosY(-75, 0.7f)
                    .SetEase(Ease.OutBounce);
            }
            else
            {
                bossHealthBarUI.GetComponent<RectTransform>().DOAnchorPosY(110, 0.7f)
                    .SetEase(Ease.Linear)
                    .OnComplete(() => bossHealthBarUI.gameObject.SetActive(false));
            }
        }
        public void SetInfoBoxAlpha(float alpha, bool tweenAnimationSkip = true)
        {
            if (tweenAnimationSkip)
            {
                itemInfoCanvasGroup.alpha = alpha;
            }
            else
            {
                const float duration = 0.1f;
                if (transparencyTween != null) transparencyTween.Kill();
                transparencyTween = DOTween.To(() => 0.5f, x => itemInfoCanvasGroup.alpha = x, alpha, duration)
                    .SetEase(Ease.InSine);
            }
        }
        public void SetInfoBoxPosition(Vector3 worldPosition)
        {
            Camera playerCamera = Player.Instance.GetEntityComponent<PlayerCamera>().GetPlayerCamera;
            Vector3 screenPosition = playerCamera.WorldToScreenPoint(worldPosition, Camera.MonoOrStereoscopicEye.Mono);
            itemInfoRectTransform.transform.position = screenPosition;
        }
        public void SetInfoBox(IPlayerEquipable equipable)
        {
            Color itemColor = ColorUtils.GetColorRGBUnity(equipable.GetColor);
            iconImageBackground.color = itemColor;

            iconImage.sprite = equipable.GetSprite;
            itemInfoTitle.text = equipable.DisplayName;
        }
    }
}
