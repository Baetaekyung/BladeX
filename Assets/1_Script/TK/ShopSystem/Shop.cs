using System;
using System.Collections.Generic;
using UnityEngine;

namespace Swift_Blade
{
    public class Shop : MonoBehaviour
    {
        [SerializeField] private List<ShopSlotUI> shopSlots = new List<ShopSlotUI>();
        
        public void SetItems(ItemTableSO itemTable)
        {
            for (int i = 0; i < shopSlots.Count; i++)
            {
                ItemSet currentItem = itemTable.GetRandomItemData();
                
                shopSlots[i].SetSlotItem(currentItem.itemData, 
                    currentItem.itemCount, currentItem.itemCost);
            }
        }
    }
}
