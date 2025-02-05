using Swift_Blade.Enemy.Boss.Golbin;
using Swift_Blade.Enemy.Goblin;
using Unity.Behavior;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Swift_Blade.Enemy.Golbin
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
            if (baseAnimationController.isManualRotate)
            {
                FactToTarget(target.position);
            }

            if (baseAnimationController.isManualMove)
            {
                float distance = Vector3.Distance(transform.position , target.position);
                
                if (distance > stopDistance)
                {
                    attackDestination = transform.position + transform.forward;

                    transform.position = Vector3.MoveTowards(transform.position, attackDestination, 
                        baseAnimationController.AttackMoveSpeed * Time.deltaTime);
                }
            }

            if (baseAnimationController is GoblinBossAnimatorController goblinAnimatorController)
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
