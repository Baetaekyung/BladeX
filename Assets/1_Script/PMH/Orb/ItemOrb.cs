using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Swift_Blade
{
    public class ItemOrb : BaseOrb<ItemDataSO>
    {
        [SerializeField] private ItemTableSO entireItemTable;
        private static List<ItemDataSO> cache;
        protected override IReadOnlyList<ItemDataSO> GetReadonlyList => cache ?? (cache = entireItemTable.ToItemDataSOList());
        public override IPlayerEquipable GetEquipable => defaultItem.equipmentData;

        protected override TweenCallback CollectTweenCallback()
        {
            return
                () =>
                {
                    InventoryManager.Instance.TryAddItemToEmptySlot(defaultItem);
                    Destroy(gameObject);
                };
        }
        protected override void Initialize()
        {
            if (itemRenderer != null)
            {
                itemRenderer.material = colors[(int)defaultItem.equipmentData.colorType];
            }
        }
    }
}
