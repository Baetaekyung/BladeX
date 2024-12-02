using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BossAnimationController : MonoBehaviour
{
    public Animator Animator;
    public NavMeshAgent NavMeshAgent;
    public Rigidbody Rigidbody;
    
    public Transform target;
    
    public bool animationEnd;
    public bool isManualRotate;
    public bool isManualMove;

    private Vector3 nextPathPoint;
    private Vector3 attackDestination;
    [SerializeField] private float attackMoveSpeed;
    
    [Header("Knockback info")]
    public bool isKnockback;
    public float knockbackTime;
    public float knockbackThreshold;
    
    private void Update()
    {
        if (isManualRotate)
        {
            FactToTarget(target.position);
        }
        
        if (isManualMove)
        {
            attackDestination = target.position;
            
            transform.position = Vector3.MoveTowards(transform.position , attackDestination , 
                attackMoveSpeed * Time.deltaTime);
        }
        
    }
    
    public void GetKnockback()
    {
        Vector3 test = -transform.forward;
        StartCoroutine(ApplyKnockback(test * 2.5f));
    }

    private IEnumerator ApplyKnockback(Vector3 force)
    {
        yield return null;

        isKnockback = true;
        
        NavMeshAgent.enabled = false;
        Rigidbody.isKinematic = false;
        Rigidbody.useGravity = true;

        Rigidbody.linearVelocity = Vector3.zero;
        
        Rigidbody.AddForce(force,ForceMode.Impulse);
        
        yield return new WaitForFixedUpdate();
        yield return new WaitUntil(() => Rigidbody.linearVelocity.magnitude < knockbackThreshold);
        yield return new WaitForSeconds(0.25f);
        
        Rigidbody.linearVelocity = Vector3.zero;
        Rigidbody.angularVelocity = Vector3.zero;
        Rigidbody.useGravity = false;
        Rigidbody.isKinematic = true;

        NavMeshAgent.Warp(transform.position);
        NavMeshAgent.enabled = true;

        isKnockback = false;
        
        yield return null;
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

    public void SetAnimationEnd()
    {
        animationEnd = true;
    }
    
    public void StopAnimationEnd()
    {
        animationEnd = false;
    }

    public void StartManualRotate()
    {
        isManualRotate = true;
    }

    public void StopManualRotate()
    {
        isManualRotate = false;
    }
    
    
    public void StartManualMove()
    {
        isManualMove = true;
    }
    
    public void StopManualMove()
    {
        isManualMove = false;
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
