using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

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
        public List<ItemGoods> itemTable;
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

#if UNITY_EDITOR
        public void CollectAssets()
        {
            itemTable.Clear();

            string[] guids = AssetDatabase.FindAssets("t:ItemDataSO");
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                ItemDataSO item = AssetDatabase.LoadAssetAtPath<ItemDataSO>(path);

                int cost = item.IsEquipment() ? 2 : 1;
                int count = item.IsEquipment() ? 1 : 2;
                ItemGoods toGoods = new ItemGoods(item, count, cost);
                if (item != null)
                {
                    itemTable.Add(toGoods);
                }
            }

            Debug.Log($"Found {itemTable.Count} ItemDataSO assets.");
            EditorUtility.SetDirty(this); // 변경사항 저장
        }
#endif
    }
}
