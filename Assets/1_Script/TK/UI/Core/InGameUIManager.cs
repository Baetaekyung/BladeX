using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace Swift_Blade
{
    public class InGameUIManager : MonoSingleton<InGameUIManager>
    {
        public GameObject bossHealthBarUI;

        [SerializeField] private SceneManagerSO sceneManagerSo;
        
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
    }
}
