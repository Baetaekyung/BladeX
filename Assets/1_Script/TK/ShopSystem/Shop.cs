using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

namespace Swift_Blade
{
    public class Shop : MonoBehaviour
    {
        public static List<ShopSlotUI> ShopSlots = new List<ShopSlotUI>(10);
        [SerializeField] private ShopSlotUI shopSlotPrefab;
        [SerializeField] private Transform parent;
        
        public void SetItems(ItemTableSO itemTable)
        {
            int itemCount = itemTable.itemTable.Count;

            // if (ShopSlots.Count != 0)
            //     DeleteRemainSlot();
            
            for (int i = 0; i < itemCount; i++)
            {
                ItemSet currentItem = itemTable.itemTable[i];

                ShopSlotUI shopSlot = Instantiate(shopSlotPrefab, parent);
                
                shopSlot.SetSlotItem(currentItem.itemData, 
                    currentItem.itemCount, currentItem.itemCost);
                
                ShopSlots.Add(shopSlot);
            }
        }

        private void DeleteRemainSlot()
        {
            foreach (var slot in ShopSlots)
            {
                Destroy(slot.gameObject);
            }
            
            ShopSlots.Clear();
        }
    }
}
