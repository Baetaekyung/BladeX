using System;
using DG.Tweening;
using UnityEngine;

namespace Swift_Blade
{
    public class InGameUIManager : MonoSingleton<InGameUIManager>
    {
        [field: SerializeField] public CanvasGroup BossHealthBarUI { get; private set; }

        private void Start()
        {
            EnableBossHealthBar(false);
        }

        public void EnableBossHealthBar(bool enable)
        {
            if (enable)
            {
                BossHealthBarUI.alpha = 0;
                BossHealthBarUI.gameObject.SetActive(true);
                BossHealthBarUI.DOFade(1, 0.4f);
            }
            else
            {
                BossHealthBarUI.DOFade(0, 0.4f)
                    .OnComplete(() => BossHealthBarUI.gameObject.SetActive(false));
            }
        }
    }
}
