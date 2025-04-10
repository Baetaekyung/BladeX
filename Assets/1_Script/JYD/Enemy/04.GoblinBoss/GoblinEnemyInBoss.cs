using Swift_Blade.Enemy.Boss.Goblin;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Swift_Blade.Enemy.Goblin
{
    public class GoblinEnemyInBoss : BaseGoblin
    {
        [SerializeField] private float maxAnimationSpeed;
        [SerializeField] private float minAnimationSpeed;
        private GoblinBoss parent;

        protected override void Start()
        {
            base.Start();
                        
            var animationSpeed = Random.Range(minAnimationSpeed, maxAnimationSpeed);
            goblinAnimator.SetAnimationSpeed(animationSpeed);
            
            
        }
        
        public void Init(GoblinBoss boss)
        {
            parent = boss;
        }
                
        public override void DeadEvent()
        {
            base.DeadEvent();
            parent.RemoveInSummonList(this);
        }
    }
}