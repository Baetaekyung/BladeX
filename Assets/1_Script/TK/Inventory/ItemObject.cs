using UnityEngine;

namespace Swift_Blade
{
    public class ItemObject : MonoBehaviour //Prefab
    {
        [SerializeField] private ItemDataSO itemData;
        public ItemDataSO GetItemData => itemData;
    }
}
