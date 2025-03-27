using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Swift_Blade
{
    [Serializable]
    public class ItemGoods
    {
        public ItemDataSO itemData;

        public int itemCount;
        public int itemCost;
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

        public List<ItemGoods> GetRandomItemTable(int count)
        {
            ItemTableSO tableSo = GetClonedItemTable();
            List<ItemGoods> randomTable = new List<ItemGoods>();
            
            int current = 0;
            
            while (count > current)
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
