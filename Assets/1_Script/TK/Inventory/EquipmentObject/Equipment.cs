using Swift_Blade.Combat.Health;
using UnityEngine;

namespace Swift_Blade
{
    public abstract class Equipment : ItemObject
    {
        private const int RARITY_STACK_DIVIDER = 2;

        [SerializeField]
        protected EquipmentData equipData;
        protected PlayerStatCompo _playerStat;
        protected PlayerVisualController _playerVisualController;
        private static int rarityModifierStack;
        private static int stackData;

        public virtual void OnEquipment(bool withoutStat = false)
        {
            HandleStatAdder(withoutStat);
            OnEquipParts();
        }

        public virtual void OffEquipment()
        {
            OffEquipParts();
            HandleStatRemover();
        }

        public void HandleStatAdder(bool withoutStat = false)
        {
            _playerStat = Player.Instance.GetEntityComponent<PlayerStatCompo>();

            if (!_playerStat)
                return;

            //Remove stat add
            //foreach (var stat in equipData.statModifier)
            //{
            //    _playerStat.AddModifier(
            //        stat.Key,
            //        equipData.itemSerialCode,
            //        stat.Value);

            //    Player.Instance.GetEntityComponent<PlayerHealth>().HealthUpdate();
            //}

            foreach (var tag in equipData.tags)
            {
                var tagCompo = Player.Instance.GetEntityComponent<PlayerTagCompo>();

                if(tagCompo != null)
                {
                    tagCompo.AddTagCount(tag);
                }
                else
                    Debug.Log("Please add component to player, CompoName: PlayerTagCompo");
            }

            if (withoutStat)
                return;

            _playerStat.IncreaseColorValue(equipData.colorType,
                equipData.colorAdder);

            rarityModifierStack += GetRarityColorValue();
            CalculateRarityModStack();
        }

        public void HandleStatRemover()
        {
            _playerStat = Player.Instance?.GetEntityComponent<PlayerStatCompo>();

            if (_playerStat == null)
                return;

            //foreach (var stat in equipData.statModifier)
            //{
            //    //Key is StatType
            //    _playerStat.RemoveModifier(stat.Key, equipData.itemSerialCode);

            //    if (stat.Key == StatType.HEALTH)
            //    {
            //        var playerHealth = Player.Instance.GetEntityComponent<PlayerHealth>();
            //        //PlayerHealth.CurrentHealth -= Mathf.RoundToInt(stat.Value);

            //        playerHealth?.HealthUpdate();
            //    }
            //}

            foreach (var tag in equipData.tags)
            {
                Player.Instance.GetEntityComponent<PlayerTagCompo>().RemoveTagCount(tag);
            }

            _playerStat.DecreaseColorValue(equipData.colorType,
                equipData.colorAdder);

            rarityModifierStack -= GetRarityColorValue();
            CalculateRarityModStack();
        }

        private void CalculateRarityModStack()
        {
            // 초기화 하고 다시 적용
            _playerStat.DecreaseColorValue(equipData.colorType, stackData);

            // 스택된 값 다시 계산
            stackData = rarityModifierStack / RARITY_STACK_DIVIDER;
            if (stackData >= 1)
            {
                _playerStat.IncreaseColorValue(equipData.colorType,
                    stackData);
            }
        }

        private void OnEquipParts()
        {
            _playerVisualController = Player.Instance?.GetEntityComponent<PlayerVisualController>();
            _playerVisualController.OnParts(equipData.GetPartsName);
        }
        private void OffEquipParts()
        {
            _playerVisualController = Player.Instance?.GetEntityComponent<PlayerVisualController>();
            _playerVisualController.OffParts(equipData.GetPartsName);
        }

        private int GetRarityColorValue()
        {
            int rarityColor = equipData.rarity switch
            {
                EquipmentRarity.NONE => rarityColor = 0, //default modi
                EquipmentRarity.COMMON => rarityColor = 1,
                EquipmentRarity.RARE => rarityColor = 2,
                EquipmentRarity.UNIQUE => rarityColor = 3,
                EquipmentRarity.EPIC => rarityColor = 4,
                EquipmentRarity.END => rarityColor = 0, //default modi
                _ => 0
            };

            return rarityColor;
        }
    }
}
