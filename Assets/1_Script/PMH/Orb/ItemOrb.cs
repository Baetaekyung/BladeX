using DG.Tweening;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Swift_Blade
{
    public class ItemOrb : BaseOrb
    {
        [SerializeField] private ItemTableSO itemTables;
        protected override TweenCallback CreateDefaultCallback()
        {
            return () =>
            {
                int n = Random.Range(0, itemTables.itemTable.Count);
                ItemDataSO ItemData = itemTables.itemTable[n].itemData;

                InventoryManager.Instance.AddItemToEmptySlot(ItemData);
                Destroy(gameObject);
            };
        }
    }
}
