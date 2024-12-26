using UnityEngine;

namespace Swift_Blade
{
    public class PlayerStat : StatComponent, IEntityComponentRequireInit
    {
        public void EntityComponentAwake(Entity entity)
        {
            Initalize();
        }
    }
}
