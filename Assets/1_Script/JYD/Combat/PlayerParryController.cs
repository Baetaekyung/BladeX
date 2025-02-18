using UnityEngine;
using UnityEngine.Events;

namespace Swift_Blade.Combat
{
    public class PlayerParryController : MonoBehaviour, IEntityComponent
    {
        private Player player;
        private PlayerStatCompo playerStatCompo;
        private bool canParry;

        [Range(0.01f, 1.5f)] [SerializeField] private float parryTime;
        public float ParryTime => parryTime;

        public UnityEvent ParryEvents;

        public void EntityComponentAwake(Entity entity)
        {
            player = entity as Player;
            playerStatCompo = player.GetEntityComponent<PlayerStatCompo>(); 
        }
        public void SetParry(bool _active)
        {
            canParry = _active;
        }
        private bool ParryProbability()
        {
            float additinoalParryChance = playerStatCompo.GetStatByType(StatType.PARRY_CHANCE).Value;
            const float defaultParryChance = 0.5f;
            float parryChance = defaultParryChance + additinoalParryChance;
            return parryChance > Random.value;
        }
        public bool CanParry() => canParry && ParryProbability();

    }
}
