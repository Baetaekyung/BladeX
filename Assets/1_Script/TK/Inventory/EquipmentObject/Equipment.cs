using Swift_Blade.Combat.Health;
using UnityEngine;

namespace Swift_Blade
{
    public abstract class Equipment : ItemObject
    {
        private const float COMMON_MODIFIER = 1f;
        private const float RARE_MODIFIER   = 1.15f;
        private const float UNIQUE_MODIFIER = 1.3f;
        private const float EPIC_MODIFIER   = 1.5f;

        [SerializeField]
        protected EquipmentData equipData;
        protected PlayerStatCompo _playerStat;
        protected PlayerVisualController _playerVisualController;
        //플레이어외형을결정하는오브젝트

        public virtual void OnEquipment()
        {
            HandleStatAdder();
            OnEquipParts();
        }

        public virtual void OffEquipment()
        {
            OffEquipParts();
            HandleStatRemover();
        }

        public void HandleStatAdder()
        {
            _playerStat = Player.Instance?.GetEntityComponent<PlayerStatCompo>();

            if (!_playerStat)
                return;

            if (equipData.statModifier.Count == 0)
                return;

            //Stat add part
            foreach (var stat in equipData.statModifier)
            {
                float rarityModifier = equipData.rarity switch
                {
                    EquipmentRarity.NONE   => rarityModifier = 1f, //default modi
                    EquipmentRarity.COMMON => rarityModifier = COMMON_MODIFIER,
                    EquipmentRarity.RARE   => rarityModifier = RARE_MODIFIER,
                    EquipmentRarity.UNIQUE => rarityModifier = UNIQUE_MODIFIER,
                    EquipmentRarity.EPIC   => rarityModifier = EPIC_MODIFIER,
                    EquipmentRarity.END    => rarityModifier = 1f, //default modi
                    _ => throw new System.Exception($"Not exist enum value, Enum name {equipData.rarity}"),
                };

                if (stat.Key == StatType.HEALTH)
                    rarityModifier = 1f;

                _playerStat.AddModifier(
                    stat.Key,
                    equipData.itemSerialCode,
                    stat.Value * rarityModifier);

                Player.Instance.GetEntityComponent<PlayerHealth>().HealthUpdate();
            }

            foreach (var tag in equipData.tags)
            {
                Player.Instance.GetEntityComponent<PlayerTagCompo>().AddTagCount(tag);
            }

            _playerStat.IncreaseColorValue(equipData.colorType, equipData.colorAdder);
        }

        public void HandleStatRemover()
        {
            _playerStat = Player.Instance?.GetEntityComponent<PlayerStatCompo>();

            if (_playerStat == null)
                return;

            if (equipData.statModifier.Count == 0) //is not upgrade stats
                return;

            foreach (var stat in equipData.statModifier)
            {
                //Key is StatType
                _playerStat.RemoveModifier(stat.Key, equipData.itemSerialCode);

                if (stat.Key == StatType.HEALTH)
                {
                    var playerHealth = Player.Instance.GetEntityComponent<PlayerHealth>();
                    //PlayerHealth.CurrentHealth -= Mathf.RoundToInt(stat.Value);

                    playerHealth?.HealthUpdate();
                }
            }

            foreach(var tag in equipData.tags)
            {
                Player.Instance.GetEntityComponent<PlayerTagCompo>().RemoveTagCount(tag);
            }

            _playerStat.DecreaseColorValue(equipData.colorType, equipData.colorAdder);
        }

        private void OnEquipParts()
        {
            _playerVisualController = Player.Instance?.GetEntityComponent<PlayerVisualController>();
            _playerVisualController.OnParts(equipData.partsName);
        }
        private void OffEquipParts()
        {
            _playerVisualController = Player.Instance?.GetEntityComponent<PlayerVisualController>();
            _playerVisualController.OffParts(equipData.partsName);
        }
    }
}
