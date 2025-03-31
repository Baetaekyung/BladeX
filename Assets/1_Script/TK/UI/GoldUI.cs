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
                coinText.text = $"{InventoryManager.Inventory.Coin.ToString()} ����";
            else
            {
                coinText.text = "0 ����";
            }
        }
        
        private void AddRandomGold()
        {
            SetGoldUI();
        }
    }
}
