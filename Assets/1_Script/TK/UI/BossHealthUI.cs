using UnityEngine;
using UnityEngine.UI;

namespace Swift_Blade.UI
{
    public class BossHealthUI : MonoBehaviour
    {
        [SerializeField] private Image _bossHealthFillAmount;

        /// <param name="normalizedHealth"> input currentHealth / maxHealth</param>
        public void SetFillAmount(float normalizedHealth)
        {
            _bossHealthFillAmount.fillAmount = normalizedHealth;
        }
    }
}
