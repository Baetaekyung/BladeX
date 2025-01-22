using Unity.Behavior;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Swift_Blade.Boss.Goblin
{
    public class GoblinEnemyInBoss : BaseGoblin
    {
        private GoblinBoss parent;
        
        [SerializeField] private float maxAnimationSpeed;
        [SerializeField] private float minAnimationSpeed;
        
        public void Init(GoblinBoss boss)
        {
            parent = boss;
            
            float animationSpeed = Random.Range(minAnimationSpeed , maxAnimationSpeed);
            goblinAnimator.SetAnimationSpeed(animationSpeed);
        }
        
        protected override void Start()
        {
            base.Start();
            btAgent.enabled = false;
            
            if (target == null)
            {
                target = GameObject.Find("Player").transform;
                btAgent.BlackboardReference.SetVariableValue("Target", target);
                btAgent.enabled = true;
            }
        }

        protected override void Update()
        {
            if (bossAnimationController.isManualRotate)
            {
                FactToTarget(target.position);
            }

            if (bossAnimationController.isManualMove)
            {
                float distance = Vector3.Distance(transform.position , target.position);
                
                if (distance > stopDistance)
                {
                    attackDestination = transform.position + transform.forward;

                    transform.position = Vector3.MoveTowards(transform.position, attackDestination, 
                        bossAnimationController.AttackMoveSpeed * Time.deltaTime);
                }
            }

            if (bossAnimationController is GoblinBossAnimatorController goblinAnimatorController)
            {
                if (goblinAnimatorController.isManualKnockback)
                {
                    attackDestination = transform.position + -transform.forward;

                    transform.position = Vector3.MoveTowards(transform.position, attackDestination, 
                        goblinAnimatorController.knockbackSpeed * Time.deltaTime); 
                }
            }            
            
        }

        public override void SetDead()
        {
            base.SetDead();
            parent.RemoveInSummonList(this);
        }
                
        
    }
}
