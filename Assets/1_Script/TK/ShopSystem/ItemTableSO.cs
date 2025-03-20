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
            ItemTableSO table = Instantiate(this);

            return table;
        }

        public List<ItemSet> GetRandomItemTable()
        {
            ItemTableSO tableSo = Clone();
            List<ItemSet> randomTable = new List<ItemSet>();
            
            int current = 0;
            
            while (shopItemCount > current)
            {
                var index = Random.Range(0, tableSo.itemTable.Count);
                
                randomTable.Add(tableSo.itemTable[index]);
                tableSo.itemTable.RemoveAt(index);
                
                current++;
            }

            return randomTable;
        }
    }
}
