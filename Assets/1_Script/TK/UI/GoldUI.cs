using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Swift_Blade
{
    public class GoldUI : MonoBehaviour
    {
        [SerializeField] private InventoryManager playerInventory;
        [SerializeField] private TextMeshProUGUI coinText;
        [SerializeField] private SceneManagerSO sceneManagerSo;
        
        private void Start()
        {
            sceneManagerSo.LevelClearEvent += AddRandomGold;
            
            SetGoldUI();
        }
        
        private void OnDestroy()
        {
            sceneManagerSo.LevelClearEvent -= AddRandomGold;
        }
        
        private void SetGoldUI()
        {
            if(InventoryManager.Inventory != null)                        
                coinText.text = $"{InventoryManager.Inventory.Coin.ToString()} 코인";
            else
            {
                coinText.text = "0 코인";
            }
        }
        
        private void AddRandomGold()
        {
            SetGoldUI();
        }
    }
}
