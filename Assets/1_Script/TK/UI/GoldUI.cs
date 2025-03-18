using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Swift_Blade
{
    public class GoldUI : MonoBehaviour,IUIExecute
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
            coinText.text = $"{playerInventory.Inventory.Coin.ToString()} ÄÚÀÎ";
        }
        
        public void Execute()
        {
            SetGoldUI();
        }

        private void AddRandomGold()
        {
            int randomGold = Random.Range(50, 100);
            playerInventory.Inventory.Coin += randomGold;
            
            SetGoldUI();
        }
        
    }
}
