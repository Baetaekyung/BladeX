using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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
        public int shopItemCount;

        public ItemTableSO Clone()
        {
            ItemTableSO itemTable = Instantiate(this);

            return itemTable;
        }

        public List<ItemSet> GetRandomItemTable()
        {
            int current = 0;

            List<ItemSet> randomTable = new List<ItemSet>();
            
            while (shopItemCount > current)
            {
                var index = Random.Range(0, itemTable.Count);
                
                randomTable.Add(itemTable[index]);
                itemTable.RemoveAt(index);
                
                current++;
            }

            return randomTable;
        }
    }
}
