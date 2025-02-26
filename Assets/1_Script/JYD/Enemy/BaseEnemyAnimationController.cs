using System;
using Swift_Blade.Combat.Caster;
using Swift_Blade.Feeling;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.Rendering;

namespace Swift_Blade.Enemy
{
    public class BaseEnemyAnimationController : MonoBehaviour
    {
        protected Animator Animator;
        protected NavMeshAgent NavMeshAgent;
        
        protected BaseEnemy enemy;
        protected LayerCaster layerCaster;

        [SerializeField] [Range(1,60)] private float defaultAttackMoveSpeed;
        private float attackMoveSpeed;
                
        public float AttackMoveSpeed => attackMoveSpeed;
        
        [Space]
        public bool animationEnd;
        public bool isManualRotate;
        public bool isManualMove;

        protected virtual void Awake()
        {
            Animator = GetComponent<Animator>();
            enemy = GetComponent<BaseEnemy>();
            NavMeshAgent = GetComponent<NavMeshAgent>();
            layerCaster = GetComponentInChildren<LayerCaster>();
        }
        private void Cast()
        {
            layerCaster.CastDamage();
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
                
        public void SetAllAnimationEnd()
        {
            foreach (AnimatorControllerParameter parameter in Animator.parameters)
            {
                if (parameter.type == AnimatorControllerParameterType.Bool ||
                    parameter.type == AnimatorControllerParameterType.Trigger)
                {
                    Animator.SetBool(parameter.name, false);
                }
            }
        }
        
    }
}
