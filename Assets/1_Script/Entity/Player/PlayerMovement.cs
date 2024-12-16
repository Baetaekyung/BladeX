using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swift_Blade
{
    public class PlayerMovement : PlayerComponentBase, IEntityComponentRequireInit
    {
        [Header("Movement Settings")]
        [SerializeField] private float defaultSpeed = 1;
        [SerializeField] private float onGroundYVal;
        [SerializeField] private float gravitiy = -9.81f;
        [SerializeField] private float gravitiyMultiplier = 1;

        [Header("Angle Multiplier")]
        [SerializeField] private float angleMultiplier = 20f;
        private Vector3 velocity;

        [Header("Roll Settings")]
        [SerializeField] private AnimationCurve rollCurve; // curve length should be 1.
        [SerializeField] private float debug_stmod;

        private const float rollcost = 1f;
        private const float initialRollStamina = 3f;
        private float rollStamina;

        private Rigidbody controller;
        private PlayerRenderer playerRenderer;

        public float SpeedMultiplier { get; set; } = 1;
        public float GetMaxStamina => initialRollStamina + debug_stmod;

        public Vector3 InputDirection { get; set; }
        public Vector3 RollForce { get; private set; }
        public bool AllowInputMoving { get; set; } = true;
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
            velocity = Vector3.MoveTowards(velocity, Vector3.zero, Time.fixedDeltaTime * 10);
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
                float maxDegreesDelta = Time.deltaTime * angle * angleMultiplier;
                visLookDirResult = Quaternion.RotateTowards(playerVisualTransform.rotation, visLookDirResult, maxDegreesDelta);
                playerVisualTransform.rotation = visLookDirResult;
            }
        physics:
            Vector3 inp = !AllowInputMoving ? Vector3.zero : InputDirection;
            float speed = defaultSpeed * SpeedMultiplier;
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
            //Vector3 startPos = ;
            //Debug.DrawRay(startPos, dir * force, Color.red, 5);
            //Vector3 result = ;
            controller.linearVelocity = Vector3.zero;
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
