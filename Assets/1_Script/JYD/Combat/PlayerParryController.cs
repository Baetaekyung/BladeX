using System;
using Swift_Blade.Skill;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Swift_Blade.Combat
{
    public class PlayerParryController : MonoBehaviour, IEntityComponent, IEntityComponentStart
    {
        private Player player;
        private PlayerStatCompo playerStatCompo;
        private bool canParry;

        [Range(0.01f, 1.5f)][SerializeField] private float parryTime;
        public float ParryTime => parryTime;

        public UnityEvent ParryEvents;

        [Space] [Header("Shield info")] 
        public int maxShieldAmount;
        public int shieldBuffTime;
        
        private const string PARRY_KEY = "PARRY_KEY";
        private int shieldIncreaseAmount = 0;
                
        public void EntityComponentAwake(Entity entity)
        {
            player = entity as Player;
        }

        public void EntityComponentStart(Entity entity)
        {
            playerStatCompo = player.GetPlayerStat;
            
            ParryEvents.AddListener(() => player.GetSkillController.UseSkill(SkillType.Parry));
            ParryEvents.AddListener(AddShield);
        }
        
        private void AddShield()
        {
            shieldIncreaseAmount = Mathf.Min(shieldIncreaseAmount + 1, maxShieldAmount);
            
            playerStatCompo.BuffToStat(
                StatType.HEALTH,
                PARRY_KEY,
                shieldBuffTime,
                shieldIncreaseAmount,
                () => { },
                () =>
                {
                    shieldIncreaseAmount = 0;
                }
            );
        }
        
        private void OnDestroy()
        {
            ParryEvents.RemoveAllListeners();
        }
        
        public bool GetParry()
        {
            return canParry;
        }

        public void SetParry(bool _active)
        {
            canParry = _active;
        }
        private bool ParryProbability()
        {
            float additionalParryChance = playerStatCompo.GetStat(StatType.PARRY_CHANCE).Value;
            const float defaultParryChance = 0.5f;
            float parryChance = defaultParryChance + additionalParryChance;
            return parryChance > Random.value;
        }


    }
}
