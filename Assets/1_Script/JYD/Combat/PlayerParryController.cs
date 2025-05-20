using Random = UnityEngine.Random;
using UnityEngine.Events;
using System.Text;
using UnityEngine;

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

        [Space]
        [Header("Shield info")] 
        public int shieldBuffTime;
                
        private StringBuilder parryKey = new StringBuilder();
        private const int SHIELD_INCREASE_AMOUNT = 1;
        
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
            parryKey.Append("PARRY_KEY");
                        
            playerStatCompo.BuffToStat
            (
                StatType.HEALTH,
                parryKey.Append(Time.time).ToString(),
                shieldBuffTime,
                SHIELD_INCREASE_AMOUNT
            );
            
            parryKey.Clear();
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
