using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

namespace Swift_Blade
{
    [SelectionBase]
    public class PlayerMovement : MonoBehaviour, IEntityComponent, IEntityComponentStart
    {
        [Header("Movement Settings")]
        [SerializeField] private float defaultSpeed = 1;
        [SerializeField] private float onGroundYVal = -0.5f;
        [SerializeField] private float gravitiy = -9.81f;

        [SerializeField] private float gravitiyMultiplier = 1;
        [SerializeField] private AnimationCurve curveSlope;
        private float yVal;

        [Header("Collisin Settings")]
        private float bottomYOffset = 0.4f; //const 0.4f
        private ContactPoint? lowerstContactPoint;
        private ContactPoint? lowestContactPointBottom;

        [Header("Roll Settings")]
        [SerializeField] private AnimationCurve rollCurve;
        private float rollValue;

        private const float rollcost = 1f;
        private const float initialRollStamina = 1f;
        private float currentRollStamina;

        [Header("Angle Multiplier")]
        [SerializeField] private float angleMultiplier = 20f;

        [Header("Debug")]
        [SerializeField] private float db_speedMulti;
        [SerializeField] private Transform db_mousePosition;

        [Header("Reference")]
        private Rigidbody controller;
        private PlayerRenderer playerRenderer;
        private PlayerInput playerInput;

        [Header("Cache")]
        private readonly List<ContactPoint> contactPointList = new();
        
        public float GetMaxStamina => initialRollStamina;
        public float SpeedMultiplierDefault { get; set; } = 1;
        //public float SpeedMultiplierForward { get; set; } = 1;
        public Vector3 InputDirection { get; set; }
        public Vector3 AdditionalVelocity { get; set; }
        private Vector3 DashVelocity;
        public bool AllowInputMove { get; set; } = true;
        public bool AllowRotate { get; set; } = true;
        public bool CanRoll => currentRollStamina >= 1;
        public bool UseMouseLock { get; set; }

        public void EntityComponentAwake(Entity entity)
        {
            controller = GetComponent<Rigidbody>();
            currentRollStamina = initialRollStamina;
        }
        public void EntityComponentStart(Entity entity)
        {
            playerRenderer = entity.GetEntityComponent<PlayerRenderer>();
            playerInput = entity.GetEntityComponent<PlayerInput>();
        }
        private void Update()
        {
            currentRollStamina += Time.deltaTime;
            currentRollStamina = Mathf.Min(GetMaxStamina, currentRollStamina);
            //print(SpeedMultiplierDefault);
        }
        private void FixedUpdate()
        {
            ApplyMovement();
            rollValue = Mathf.MoveTowards(rollValue, 2, Time.fixedDeltaTime * 2.5f);
            //UI_DebugPlayer.DebugText(7, rollValue, "rollValue", DBG_UI_KEYS.Keys_PlayerAction);
            //UI_DebugPlayer.DebugText(8, currentRollStamina, "currentRoll", DBG_UI_KEYS.Keys_PlayerAction);
            AdditionalVelocity = Vector3.MoveTowards(AdditionalVelocity, Vector3.zero, Time.fixedDeltaTime * 10);

            if (lowestContactPointBottom.HasValue) yVal = onGroundYVal;
            else yVal += Time.fixedDeltaTime * gravitiy * gravitiyMultiplier;

            //UI_DebugPlayer.DebugText(5, lowestContactPointBottom.HasValue, "ONGROUND", DBG_UI_KEYS.Keys_PlayerMovement);
            //UI_DebugPlayer.DebugText(0, yVal, "yVal", DBG_UI_KEYS.Keys_PlayerMovement);

            lowerstContactPoint = null;
            lowestContactPointBottom = null;
        }
        private void ApplyMovement()
        {
            Vector3 input = Vector3.zero;
            if (AllowInputMove)
            {
                input = InputDirection;
                if (lowestContactPointBottom.HasValue)
                {
                    input = Vector3.ProjectOnPlane(input, lowestContactPointBottom.Value.normal);
                    input.Normalize();
                }
                if (AllowRotate)
                {
                    if (UseMouseLock)
                    {
                        Vector3 direction = playerInput.GetMouseDirection;
                        playerRenderer.LookAtDirectionSmooth(direction, angleMultiplier);
                    }
                    else playerRenderer.LookAtDirectionSmooth(InputDirection, angleMultiplier);
                }
            }

            float multiplier = SpeedMultiplierDefault;
            float wishSpeed = defaultSpeed * multiplier;

            Vector3 movementVector = controller.linearVelocity;
            movementVector.y = 0;
            //float currentSpeed = Vector3.Magnitude(movementVector);// change this to dot
            //float speed = wishSpeed - currentSpeed;
            float speed = wishSpeed;
            //UI_DebugPlayer.DebugText(1, speed, "speed", DBG_UI_KEYS.Keys_PlayerMovement);
            //UI_DebugPlayer.DebugText(1, AllowInputMove, "allowInputMove", DBG_UI_KEYS.Keys_PlayerAction);
            //UI_DebugPlayer.DebugText(2, wishSpeed, "wishSpeed", DBG_UI_KEYS.Keys_PlayerMovement);

            Vector3 addition = AdditionalVelocity + DashVelocity * rollCurve.Evaluate(rollValue);
            Vector3 result = input * speed + addition;
            result.y += yVal;
            controller.linearVelocity = result;
            //Debug.DrawRay(transform.position + Vector3.up * 0.5f, input, Color.cyan, 1);
        }
        public void AddForceFacingDirection(Vector3 force, float multiplier = 1)
        {
            Transform visulTrnasform = playerRenderer.GetPlayerVisualTrasnform;
            Vector3 result = visulTrnasform.TransformVector(force) * multiplier;
            AdditionalVelocity += result;
        }

        public void Dash(Vector3 dir, float force)
        {
            Vector3 result = dir * force;
            Debug.DrawRay(transform.position, result, Color.black, 1);
            rollValue = 0;
            currentRollStamina = 0;
            DashVelocity = result;
        }

        public void SetAdditionalVelocity(Vector3 velocitiy) => AdditionalVelocity = velocitiy;

        private void OnCollisionStay(Collision collision)
        {
            ContactPoint? GetLowestPoint()
            {
                ContactPoint? result = null;
                float lowestY = Mathf.Infinity;
                contactPointList.Clear();
                collision.GetContacts(contactPointList);
                foreach (var item in contactPointList)
                {
                    float itemY = item.point.y;
                    if (lowestY > itemY)
                    {
                        result = item;
                        lowestY = itemY;
                    }
                }
                //if (result.HasValue)
                //    Debug.DrawRay(result.Value.point, result.Value.normal, Color.yellow, 1f);
                return result;
            }
            ContactPoint? newContactPoint = GetLowestPoint();

            if (!newContactPoint.HasValue) return;

            if (!lowerstContactPoint.HasValue)
                lowerstContactPoint = newContactPoint;

            float lowestPointY = lowerstContactPoint.Value.point.y;
            float newPointY = newContactPoint.Value.point.y;

            if (lowestPointY >= newPointY)
            {
                lowerstContactPoint = newContactPoint;

                float bottomY = transform.position.y + bottomYOffset;
                if (newPointY < bottomY) lowestContactPointBottom = newContactPoint;
            }
        }

        
    }
}
