using Swift_Blade.Audio;
using UnityEngine;

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

        public override void StopAllAnimationEvents()
        {
            StopDrawBowstring();
            base.StopAllAnimationEvents();
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
