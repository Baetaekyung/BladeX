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

        protected override void Update()
        {
            if(baseHealth.isDead)return;
            
            if (baseAnimationController.isManualRotate) 
                FactToTarget(target.position);
            
            if (baseAnimationController.isManualMove)
            {
                var distance = Vector3.Distance(transform.position, target.position);

                if (distance > stopDistance)
                {
                    attackDestination = transform.position + transform.forward;

                    transform.position = Vector3.MoveTowards(transform.position, attackDestination,
                        baseAnimationController.AttackMoveSpeed * Time.deltaTime);
                }
            }

            if (goblinAnimator.isManualKnockback)
            {
                attackDestination = transform.position + -transform.forward;

                transform.position = Vector3.MoveTowards(transform.position, attackDestination,
                    goblinAnimator.knockbackSpeed * Time.deltaTime);
            }
            
        }
        
        public override void DeadEvent()
        {
            base.DeadEvent();
            parent.RemoveInSummonList(this);
        }
    }
}