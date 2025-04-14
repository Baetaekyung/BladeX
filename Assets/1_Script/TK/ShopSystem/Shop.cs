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
        private List<ShopSlotUI> shopSlots = new List<ShopSlotUI>();

        [SerializeField] private ShopSlotUI shopSlotPrefab;
        [SerializeField] private Transform  parent;

        public void SetItems(ItemTableSO itemTable, int itemCount)
        {
            ItemTableSO randomItemTable = itemTable.GetClonedItemTable();

            if (shopSlots.Count > 0)
                DeleteRemainSlot();
            
            for (int i = 0; i < itemCount; i++)
            {
                int index = UnityEngine.Random.Range(0, randomItemTable.itemTable.Count);

                ItemGoods currentItem = randomItemTable.itemTable[index];
                ShopSlotUI shopSlot = Instantiate(shopSlotPrefab, parent);
                shopSlot.GetCanvasGroup.DOFade(1, 1.5f);
                
                shopSlot.SetSlotItem(currentItem.itemData, 
                    currentItem.itemCount, currentItem.itemCost);

                randomItemTable.itemTable.RemoveAt(index);
                shopSlots.Add(shopSlot);
            }
        }

        private void DeleteRemainSlot()
        {
            foreach (var slot in shopSlots)
            {
                slot.GetCanvasGroup.alpha = 0;
                Destroy(slot.gameObject);
            }
            
            shopSlots.Clear();
        }
    }
}
