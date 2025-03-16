using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Swift_Blade
{
    public class GoldUI : MonoBehaviour
    {
        [SerializeField] private PlayerInventory playerInventory;
        [SerializeField] private TextMeshProUGUI coinText;

        private void Start()
        {
            coinText.text = $"{playerInventory.Coin.ToString()} 코인";
        }

        public void SetGoldUI()
        {
            coinText.text = $"{playerInventory.Coin.ToString()} 코인";
        }
    }
}
