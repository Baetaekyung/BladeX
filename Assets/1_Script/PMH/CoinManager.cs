using TMPro;
using UnityEngine;

namespace Swift_Blade
{
    public class CoinManager : MonoBehaviour
    {
        public static CoinManager Instance;

        [SerializeField] private int coinValue = 300;

        private int missCount = 50;

        [SerializeField] private TMP_Text coinText;

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(Instance);
            }
        }
        private void Update()
        {
            coinText.text = coinValue + " Coin";
        }

        public void DiscountCoin()
        {
            coinValue -= missCount;
        }
    }
}
