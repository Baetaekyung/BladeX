using System.Collections.Generic;
using UnityEngine;
using Swift_Blade.Combat.Caster;

namespace Swift_Blade
{
    public class RogueTagEffect : TagEffectBase
    {
        [SerializeField] private int _percent = 15;

        private List<StatType> _modifierStats;
        private List<object> _keys;
        
        private PlayerStatCompo _stat;
        private PlayerDamageCaster _damageCaster;

        public override void Initialize(Player player)
        {
            base.Initialize(player);

            _modifierStats = new List<StatType>();
            _keys = new List<object>();
            
            _stat = _player.GetEntityComponent<PlayerStatCompo>();
            _damageCaster = _player.GetEntityComponent<PlayerDamageCaster>();
        }

        protected override void TagEnableEffect(int tagCount)
        {
            _damageCaster.OnCastDamageEvent.AddListener(Steal);
        }

        protected override void TagDisableEffect()
        {
            for(int i = 0; i < _keys.Count; ++i)
            {
                _stat.RemoveModifier(_modifierStats[i], _keys[i]);
            }
            _damageCaster.OnCastDamageEvent.RemoveListener(Steal);
        }

        private void Steal(ActionData actionData)
        {
            if(Random.Range(0, 100) >= _percent) return;

            StatType statType = (StatType)Random.Range(0, 4);
            object key = new();
            _stat.AddModifier(statType, key, 1f);

            _modifierStats.Add(statType);
            _keys.Add(key);
        }
    }
}
