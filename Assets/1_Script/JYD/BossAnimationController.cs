using System.Collections;
using Swift_Blade.Feeling;
using UnityEngine;
using UnityEngine.AI;


public class BossAnimationController : MonoBehaviour
{
    public Animator Animator;
    public NavMeshAgent NavMeshAgent;
    public BossHealth bossHealth;
    
    public Transform target;
    
    public bool animationEnd;
    public bool isManualRotate;
    public bool isManualMove;

    private Vector3 nextPathPoint;
    private Vector3 attackDestination;
    [SerializeField] private float defaultAttackMoveSpeed;
    private float attackMoveSpeed;
    
    
    [SerializeField] private CameraShakeType cameraShakeType;
    [SerializeField] private LayerCaster layerCaster;
    
    /*[Header("Knockback info")]
    public bool isKnockback;
    public float knockbackTime;
    public float knockbackThreshold;*/

    private void Start()
    {
        bossHealth.OnDeadEvent += SetDead;
        
        bossHealth.OnParryHitEvent += SetForce;
    }

    private void OnDestroy()
    {
        bossHealth.OnDeadEvent -= SetDead;;
        
        bossHealth.OnParryHitEvent -= SetForce;
    }

    private void Update()
    {
        if (isManualRotate)
        {
            FactToTarget(target.position);
        }
        
        if (isManualMove)
        {
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            
            attackDestination = target.position - directionToTarget * 1f;
            
            transform.position = Vector3.MoveTowards(transform.position , attackDestination , 
                attackMoveSpeed * Time.deltaTime);
        }
    }

    private void SetDead()
    {
        StopManualMove();
        StopManualRotate();
        StopApplyRootMotion();
        StopImmediately();
    }
    
    private void Cast()
    {
        layerCaster.CastDamage();
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

    
    private void StopImmediately()
    {
        if (NavMeshAgent.enabled == false) return;
        
        NavMeshAgent.isStopped = true;
        NavMeshAgent.velocity = Vector3.zero;
    }
    
    public Vector3 GetNextPathPoint()
    {
        NavMeshPath path = NavMeshAgent.path;

        if(path.corners.Length < 2)
        {
            return NavMeshAgent.destination;
        }

        for(int i = 0; i < path.corners.Length; i++)
        {
            float distance = Vector3.Distance(NavMeshAgent.transform.position, path.corners[i]);

            if (distance < 1 && i < path.corners.Length - 1)
            {
                nextPathPoint = path.corners[i + 1];
                return nextPathPoint;
            }
        }
        
        return nextPathPoint;
    }
        
    public void FactToTarget(Vector3 target)
    {
        Quaternion targetRot = Quaternion.LookRotation(target - transform.position);
        Vector3 currentEulerAngle = transform.rotation.eulerAngles;

        float yRotation = Mathf.LerpAngle(currentEulerAngle.y, targetRot.eulerAngles.y, 20 * Time.deltaTime);
        transform.rotation = Quaternion.Euler(currentEulerAngle.x, yRotation, currentEulerAngle.z);
    }
    
    #region AnimationEvents

    public void SetAnimationEnd() => animationEnd = true;
    public void StopAnimationEnd()=>animationEnd = false;
    public void StartManualRotate() =>isManualRotate = true;
    public void StopManualRotate() =>isManualRotate = false;
    
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
    
    #endregion
    public void SetAllAnimationEnd()
    {
        foreach (AnimatorControllerParameter parameter in Animator.parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Bool || parameter.type == AnimatorControllerParameterType.Trigger)
            {
                Animator.SetBool(parameter.name, false);
            }
        }
    }
}
