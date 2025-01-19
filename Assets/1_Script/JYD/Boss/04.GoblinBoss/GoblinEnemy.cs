using Unity.Behavior;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Swift_Blade.Boss.Goblin
{
    public class GoblinEnemy : BaseBoss
    {
        private GoblinBoss boss;
        public float stopDistance;
        
        public float maxAnimationSpeed;
        public float minAnimationSpeed;

        private BehaviorGraphAgent btAgent;

        private void Awake()
        {
            btAgent = GetComponent<BehaviorGraphAgent>();
            
            if (target == null)
            {
                target = GameObject.Find("Player").transform;
                btAgent.BlackboardReference.SetVariableValue("Target", target);
                btAgent.enabled = true;
            }
        }
        
        protected override void Start()
        {
            base.Start();
                                    
            float animationSpeed = Random.Range(minAnimationSpeed , maxAnimationSpeed);
            (bossAnimationController as GoblinAnimatorController).SetAnimationSpeed(animationSpeed);
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

            if (bossAnimationController is GoblinAnimatorController goblinAnimatorController)
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
            boss.RemoveInSummonList(this);
        }

        public void Init(GoblinBoss _boss)
        {
            boss = _boss;
        }
        
    }
}
