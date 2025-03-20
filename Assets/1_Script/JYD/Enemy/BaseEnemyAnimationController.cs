using Swift_Blade.Combat.Caster;
using UnityEngine;
using UnityEngine.AI;


namespace Swift_Blade.Enemy
{
    public class BaseEnemyAnimationController : MonoBehaviour
    {
        protected Animator Animator;
        protected NavMeshAgent NavMeshAgent;
        
        protected BaseEnemy enemy;
        protected ICasterAble caster;

        [SerializeField] [Range(1,60)] private float defaultAttackMoveSpeed;
        private float attackMoveSpeed;
                
        public float AttackMoveSpeed => attackMoveSpeed;
        
        [Space]
        public bool animationEnd;
        public bool isManualRotate;
        public bool isManualMove;
        
        public float maxAnimationSpeed = 1.3f;
        public float minAnimationSpeed = 1;
        
        protected virtual void Awake()
        {
            Animator = GetComponent<Animator>();
            enemy = GetComponent<BaseEnemy>();
            NavMeshAgent = GetComponent<NavMeshAgent>();
            caster = GetComponentInChildren<ICasterAble>();
        }
        
        protected void Start()
        {
            float animationSpeed = Random.Range(minAnimationSpeed, maxAnimationSpeed);
            Animator.SetFloat("Speed" ,animationSpeed);
        }
        private void Cast()
        {
            caster.Cast();
        }
        
        public void SetAnimationEnd() => animationEnd = true;
        public void StopAnimationEnd() => animationEnd = false;
        public void StartManualRotate() => isManualRotate = true;
        public void StopManualRotate() => isManualRotate = false;
        public void StartApplyRootMotion() => Animator.applyRootMotion = true;
        public void StopApplyRootMotion() => Animator.applyRootMotion = false;
        
        public void StartManualMove(float _moveSpeed = 0)
        {
            attackMoveSpeed = _moveSpeed == 0 ? defaultAttackMoveSpeed : _moveSpeed;

            isManualMove = true;

            NavMeshAgent.enabled = false;
        }
        public void StopManualMove()
        {
            attackMoveSpeed = defaultAttackMoveSpeed;

            NavMeshAgent.Warp(transform.position);
            isManualMove = false;

            NavMeshAgent.enabled = true;
        }

        public virtual void StopAllAnimationEvents()
        {
            StopAnimationEnd();
            StopManualMove();
            StopManualRotate();
            StopApplyRootMotion();
        }
        
        public void Rebind()
        {
            Animator.Rebind();
        }
        
    }
}
