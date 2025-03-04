using UnityEngine;
using UnityEngine.UI;

namespace Swift_Blade
{
    public class ExpBar : MonoBehaviour
    {
        [SerializeField] private Image gauge;

        public void SetGauge(float percent)
        {
            gauge.fillAmount = percent % 100f;
        }
    }
}
