using UnityEngine;
using UnityEngine.Events;

namespace Swift_Blade.Combat
{
    public class PlayerParryController : MonoBehaviour,IEntityComponent,IEntityComponentStart
    {
        private Player player;
        private bool canParry;

        [Range(0.01f, 1.5f)] [SerializeField] private float parryTime;
        public float ParryTime => parryTime;

        public UnityEvent<ActionData> ParryEvents;
        
        public void EntityComponentAwake(Entity entity)
        {
            player = entity as Player;
        }
        
        public void EntityComponentStart(Entity entity)
        {
            
        }

        public void SetParry(bool _active)
        {
            canParry = _active;
        }
        
        public bool CanParry() => canParry;

       
    }
}
