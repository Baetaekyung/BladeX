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

        protected override void Update()
        {
            if (baseAnimationController.isManualRotate) FactToTarget(target.position);

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

            if (baseAnimationController is GoblinBossAnimatorController goblinAnimatorController)
                if (goblinAnimatorController.isManualKnockback)
                {
                    attackDestination = transform.position + -transform.forward;

                    transform.position = Vector3.MoveTowards(transform.position, attackDestination,
                        goblinAnimatorController.knockbackSpeed * Time.deltaTime);
                }
        }

        public void Init(GoblinBoss boss)
        {
            parent = boss;
        }

        public override void SetDead()
        {
            base.SetDead();
            parent.RemoveInSummonList(this);
        }
    }
}