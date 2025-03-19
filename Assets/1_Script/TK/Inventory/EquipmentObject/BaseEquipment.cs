using System;
using UnityEditor;
using UnityEngine;

namespace Swift_Blade
{
    public abstract class BaseEquipment : ItemObject
    {
        [SerializeField] protected EquipmentData equipData;
        protected PlayerStatCompo _playerStat;
        
        public virtual void OnEquipment()
        {
            HandleStatAdder();
            
            EquipmentChannelSO channel = equipData.EventChannel;
            if(channel != null)
                channel.OnEquipped += ItemEffect;
            
            // Debug.Log(equipData.name);
        }
        
        public virtual void OffEquipment()
        {
            HandleStatRemover();
            
            EquipmentChannelSO channel = equipData.EventChannel;
            if (channel != null)
                channel.OnEquipped -= ItemEffect;

            // Debug.Log(equipData.name);
        }
        
        public void HandleStatAdder()
        {
            _playerStat = Player.Instance?.GetEntityComponent<PlayerStatCompo>();
            
            if (_playerStat == null)
            {
                Debug.LogWarning("Player Stat Component is missing, " +
                                 "add <b>PlayerStatCompo</b> to player");
                return;
            }

            if (equipData.statModifier.Count == 0)
                return;
            
            foreach (var stat in equipData.statModifier)
            {
                //Key is StatType, Value is ModifyValue
                _playerStat.AddModifier(stat.Key, equipData.itemSerialCode, stat.Value);

                if (stat.Key == StatType.HEALTH)
                {
                    PlayerHealth playerHealth = Player.Instance?.GetEntityComponent<PlayerHealth>();
                    //playerHealth?.TakeHeal(stat.Value);
                    playerHealth?.HealthUpdate();
                }
                
                Debug.Log($"{stat.Key.ToString()} Stat increase amount {stat.Value}!)");
            }
        }

        public void HandleStatRemover()
        {
            _playerStat = Player.Instance?.GetEntityComponent<PlayerStatCompo>();
            
            if (_playerStat == null)
            {
                Debug.LogWarning("Player Stat Component is missing, " +
                                 "add <b>PlayerStatCompo</b> to player");
                return;
            }

            if (equipData.statModifier.Count == 0) //is not upgrade stats
                return;
            
            foreach (var stat in equipData.statModifier)
            {
                //Key is StatType
                _playerStat.RemoveModifier(stat.Key, equipData.itemSerialCode);
                
                if (stat.Key == StatType.HEALTH)
                {
                    PlayerHealth playerHealth = Player.Instance?.GetEntityComponent<PlayerHealth>();
                    //playerHealth?.TakeHeal(-stat.Value);
                    playerHealth?.HealthUpdate();
                }
                
                Debug.Log($"{stat.Key.ToString()} Stat decrease amount {stat.Value}!)");
            }
        }
    }
}
