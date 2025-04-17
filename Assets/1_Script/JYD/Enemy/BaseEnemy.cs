using Swift_Blade.Combat.Health;
using Swift_Blade.Level;
using Unity.Behavior;
using UnityEngine.AI;
using UnityEngine;
using System.Linq;

namespace Swift_Blade.Enemy
{
    public class BaseEnemy : MonoBehaviour
    {
        public Transform target;
        
        [Header("Movement Info")]
        [Range(0,30)][SerializeField] protected float moveSpeed;
        [Range(0,20)][SerializeField] protected float rotateSpeed;
        [Range(-1, 5)] [SerializeField] protected float stopDistance;
        [SerializeField] private LayerMask whatIsGround;
        
        [Header("Detect Forward Info")]
        public Transform checkForward;
        public LayerMask whatIsWall;
        public float maxDistance;
        public bool showGizmo;
        
        [Header("Weapon info")]
        public GameObject weapon;
        
        protected Vector3 attackDestination;
        protected Collider enemyCollider;
        protected BehaviorGraphAgent btAgent;
        protected NavMeshAgent NavmeshAgent;
        
        protected BaseEnemyAnimationController baseAnimationController;
        protected BaseEnemyHealth baseHealth;
        
        private Vector3 nextPathPoint;
        private EnemySpawner owner;
        
        protected virtual void Start()
        {
            btAgent = GetComponent<BehaviorGraphAgent>();
            NavmeshAgent = GetComponent<NavMeshAgent>();
            baseHealth = GetComponent<BaseEnemyHealth>();
            enemyCollider = GetComponent<Collider>();
            baseAnimationController = GetComponentInChildren<BaseEnemyAnimationController>();
            
            InitBtAgent();
        }
        
        private void InitBtAgent()
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
            btAgent.SetVariableValue("MoveSpeed", moveSpeed);
                            
            btAgent.enabled = true;
        }
        
        public void SetOwner(EnemySpawner _owner)
        {
            owner = _owner;
        }
        
        protected virtual void Update()
        {
            if (baseHealth.isDead)
                return;
            
            if (baseAnimationController.isManualRotate) 
                FactToTarget(target.position);

            if (baseAnimationController.isManualMove && !DetectForwardObstacle())
            {
                bool isGround = Physics.Raycast(transform.position + Vector3.up * 1.2f, Vector3.down , out RaycastHit RaycastHit,  10, whatIsGround);
                if (isGround == false)return;
                
                var distance = Vector3.Distance(transform.position, target.position);
                
                if (distance > stopDistance)
                {
                    attackDestination = transform.position + transform.forward * 1;
                    attackDestination.y = RaycastHit.point.y;
                    
                    transform.position = Vector3.MoveTowards(transform.position, attackDestination,
                        baseAnimationController.AttackMoveSpeed * Time.deltaTime);
                }
            }
        }
        
        public void FactToTarget(Vector3 target)
        {
            var targetRot = Quaternion.LookRotation(target - transform.position);
            var currentEulerAngle = transform.rotation.eulerAngles;
            
            var yRotation = Mathf.LerpAngle(currentEulerAngle.y, targetRot.eulerAngles.y, rotateSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(currentEulerAngle.x, yRotation, currentEulerAngle.z);
        }

        private void StopImmediately()
        {
            if (NavmeshAgent.enabled == false) return;
            
            NavmeshAgent.isStopped = true;
            NavmeshAgent.velocity = Vector3.zero;
        }

        public Vector3 GetNextPathPoint()
        {
            var path = NavmeshAgent.path;

            if (path.corners.Length < 2) return NavmeshAgent.destination;

            for (var i = 0; i < path.corners.Length; i++)
            {
                var distance = Vector3.Distance(NavmeshAgent.transform.position, path.corners[i]);

                if (distance < 1 && i < path.corners.Length - 1)
                {
                    nextPathPoint = path.corners[i + 1];
                    return nextPathPoint;
                }
            }

            return nextPathPoint;
        }

        public virtual void DeadEvent()
        {
            StopImmediately();
                                    
            enemyCollider.enabled = false;
            NavmeshAgent.avoidancePriority = 99;
            NavmeshAgent.enabled = false;
            
            if(owner != null)
                owner.TryNextEnemyCanSpawn(transform.localPosition,transform.forward);
            
            if(weapon != null)
                weapon.AddComponent<EnemyWeapon>();
        }
        
        protected bool DetectForwardObstacle()
        {
            var ray = new Ray(checkForward.position, checkForward.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, whatIsWall)) return true;
            return false;
        }
        
        protected virtual void OnDrawGizmos()
        {
            if (showGizmo == false) return;
            
            Gizmos.DrawRay(checkForward.position, checkForward.forward * maxDistance);
        }
    }
}