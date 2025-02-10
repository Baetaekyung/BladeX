using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Swift_Blade
{
    [Serializable]
    public class ItemSet
    {
        public ItemDataSO itemData;
        public int itemCount;
        public int itemCost;
    }
    
    [CreateAssetMenu(fileName = "ItemTableSO", menuName = "SO/Item/Table")]
    public class ItemTableSO : ScriptableObject
    {
        public List<ItemSet> itemTable = new List<ItemSet>();
        
        public ItemSet GetItemData(string itemName)
        {
            ItemSet itemSet = itemTable.FirstOrDefault(i => i.itemData.itemName == itemName);

            if (itemSet == null)
            {
                Debug.Log("잘못된 아이템 이름: " + itemName);
                return null;
            }
            
            return itemSet;
        }
        
        public ItemSet GetRandomItemData()
        {
            int randomIndex = Random.Range(0, itemTable.Count);

            return itemTable[randomIndex];
        }

        public ItemSet GetRandomItemData(ItemType type)
        {
            List<ItemSet> typedItems = itemTable.FindAll(item => item.itemData.itemType == type);
            
            if(typedItems.Count == 0)
            {
                Debug.Log("이 타입의 아이템이 테이블에 존재하지 않음");
                return null;
            }

            int randomIndex = Random.Range(0, typedItems.Count);

            return typedItems[randomIndex];
        }
    }
}
