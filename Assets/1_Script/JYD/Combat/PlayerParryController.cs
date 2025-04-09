using System;
using Swift_Blade.Skill;
using UnityEngine;
using UnityEngine.Events;
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

        public void EntityComponentAwake(Entity entity)
        {
            player = entity as Player;
        }

        public void EntityComponentStart(Entity entity)
        {
            playerStatCompo = player.GetEntityComponent<PlayerStatCompo>();
            ParryEvents.AddListener(() => player.GetSkillController.UseSkill(SkillType.Special));
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
