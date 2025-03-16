using TMPro;
using UnityEngine;

namespace Swift_Blade
{
    public class CoinManager : MonoBehaviour
    {
        public static CoinManager Instance;

        [field : SerializeField] public int coinValue = 0;

        private int missCount = 50;

        [SerializeField] private TMP_Text coinText;

        [SerializeField] private PlayerInventory invenSO;

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

        public void DiscountCoin()
        {
            coinValue -= missCount;
            coinText.text = coinValue + " Coin";
        }
        public void AddedCountCoin(int setCoinValue = 50)
        {
            coinValue += setCoinValue;
            coinText.text = coinValue + " Coin";
        }

        public void GameFinishToAddCoin()
        {
            invenSO.Coin += coinValue;
        }
    }
}
