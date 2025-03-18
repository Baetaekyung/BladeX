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
            coinText.text = "0 코인";
            //SetGoldUI();
        }
        
        private void OnDestroy()
        {
            sceneManagerSo.LevelClearEvent -= AddRandomGold;
        }
                
        private void SetGoldUI()
        {
            coinText.text = $"{InventoryManager.Inventory.Coin.ToString()} 코인";
        }
        
        public void Execute()
        {
            SetGoldUI();
        }

        private void AddRandomGold()
        {
            int randomGold = Random.Range(50, 100);
            InventoryManager.Inventory.Coin += randomGold;
            
            SetGoldUI();
        }
        
    }
}
