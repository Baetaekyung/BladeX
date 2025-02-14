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
        public Transform target;

        [SerializeField] protected CameraShakeType cameraShakeType;
        [SerializeField] protected float rotateSpeed;
        [Range(0, 5)] [SerializeField] protected float stopDistance;

        [Header("Detect Forward")] public Transform checkForward;

        public LayerMask whatIsWall;
        public float maxDistance;
        public bool showGizmo;
        protected Collider _collider;
        protected Vector3 attackDestination;
        protected BaseEnemyAnimationController baseAnimationController;
        protected BaseEnemyHealth baseHealth;
        protected BehaviorGraphAgent btAgent;

        protected NavMeshAgent NavmeshAgent;

        protected Vector3 nextPathPoint;

        protected EnemySpawner owner;

        protected virtual void Start()
        {
            btAgent = GetComponent<BehaviorGraphAgent>();
            baseAnimationController = GetComponentInChildren<BaseEnemyAnimationController>();
            NavmeshAgent = GetComponent<NavMeshAgent>();
            baseHealth = GetComponent<BaseEnemyHealth>();
            _collider = GetComponent<Collider>();

            InitTarget();
        }

        protected virtual void Update()
        {
            if (baseHealth.isDead) return;

            if (baseAnimationController.isManualRotate) FactToTarget(target.position);

            if (baseAnimationController.isManualMove)
            {
                var directionToTarget = (target.position - transform.position).normalized;
                attackDestination = target.position - directionToTarget * 1f;

                var distance = Vector3.Distance(transform.position, target.position);

                if (distance > stopDistance)
                {
                    attackDestination = transform.position + transform.forward;

                    transform.position = Vector3.MoveTowards(transform.position, attackDestination,
                        baseAnimationController.AttackMoveSpeed * Time.deltaTime);
                }
            }
        }

        protected virtual void OnDrawGizmos()
        {
            if (showGizmo == false) return;

            Gizmos.DrawRay(checkForward.position, checkForward.forward * maxDistance);
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

        public void FactToTarget(Vector3 target)
        {
            var targetRot = Quaternion.LookRotation(target - transform.position);
            var currentEulerAngle = transform.rotation.eulerAngles;

            var yRotation = Mathf.LerpAngle(currentEulerAngle.y, targetRot.eulerAngles.y, rotateSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(currentEulerAngle.x, yRotation, currentEulerAngle.z);
        }

        protected void StopImmediately()
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

        public bool DetectForwardObstacle()
        {
            var ray = new Ray(checkForward.position, checkForward.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxDistance, whatIsWall)) return true;
            return false;
        }
    }
}