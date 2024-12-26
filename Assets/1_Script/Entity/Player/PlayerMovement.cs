using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

namespace Swift_Blade
{
    public class PlayerMovement : PlayerComponentBase, IEntityComponentRequireInit
    {
        [Header("Movement Settings")]
        [SerializeField] private float onGroundYVal;
        [SerializeField] private float gravitiy = -9.81f;
        [SerializeField] private float gravitiyMultiplier = 1;
        private Vector3 velocity;

        [Header("Roll Settings")]
        [SerializeField] private AnimationCurve rollCurve; // curve length should be 1.
        [SerializeField] private float debug_stmod;

        [Header("DashSetting")]
        [SerializeField] private CinemachinePositionComposer cine;
        [SerializeField] private TrailRenderer dashPar;

        private const float rollcost = 1f;
        private const float initialRollStamina = 3f;
        private float rollStamina;
        public float SpeedMultiplier { get; set; } = 1;
        public float GetCurrentRollStamina => rollStamina;
        public float GetMaxStamina => initialRollStamina + debug_stmod;

        public Vector3 InputDirection { get; set; }
        public Vector3 RollForce { get; private set; }
        public bool AllowInputMoving { get; set; } = true;
        private Rigidbody controller;
        public float GetCurrentStamina => rollStamina;
        private PlayerRenderer playerRenderer;
        public void EntityComponentAwake(Entity entity)
        {
            playerRenderer = entity.GetEntityComponent<PlayerRenderer>();
            controller = GetComponent<Rigidbody>();
            rollStamina = initialRollStamina;
        }
        private void Update()
        {
            rollStamina += Time.deltaTime;
            rollStamina = Mathf.Min(GetMaxStamina, rollStamina);
            if (Input.GetKeyDown(KeyCode.V))
            {
                //controller.linearVelocity = Vector3.zero;
                AddForceLocaly(Vector3.forward * 3);
            }
        }
        private void FixedUpdate()
        {
            velocity = Vector3.MoveTowards(velocity, Vector3.zero, Time.fixedDeltaTime * 21);
            ApplyMovement();
        }
        private void ApplyMovement()
        {
            if (!AllowInputMoving) goto physics;
            Transform playerVisualTransform = playerRenderer.GetPlayerVisualTrasnform;
            if (InputDirection.sqrMagnitude > 0)
            {
                Quaternion visLookDirResult = Quaternion.LookRotation(InputDirection, Vector3.up);
                float angle = Vector3.Angle(InputDirection, playerVisualTransform.forward);
                const float angleMultiplier = 20;
                float maxDegreesDelta = Time.deltaTime * angle * angleMultiplier;
                visLookDirResult = Quaternion.RotateTowards(playerVisualTransform.rotation, visLookDirResult, maxDegreesDelta);
                playerVisualTransform.rotation = visLookDirResult;
            }
        physics:
            Vector3 inp = !AllowInputMoving ? Vector3.zero : InputDirection;
            float speed = 10 * SpeedMultiplier;
            Vector3 addition = RollForce + velocity;
            Vector3 result = inp * speed + addition;
            result.y = controller.linearVelocity.y;
            controller.linearVelocity = result;

        }
        public void AddForceLocaly(Vector3 force, ForceMode forceMode= ForceMode.Force, float multiplier = 1)
        {
            Transform visulTrnasform = playerRenderer.GetPlayerVisualTrasnform;
            Vector3 result = visulTrnasform.TransformVector(force) * multiplier;
            Debug.DrawRay(Vector3.zero, force, Color.red, 5);
            Debug.DrawRay(Vector3.zero, result, Color.yellow, 6);
            velocity = Vector3.zero;
            velocity += result;
        }

        public void Dash(Vector3 dir, float force)
        {
            //Vector3 normalizedDir = dir.normalized;

            controller.linearVelocity = Vector3.zero;

            float velPower = controller.linearVelocity.magnitude;
            Vector3 movement = dir * (force);

            Vector3 destination = transform.position + movement;

            cine.Damping = new Vector3(0.1f, 0.1f, 0.1f);

            dashPar.gameObject.SetActive(true);

            transform.DOMove(destination, 0.1f).SetEase(Ease.Flash).OnComplete(DashEnd);

            //controller.AddForce(dir * force, ForceMode.Impulse);
            //Vector3 startPos = ;
            //Debug.DrawRay(startPos, dir * force, Color.red, 5);
            //Vector3 result = ;
        }

        private void DashEnd()
        {
            cine.Damping = new Vector3(1, 1, 1);

            dashPar.SetPosition(0, transform.position);
            dashPar.SetPosition(1, transform.position);

            dashPar.gameObject.SetActive(false);
        }
        //public void Dash(Vector3 dashDirection, int force, Action callback = null)
        //{
        //    StopAllCoroutines();
        //    StartCoroutine(DashCor());
        //    IEnumerator DashCor()
        //    {
        //        rollStamina -= rollcost;
        //        AllowInputMoving = false;
        //        RollForce = dashDirection * force;
        //
        //        Vector3 startVelocitiy = RollForce;
        //
        //        float stepOffset = 0.2f;// controller.stepOffset;
        //        Vector3 startPos = transform.position + new Vector3(0, stepOffset);
        //        bool result = Physics.Raycast(startPos, dashDirection, out RaycastHit hit, force);
        //        Debug.DrawRay(startPos, dashDirection * force, Color.red, 5);
        //        Debug.DrawRay(startPos, Vector3.up * 5, Color.red, 5);
        //        if (result)
        //            Debug.DrawRay(hit.point, Vector3.up, Color.blue, 5);
        //        float resultDistance = result ? hit.distance : force;
        //
        //        float originalDistance = force;
        //        const float dashMultiplier = 0.3f; /* (resultDis / origianlDis) is approximately 1
        //                                            * so.. = 1 * dashMultiplier; */
        //        float timer = 0;
        //        float endTime = resultDistance / originalDistance * dashMultiplier;
        //        print(endTime);
        //        Vector3 targetVector = Vector3.zero;
        //
        //        while (timer < endTime)
        //        {
        //            float curveValue = timer / endTime;
        //            float val = rollCurve.Evaluate(curveValue);
        //            RollForce = Vector3.Lerp(startVelocitiy, targetVector, val);
        //            timer += Time.deltaTime;
        //            yield return null;
        //        }
        //        RollForce = Vector3.zero;
        //        AllowInputMoving = true;
        //        callback?.Invoke();
        //    }
        //}
    }
}
