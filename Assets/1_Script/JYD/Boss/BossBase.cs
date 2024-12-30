using System.Collections;
using Swift_Blade.Feeling;
using UnityEngine;
using UnityEngine.AI;

namespace Swift_Blade.Boss
{
    public class BossBase : MonoBehaviour
    {
        private BossAnimationController bossAnimationController;
        
        private NavMeshAgent NavmeshAgent;
        
        private Vector3 nextPathPoint;
        private Vector3 attackDestination;
        
        public Transform target;
               
        [SerializeField] private CameraShakeType cameraShakeType;
        
        /*[Header("Knockback info")]
        public bool isKnockback;
        public float knockbackTime;
        public float knockbackThreshold;*/

        private void Start()
        {
            bossAnimationController = GetComponent<BossAnimationController>();
            NavmeshAgent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            if (bossAnimationController.isManualRotate)
            {
                FactToTarget(target.position);
            }

            if (bossAnimationController.isManualMove)
            {
                Vector3 directionToTarget = (target.position - transform.position).normalized;

                attackDestination = target.position - directionToTarget * 1f;

                transform.position = Vector3.MoveTowards(transform.position, attackDestination,
                    bossAnimationController.AttackMoveSpeed * Time.deltaTime);
            }
        }
        
        
        public void FactToTarget(Vector3 target)
        {
            Quaternion targetRot = Quaternion.LookRotation(target - transform.position);
            Vector3 currentEulerAngle = transform.rotation.eulerAngles;

            float yRotation = Mathf.LerpAngle(currentEulerAngle.y, targetRot.eulerAngles.y, 20 * Time.deltaTime);
            transform.rotation = Quaternion.Euler(currentEulerAngle.x, yRotation, currentEulerAngle.z);
        }
        
        private void StopImmediately()
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
        
        
        public void SetDead()
        {
            bossAnimationController.StopAllAnimationEvents();
            StopImmediately();
        }
        
        public void ShakeCam()
        {
            CameraShakeManager.Instance.DoShake(cameraShakeType);
        }
        
        private void SetForce(ActionData actionData)
        {
            Vector3 dir = actionData.knockbackDir.normalized; // 방향 정규화
            dir.y = 0; // y축은 고정

            float power = actionData.knockbackPower;
            float duration = actionData.knockbackDuration;

            StartCoroutine(AddForce(dir, power, duration));
        }
        
        private IEnumerator AddForce(Vector3 dir, float power, float duration)
        {
            StopImmediately(); // 움직임 초기화
            float currentTime = 0f;

            Vector3 initialPos = transform.position; // 시작 위치
            Vector3 targetPos = initialPos + dir * power; // 목표 위치

            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                float t = currentTime / duration; // 0에서 1로 진행
                transform.position = Vector3.Lerp(initialPos, targetPos, t); // 보간
                yield return null;
            }

            // 목표 위치에 도달한 후 위치를 보정
            transform.position = targetPos;
        }
    }
}