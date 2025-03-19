using UnityEngine;
using Swift_Blade.Enemy.Bow;

namespace Swift_Blade.Enemy
{
    public class BowEnemyAnimationController : BaseEnemyAnimationController
    {
        [SerializeField] private Bowstring bowstring;
        
        public void StartDrawBowstring()
        {
            bowstring.canDraw = true;
        }
        
        public void StopDrawBowstring()
        {
            bowstring.canDraw = false;
        }
    }
}
