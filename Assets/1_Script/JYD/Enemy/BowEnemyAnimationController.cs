using UnityEngine;
using Swift_Blade.Enemy.Bow;

namespace Swift_Blade.Enemy.Bow
{
    public class BowEnemyAnimationController : BaseEnemyAnimationController
    {
        [SerializeField] private Bowstring bowstring;

        protected override void Start()
        {
            base.Start();
            StopDrawBowstring();
        }

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
