using System;
using System.Collections.Generic;
using UnityEngine;

namespace Swift_Blade
{
    public class Shop : MonoBehaviour
    {
        [SerializeField] private ItemTableSO testItemTable;
        
        [SerializeField] private List<ShopSlotUI> shopSlots = new List<ShopSlotUI>();

        private void Awake()
        {
            SetItems(testItemTable);
        }

        public void SetItems(ItemTableSO itemTable)
        {
            foreach (var shopSlot in shopSlots)
            {
                shopSlot.SetSlotItem(itemTable.GetRandomItemData());
            }
        }
    }
}
