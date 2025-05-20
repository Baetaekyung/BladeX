using Swift_Blade.Audio;
using Swift_Blade.Combat.Caster;
using UnityEngine.AI;
using UnityEngine;


namespace Swift_Blade.Enemy
{
    public class BaseEnemyAnimationController : MonoBehaviour
    {
        protected Animator Animator;
        protected NavMeshAgent NavMeshAgent;
        
        protected BaseEnemy enemy;
        protected ICasterAble caster;

        [SerializeField] [Range(1,60)] protected float defaultAttackMoveSpeed;
        protected float attackMoveSpeed;
        
        public float AttackMoveSpeed => attackMoveSpeed;
        
        [Space]
        public bool animationEnd;
        public bool isManualRotate;
        public bool isManualMove;
        
        public float maxAnimationSpeed = 1.3f;
        public float minAnimationSpeed = 1;
        
        private readonly int ANIMATION_SPEED = Animator.StringToHash("AnimationSpeed");
        public Animator GetAnimator() => Animator;
        
        private float originMotionSpeed;
        float originMoveSpeed;
        
        protected virtual void Awake()
        {
            Animator = GetComponent<Animator>();
            enemy = GetComponent<BaseEnemy>();
            NavMeshAgent = GetComponent<NavMeshAgent>();
            caster = GetComponentInChildren<ICasterAble>();
        }
        
        protected virtual void Start()
        {
            SetRandomAttackAnimationSpeed();
        }
        
        protected void SetRandomAttackAnimationSpeed()
        {
            originMotionSpeed = Random.Range(minAnimationSpeed, maxAnimationSpeed);
            SetAnimationSpeed(originMotionSpeed);
        }
        
        public void ResetAnimationSpeed()
        {
            SetAnimationSpeed(originMotionSpeed);
        }
        
        public void SetAnimationSpeed(float animationSpeed)
        {
            Animator.SetFloat(ANIMATION_SPEED ,animationSpeed);
        }
        
        public void MultiplyDefaultAttackMoveSpeed(float ratio)
        {
            originMoveSpeed = defaultAttackMoveSpeed;
            defaultAttackMoveSpeed *= ratio;
        }
        
        public void ResetDefaultMoveSpeed()
        {
            defaultAttackMoveSpeed = originMoveSpeed;
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
            
            if(NavMeshAgent != null)
                NavMeshAgent.enabled = false;
        }
        public virtual void StopManualMove()
        {
            if(NavMeshAgent == null || enemy == null)return;
            
            attackMoveSpeed = defaultAttackMoveSpeed;
            NavMeshAgent.Warp(transform.position);
            isManualMove = false;
            
            //if dead? off navmeshAgent
            NavMeshAgent.enabled = !enemy.GetHealth().isDead;
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
        
        private void OnAudioPlay(AudioSO audio)
        {
            AudioManager.PlayWithInit(audio, true);
        }
        
        private void OnAudioPlayCollection(AudioCollectionSO audioCollectionSo) => OnAudioPlay(audioCollectionSo.GetRandomAudio);
        
       
    }
}
