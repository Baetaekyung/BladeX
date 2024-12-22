using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Swift_Blade.UI
{
    public class PlayerHealthUI : MonoBehaviour
    {
        [SerializeField] private Image _healthFillAmount;
        
        /// <param name="normalizedHealth">현재 체력 / 최대 체력 넣기</param>
        public void SetHealthFillAmount(int normalizedHealth)
        {
            _healthFillAmount.fillAmount = normalizedHealth;
        }
    }
}
