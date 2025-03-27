using System;
using UnityEditor;
using UnityEngine;

namespace Swift_Blade
{
    public abstract class BaseEquipment : ItemObject
    {
        [SerializeField]
        protected EquipmentData   equipData;
        protected PlayerStatCompo _playerStat;
        
        public virtual void OnEquipment()
        {
            HandleStatAdder();
        }
        
        public virtual void OffEquipment()
        {
            HandleStatRemover();
        }
        
        public void HandleStatAdder()
        {
            _playerStat = Player.Instance?.GetEntityComponent<PlayerStatCompo>();
            
            if (!_playerStat)
                return;

            if (equipData.statModifier.Count == 0)
                return;
            
            foreach (var stat in equipData.statModifier)
            {
                //Key is StatType, Value is ModifyValue
                _playerStat.AddModifier(
                    stat.Key, 
                    equipData.itemSerialCode,
                    stat.Value);

                if (stat.Key == StatType.HEALTH)
                {
                    var playerHealth = Player.Instance?.GetEntityComponent<PlayerHealth>();

                    playerHealth?.HealthUpdate();
                }
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
                    var playerHealth = Player.Instance?.GetEntityComponent<PlayerHealth>();
        
                    playerHealth?.HealthUpdate();
                }
            }

            _playerStat.DecreaseColorValue(equipData.colorType, equipData.colorAdder);
        }
    }
}
