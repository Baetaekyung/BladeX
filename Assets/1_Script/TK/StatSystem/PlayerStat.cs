using UnityEngine;

namespace Swift_Blade
{
    public class PlayerStat : StatComponent, IEntityComponent
    {
        public void EntityComponentAwake(Entity entity)
        {
            Initalize();
        }
    }
}
