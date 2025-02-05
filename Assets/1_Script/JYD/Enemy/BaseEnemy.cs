using System.Linq;
using Swift_Blade.Combat.Health;
using Swift_Blade.Feeling;
using Swift_Blade.Level;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;

namespace Swift_Blade.Enemy
{
    public class BaseEnemy : MonoBehaviour
    {
        protected BaseEnemyAnimationController baseAnimationController;
        protected BaseEnemyHealth baseHealth;
        
        protected NavMeshAgent NavmeshAgent;
        protected BehaviorGraphAgent btAgent;
        
        protected Vector3 nextPathPoint;
        protected Vector3 attackDestination;
        
        public Transform target;
               
        [SerializeField] protected CameraShakeType cameraShakeType;
        [SerializeField] protected float rotateSpeed;
        [Range(0,5)][SerializeField] protected float stopDistance;

        protected EnemySpawner owner;
        protected Collider _collider;

        
        protected virtual void Start()
        {
            btAgent = GetComponent<BehaviorGraphAgent>();
            baseAnimationController = GetComponentInChildren<BaseEnemyAnimationController>();
            NavmeshAgent = GetComponent<NavMeshAgent>();
            baseHealth = GetComponent<BaseEnemyHealth>();
            _collider = GetComponent<Collider>();
            
            InitTarget();
        }

        private void InitTarget()
        {
            btAgent.enabled = false;
            if (target == null)
            {
                var player = FindObjectsByType<PlayerMovement>(FindObjectsSortMode.None).FirstOrDefault();
                if (player != null)
                    target = player.transform;
            }

            if (target == null) return;
            btAgent.SetVariableValue("Target", target);
            btAgent.enabled = true;
        }

        protected virtual void Update()
        {
            if(baseHealth.isDead)return;
            
            if (baseAnimationController.isManualRotate)
            {
                FactToTarget(target.position);
            }

            if (baseAnimationController.isManualMove)
            {
                Vector3 directionToTarget = (target.position - transform.position).normalized;
                attackDestination = target.position - directionToTarget * 1f;
                
                float distance = Vector3.Distance(transform.position , target.position);
                
                if (distance > stopDistance)
                {
                    attackDestination = transform.position + transform.forward;

                    transform.position = Vector3.MoveTowards(transform.position, attackDestination,
                        baseAnimationController.AttackMoveSpeed * Time.deltaTime);
                }
                
            }
        }
        
        public void FactToTarget(Vector3 target)
        {
            Quaternion targetRot = Quaternion.LookRotation(target - transform.position);
            Vector3 currentEulerAngle = transform.rotation.eulerAngles;

            float yRotation = Mathf.LerpAngle(currentEulerAngle.y, targetRot.eulerAngles.y, rotateSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(currentEulerAngle.x, yRotation, currentEulerAngle.z);
        }
                
        protected void StopImmediately()
        {
            if (NavmeshAgent.enabled == false) return;

            NavmeshAgent.isStopped = true;
            NavmeshAgent.velocity = Vector3.zero;
        }
        
        private Vector3 GetNextPathPoint()
        {
            NavMeshPath path = NavmeshAgent.path;

            if (path.corners.Length < 2)
            {
                return NavmeshAgent.destination;
            }

            for (int i = 0; i < path.corners.Length; i++)
            {
                float distance = Vector3.Distance(NavmeshAgent.transform.position, path.corners[i]);

                if (distance < 1 && i < path.corners.Length - 1)
                {
                    nextPathPoint = path.corners[i + 1];
                    return nextPathPoint;
                }
            }

            return nextPathPoint;
        }
        
        public virtual void SetDead()
        {
            owner?.CheckSpawn();
            
            _collider.enabled = false;
            
            StopImmediately();
            baseAnimationController.StopAllAnimationEvents();
        }

        public void SetOwner(EnemySpawner _owner)
        {
            owner = _owner;
        }
                
    }
}