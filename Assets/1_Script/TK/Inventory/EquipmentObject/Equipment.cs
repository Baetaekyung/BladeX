using Swift_Blade.Combat.Health;
using UnityEngine;

namespace Swift_Blade
{
    public abstract class Equipment : ItemObject
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
                //if (stat.Key == StatType.HEALTH)
                //{
                //    PlayerHealth.CurrentHealth += Mathf.RoundToInt(stat.Value);
                //}

                //Key is StatType, Value is ModifyValue
                _playerStat.AddModifier(
                    stat.Key, 
                    equipData.itemSerialCode,
                    stat.Value);

                Player.Instance.GetEntityComponent<PlayerHealth>().HealthUpdate();
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
                    //PlayerHealth.CurrentHealth -= Mathf.RoundToInt(stat.Value);

                    playerHealth?.HealthUpdate();
                }
            }

            _playerStat.DecreaseColorValue(equipData.colorType, equipData.colorAdder);
        }
    }
}
