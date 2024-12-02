using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swift_Blade
{
    public class PlayerMovement : PlayerComponentBase, IEntityComponentRequireInit
    {
        [Header("Roll Settings")]
        [SerializeField] private AnimationCurve rollCurve; // curve length should be 1.
        [SerializeField] private float debug_stmod;
        private const float rollcost = 1f;
        private const float initialRollStamina = 3f;
        private float rollStamina;
        public float GetCurrentRollStamina => rollStamina;
        public float GetMaxStamina => initialRollStamina + debug_stmod;

        public Vector3 InputDirection { get; set; }
        public Vector3 RollForce { get; private set; }
        public bool AllowInputMoving { get; set; } = true;
        private CharacterController controller;

        public float GetCurrentStamina => rollStamina;
        private PlayerRenderer playerRenderer;
        public void EntityComponentAwake(Entity entity)
        {
            playerRenderer = entity.GetEntityComponent<PlayerRenderer>();
            controller = GetComponent<CharacterController>();
            rollStamina = initialRollStamina;
        }
        private void Update()
        {
            rollStamina += Time.deltaTime;
            rollStamina = Mathf.Min(GetMaxStamina, rollStamina);
        }
        private void FixedUpdate()
        {
            ApplyMovement();
        }
        private void ApplyMovement()
        {
            if (!AllowInputMoving) return;

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
            float speed = 5 * Time.deltaTime;
            Vector3 result = InputDirection * speed + RollForce;
            controller.Move(result);
        }
        public void Dash(Vector3 dashDirection, int force, Action callback = null)
        {
            StopAllCoroutines();
            StartCoroutine(CO_DoABarrelRoll());
            IEnumerator CO_DoABarrelRoll()
            {
                rollStamina -= rollcost;
                AllowInputMoving = false;
                RollForce = dashDirection * force;

                Vector3 startVelocitiy = RollForce;
                float resultDistance = GetDistance();
                float originalDistance = force;
                const float dashMultiplier = 0.3f; /* (resultDis / origianlDis) is approximately 1
                                                    * so.. = 1 * dashMultiplier; */
                float endTime = resultDistance / originalDistance * dashMultiplier;
                float timer = 0;
                Vector3 targetVector = Vector3.zero;

                float GetDistance()
                {
                    //returns distance betwn objs when hit othewise return origianlForce(move distance)
                    float stepOffset = controller.stepOffset;
                    Vector3 startPos = transform.position + new Vector3(0, stepOffset);

                    bool result = Physics.Raycast(startPos, dashDirection, out RaycastHit hit, force);
                    Debug.DrawRay(startPos, dashDirection * force, Color.red, 5);
                    Debug.DrawRay(startPos, Vector3.up * 5, Color.red, 5);
                    if (result)
                        Debug.DrawRay(hit.point, Vector3.up, Color.blue, 5);

                    return result
                        ? hit.distance
                        : force;
                }
                void OnEnd()
                {
                    RollForce = Vector3.zero;
                    AllowInputMoving = true;
                    callback?.Invoke();
                }
                while (timer < endTime)
                {
                    float curveValue = timer / endTime;
                    float val = rollCurve.Evaluate(curveValue);
                    RollForce = Vector3.Lerp(startVelocitiy, targetVector, val);
                    timer += Time.deltaTime;
                    yield return null;
                }
                OnEnd();
            }
        }
    }
}
