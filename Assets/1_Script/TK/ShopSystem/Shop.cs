using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace Swift_Blade
{
    public class Shop : MonoBehaviour
    {
        public List<ShopSlotUI> shopSlots = new List<ShopSlotUI>(10);
        [SerializeField] private ShopSlotUI shopSlotPrefab;
        [SerializeField] private Transform parent;

        public void SetItems(ItemTableSO itemTable)
        {
            List<ItemSet> randomItemTable = itemTable.GetRandomItemTable();

            if (shopSlots.Count != 0)
                DeleteRemainSlot();
            
            for (int i = 0; i < randomItemTable.Count; i++)
            {
                ItemSet currentItem = itemTable.itemTable[i];
                ShopSlotUI shopSlot = Instantiate(shopSlotPrefab, parent);
                shopSlot.GetCanvasGroup.alpha = 0;
                shopSlot.GetCanvasGroup.DOFade(1, 0.3f);
                
                shopSlot.SetSlotItem(currentItem.itemData, 
                    currentItem.itemCount, currentItem.itemCost);
                
                shopSlots.Add(shopSlot);
            }
        }

        private void DeleteRemainSlot()
        {
            foreach (var slot in shopSlots)
            {
                Destroy(slot.gameObject);
            }
            
            shopSlots.Clear();
        }
    }
}
