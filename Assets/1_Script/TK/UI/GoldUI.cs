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
        
        public void Execute()
        {
            SetGoldUI();
        }
        
        private void SetGoldUI()
        {
            coinText.text = $"{InventoryManager.Inventory.Coin.ToString()} ÄÚÀÎ";
        }
        
        private void AddRandomGold()
        {
            int randomGold = Random.Range(50, 100);
            InventoryManager.Inventory.Coin += randomGold;
            
            SetGoldUI();
        }
        
    }
}
