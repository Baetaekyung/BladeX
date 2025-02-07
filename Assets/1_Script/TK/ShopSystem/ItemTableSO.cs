using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "ItemTableSO", menuName = "SO/Item/Table")]
    public class ItemTableSO : ScriptableObject
    {
        public List<ItemDataSO> itemTable;

        public ItemDataSO GetItemData(string itemName)
        {
            ItemDataSO item = itemTable.FirstOrDefault(i => i.name == itemName);

            if (item == null)
            {
                Debug.Log("�߸��� ������ �̸�: " + itemName);
                return null;
            }
            
            return item;
        }
        
        public ItemDataSO GetRandomItemData()
        {
            int randomIndex = Random.Range(0, itemTable.Count);

            return itemTable[randomIndex];
        }

        public ItemDataSO GetRandomItemData(ItemType type)
        {
            List<ItemDataSO> typedItems = itemTable.FindAll(item => item.itemType == type);
            
            if(typedItems.Count == 0)
            {
                Debug.Log("�� Ÿ���� �������� ���̺� �������� ����");
                return null;
            }

            int randomIndex = Random.Range(0, typedItems.Count);

            return typedItems[randomIndex];
        }
    }
}
