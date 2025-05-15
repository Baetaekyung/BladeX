using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Swift_Blade
{
    [Serializable]
    public struct ItemGoods
    {
        public ItemDataSO itemData;

        public int itemCount;
        public int needHealth;

        public ItemGoods(ItemDataSO itemData, int itemCount, int itemCost)
        {
            this.itemData = itemData;
            this.itemCount = itemCount; 
            this.needHealth = itemCost;
        }
    }
    
    [CreateAssetMenu(fileName = "ItemTableSO", menuName = "SO/Item/Table")]
    public class ItemTableSO : ScriptableObject
    {
        public List<ItemGoods> itemTable = new List<ItemGoods>();
        public List<ItemDataSO> ToItemDataSOList()
        {
            List<ItemDataSO> result = new List<ItemDataSO>(itemTable.Select(goods => goods.itemData));
            return result;
        }

        public ItemTableSO GetClonedItemTable()
        {
            ItemTableSO table = Instantiate(this);

            return table;
        }
    }
}
