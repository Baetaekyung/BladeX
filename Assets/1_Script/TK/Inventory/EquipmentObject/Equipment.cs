using Swift_Blade.Combat.Health;
using UnityEngine;

namespace Swift_Blade
{
    public abstract class Equipment : ItemObject
    {
        [SerializeField]
        protected EquipmentData equipData;
        protected PlayerStatCompo _playerStat;
        protected PlayerVisualController _playerVisualController;

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
            _playerStat = Player.Instance.GetEntityComponent<PlayerStatCompo>();

            if (!_playerStat)
                return;

            //Stat add part
            foreach (var stat in equipData.statModifier)
            {
                _playerStat.AddModifier(
                    stat.Key,
                    equipData.itemSerialCode,
                    stat.Value);

                Player.Instance.GetEntityComponent<PlayerHealth>().HealthUpdate();
            }

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

            _playerStat.IncreaseColorValue(equipData.colorType,
                equipData.colorAdder + GetRarityColorValue());
        }

        public void HandleStatRemover()
        {
            _playerStat = Player.Instance?.GetEntityComponent<PlayerStatCompo>();

            if (_playerStat == null)
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

            _playerStat.DecreaseColorValue(equipData.colorType,
                equipData.colorAdder + GetRarityColorValue());
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
                EquipmentRarity.COMMON => rarityColor = 0,
                EquipmentRarity.RARE => rarityColor = 0,
                EquipmentRarity.UNIQUE => rarityColor = 1,
                EquipmentRarity.EPIC => rarityColor = 2,
                EquipmentRarity.END => rarityColor = 0, //default modi
                _ => 0
            };

            return rarityColor;
        }
    }
}
