using UnityEngine;

namespace Swift_Blade
{
    public interface ITriggerable
    {
        public void Trigger();
        public bool IsTriggered();
    }
}
