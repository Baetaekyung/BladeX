using UnityEngine;

namespace Swift_Blade.Combat
{
    public class PlayerParryController : MonoBehaviour,IEntityComponent
    {
        private Player player;
        private bool canParry;

        [Range(0.1f, 1.5f)] [SerializeField] private float parryTime;
        public float ParryTime => parryTime;
        
        public void EntityComponentAwake(Entity entity)
        {
            player = entity as Player;
        }

        public void SetParry(bool _active)
        {
            canParry = _active;
        }
        
        public bool CanParry() => canParry;
        
        
    }
}
