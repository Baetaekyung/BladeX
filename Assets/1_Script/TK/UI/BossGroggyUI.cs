using UnityEngine;
using UnityEngine.UI;

namespace Swift_Blade.UI
{
    public class BossGroggyUI : MonoBehaviour
    {
        [SerializeField] private Image _bossGroggyFillAmount;

        /// <param name="normalizedGroggy"> input currentGroggy / maxGroggy</param>
        public void SetFillAmount(float normalizedGroggy)
        {
            _bossGroggyFillAmount.fillAmount = normalizedGroggy;
        }
    }
}
