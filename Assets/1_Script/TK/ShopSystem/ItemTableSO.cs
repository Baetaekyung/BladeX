using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Swift_Blade
{
    [Serializable]
    public struct ItemGoods
    {
        public ItemDataSO itemData;

        public int itemCount;
        public int itemCost;

        public ItemGoods(ItemDataSO itemData, int itemCount, int itemCost)
        {
            this.itemData = itemData;
            this.itemCount = itemCount; 
            this.itemCost = itemCost;
        }
    }
    
    [CreateAssetMenu(fileName = "ItemTableSO", menuName = "SO/Item/Table")]
    public class ItemTableSO : ScriptableObject
    {
        public List<ItemGoods> itemTable = new List<ItemGoods>();

        public ItemTableSO GetClonedItemTable()
        {
            ItemTableSO table = Instantiate(this);

            return table;
        }
    }
}
