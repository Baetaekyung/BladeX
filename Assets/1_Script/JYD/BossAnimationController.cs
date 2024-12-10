using System;
using System.Collections;
using Swift_Blade;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public struct ActionData
{
    public Vector3 knockbackDir;
    public float knockbackDuration;
    public float knockbackPower;
    public float healthPercent;
}


public class BossAnimationController : MonoBehaviour
{
    public Animator Animator;
    public NavMeshAgent NavMeshAgent;
    public EnemyHealth EnemyHealth;
    
    public Transform target;
    
    public bool animationEnd;
    public bool isManualRotate;
    public bool isManualMove;

    private Vector3 nextPathPoint;
    private Vector3 attackDestination;
    [SerializeField] private float attackMoveSpeed;

    [SerializeField] private CameraShakeType cameraShakeType;
    [FormerlySerializedAs("DamageCaster")] [SerializeField] private LayerCaster layerCaster;
    
    
    [Header("Knockback info")]
    public bool isKnockback;
    public float knockbackTime;
    public float knockbackThreshold;

    private void Start()
    {
        EnemyHealth.OnHitEvent += SetForce;
        
        EnemyHealth.OnDeadEvent += StopManualMove;
        EnemyHealth.OnDeadEvent += StopManualRotate;
        EnemyHealth.OnDeadEvent += StopApplyRootMotion;
        EnemyHealth.OnDeadEvent += StopImmediately;
    }

    private void OnDestroy()
    {
        EnemyHealth.OnHitEvent -= SetForce;
        
        EnemyHealth.OnDeadEvent -= StopManualMove;
        EnemyHealth.OnDeadEvent -= StopManualRotate;
        EnemyHealth.OnDeadEvent -= StopApplyRootMotion;
        EnemyHealth.OnDeadEvent -= StopImmediately;
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
    private void Cast()
    {
        layerCaster.CastDamage();
    }
    public void ShakeCam()
    {
        CameraShakeManager.Instance.GenerateImpulse(cameraShakeType);
    }
    
    public void SetForce(ActionData actionData)
    {
        Vector3 dir = actionData.knockbackDir;
        dir.y = 0;
        
        float power = actionData.knockbackPower;
        float duration = actionData.knockbackDuration;
        
        StartCoroutine(AddForce(dir, power, duration));
    }
    
    private void StopImmediately()
    {
        if (NavMeshAgent.enabled == false) return;
        
        NavMeshAgent.isStopped = true;
        NavMeshAgent.velocity = Vector3.zero;
    }
    
    private IEnumerator AddForce(Vector3 dir, float power, float duration)
    {
        StopImmediately();
        float currentTime = 0;
        
        Vector3 endPos = transform.position + dir * power;
        while(currentTime < duration){
            currentTime += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, endPos, Time.deltaTime);
            yield return null;
        }
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
        
    private void FactToTarget(Vector3 target)
    {
        Quaternion targetRot = Quaternion.LookRotation(target - transform.position);
        Vector3 currentEulerAngle = transform.rotation.eulerAngles;

        float yRotation = Mathf.LerpAngle(currentEulerAngle.y, targetRot.eulerAngles.y, 5 * Time.deltaTime);
        transform.rotation = Quaternion.Euler(currentEulerAngle.x, yRotation, currentEulerAngle.z);
    }

    #region AnimationEvents

    public void SetAnimationEnd() => animationEnd = true;
    public void StopAnimationEnd()=>animationEnd = false;
    public void StartManualRotate() =>isManualRotate = true;

    public void StopManualRotate() =>isManualRotate = false;
    
    public void StartApplyRootMotion() => Animator.applyRootMotion = true;
    public void StopApplyRootMotion() => Animator.applyRootMotion = false;
    
    public void StartManualMove()
    {
        isManualMove = true;
        NavMeshAgent.enabled = false;
    }
    
    public void StopManualMove()
    {
        isManualMove = false;
        NavMeshAgent.Warp(transform.position);
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
