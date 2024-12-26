using UnityEngine;
using UnityEngine.UI;

namespace Swift_Blade.UI
{
    public class BossGroggyUI : MonoBehaviour
    {
        [SerializeField] private Image _bossGroggyFillAmount;

        /// <param name="normalizedGroggy">현재 체력 / 최대 체력 넣기</param>
        public void SetFillAmount(float normalizedGroggy)
        {
            _bossGroggyFillAmount.fillAmount = normalizedGroggy;
        }
    }
}
