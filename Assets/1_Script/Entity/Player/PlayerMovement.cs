using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

namespace Swift_Blade
{
    [SelectionBase]
    public class PlayerMovement : MonoBehaviour, IEntityComponent
    {
        [Header("Movement Settings")]
        [SerializeField] private float defaultSpeed = 1;
        [SerializeField] private float onGroundYVal = -0.5f;
        [SerializeField] private float gravitiy = -9.81f;
        /// <summary>
        /// should I increase this when higher
        /// </summary>
        [SerializeField] private float gravitiyMultiplier = 1;
        [SerializeField] private AnimationCurve curveSlope;
        private float yVal;

        [Header("Collisin Settings")]
        [SerializeField] private float bottomYOffset = 0.4f; //const 0.4f
        private ContactPoint? lowerstContactPoint;
        private ContactPoint? lowestContactPointBottom;

        [Header("Roll Settings")]
        [SerializeField] private AnimationCurve rollCurve; // curve length should be 1.
        [SerializeField] private float debug_stmod;

        [Header("DashSetting")]
        [SerializeField] private CinemachinePositionComposer cine;
        [SerializeField] private LayerMask whatIsObstacle;
        [SerializeField] private CapsuleCollider capCol;

        private const float rollcost = 1f;
        private const float initialRollStamina = 3f;
        private float currentRollStamina;

        [Header("Angle Multiplier")]
        [SerializeField] private float angleMultiplier = 20f;

        [Header("Debug")]
        [SerializeField] private float db_speedMulti;
        [SerializeField] private Transform db_closestEnemyTransform;

        [Header("Reference")]
        private Rigidbody controller;
        private PlayerRenderer playerRenderer;

        [Header("Cache")]
        private readonly List<ContactPoint> contactPointList = new();

        public float GetMaxStamina => initialRollStamina + debug_stmod;
        public float SpeedMultiplierDefault { get; set; } = 1;
        public float SpeedMultiplierForward { get; set; } = 1;
        public Vector3 InputDirection { get; set; }
        public Vector3 AdditionalVector { get; set; }
        //public Vector3 RollForce { get; private set; }
        public bool AllowInputMoving { get; set; } = true;
        public bool LockOnEnemy { get; set; } = false;

        public void EntityComponentAwake(Entity entity)
        {
            playerRenderer = entity.GetEntityComponent<PlayerRenderer>();
            controller = GetComponent<Rigidbody>();
            currentRollStamina = initialRollStamina;
        }
        private void Update()
        {
            currentRollStamina += Time.deltaTime;
            currentRollStamina = Mathf.Min(GetMaxStamina, currentRollStamina);
            if (Input.GetKeyDown(KeyCode.V))
            {
                AddForceLocaly(Vector3.forward);
                //controller.linearVelocity = Vector3.zero;
                //AddForceLocaly(Vector3.forward, db_speedMulti);
            }
            //Debug.DrawRay(transform.position, Vector3.up * 10, Color.yellow);
            //if (lowerstContactPoint.HasValue)
            //    Debug.DrawRay(lowerstContactPoint.Value.point, Vector3.right, Color.yellow);
        }
        private void FixedUpdate()
        {
            AdditionalVector = Vector3.MoveTowards(AdditionalVector, Vector3.zero, Time.fixedDeltaTime * 10);
            ApplyMovement();
            UI_DebugPlayer.Instance.DebugText(5, lowestContactPointBottom.HasValue, "ONGROUND", DBG_UI_KEYS.Keys_PlayerMovement);
            if (lowestContactPointBottom.HasValue) yVal = onGroundYVal;
            else yVal += Time.fixedDeltaTime * gravitiy * gravitiyMultiplier;
            UI_DebugPlayer.Instance.DebugText(0, yVal, "yVal", DBG_UI_KEYS.Keys_PlayerMovement);
            lowerstContactPoint = null;
            lowestContactPointBottom = null;
        }
        private void ApplyMovement()
        {
            Vector3 input = Vector3.zero;
            if (AllowInputMoving)
            {
                input = InputDirection;
                if (LockOnEnemy)
                {
                    Vector3 targetVector = GetClosestEnemy();
                    playerRenderer.LookTarget(targetVector);
                }
                else playerRenderer.LookTargetSmooth(InputDirection, angleMultiplier);

                if (lowestContactPointBottom.HasValue)
                {
                    //controller.useGravity = false;
                    input = Vector3.ProjectOnPlane(InputDirection, lowerstContactPoint.Value.normal);// this is causing physics error on 90 deg angle normal
                    input.Normalize();
                }
                //else controller.useGravity = true;
            }

            float multiplier = SpeedMultiplierForward * SpeedMultiplierDefault;
            float wishSpeed = defaultSpeed * multiplier;

            Vector3 movementVector = controller.linearVelocity;
            movementVector.y = 0;
            //float currentSpeed = Vector3.Magnitude(movementVector);// change this to dot
            //float speed = wishSpeed - currentSpeed;
            float speed = wishSpeed;
            UI_DebugPlayer.Instance.DebugText(1, speed, "speed", DBG_UI_KEYS.Keys_PlayerMovement);
            UI_DebugPlayer.Instance.DebugText(2, wishSpeed, "wishSpeed", DBG_UI_KEYS.Keys_PlayerMovement);
            //UI_DebugPlayer.Instance.DebugText(3, currentSpeed, "curSpeed", DBG_UI_KEYS.Keys_PlayerMovement);
            //if (speed < 0) return;

            Vector3 addition = AdditionalVector;
            Vector3 result = input * speed + addition;
            result.y += yVal;
            controller.linearVelocity = result;
            Debug.DrawRay(transform.position + Vector3.up * 0.5f, input, Color.cyan, 1);
        }

        private Vector3 GetClosestEnemy()
        {
            Vector3 result = db_closestEnemyTransform == null ? Vector3.zero : db_closestEnemyTransform.position;
            return result;
        }

        public void AddForceLocaly(Vector3 force, float multiplier = 1, ForceMode forceMode = ForceMode.VelocityChange)
        {
            Transform visulTrnasform = playerRenderer.GetPlayerVisualTrasnform;
            Vector3 result = visulTrnasform.TransformVector(force) * multiplier;
            Debug.DrawRay(Vector3.zero, force, Color.red, 5);
            Debug.DrawRay(Vector3.zero, result, Color.yellow, 6);
            controller.linearVelocity = Vector3.zero;
            AdditionalVector += result;
        }

        public void Dash(Vector3 dir, float force)
        {
            if (dir == Vector3.zero) return;

            AllowInputMoving = false;

            controller.linearVelocity = Vector3.zero;

            float velPower = controller.linearVelocity.magnitude;

            Vector3 movement = dir * (force);

            Vector3 destination = (transform.position) + movement;

            cine.Damping = new Vector3(0.1f, 0.1f, 0.1f);

            float hitDist = 1;
            if (Physics.Raycast(transform.position + capCol.center, dir, out RaycastHit hit, force, whatIsObstacle))
            {
                Debug.Log(hit.distance);
                hitDist = hit.distance;

                destination = hit.point + dir.normalized * 0.1f;
                destination += new Vector3((capCol.bounds.size.x * -(dir.x + 0.1f)) / 2, 0, (capCol.bounds.size.z * -(dir.z + 0.1f)) / 2);
                destination -= capCol.center;
            }
            if (hitDist < 1f)
            {
                AllowInputMoving = true;
                return;
            }

            Debug.Log(dir);

            transform.DOMove(destination, 0.1f).SetEase(Ease.Flash).OnComplete(DashEnd);
        }

        private void DashEnd()
        {
            cine.Damping = new Vector3(1, 1, 1);
            AllowInputMoving = true;
            //dashPar.SetPosition(0, transform.position);
            //dashPar.SetPosition(1, transform.position);
            //
            //dashPar.gameObject.SetActive(false);
        }


        private void SetGravitiy(bool value) => controller.useGravity = value;
        public void SetVelocitiy(Vector3 velocitiy) => controller.linearVelocity = velocitiy;

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
                if (result.HasValue)
                    Debug.DrawRay(result.Value.point, result.Value.normal, Color.yellow, 1f);
                return result;
            }
            ContactPoint? newContactPoint = GetLowestPoint();

            if (!newContactPoint.HasValue) return;

            if (!lowerstContactPoint.HasValue)
                lowerstContactPoint = newContactPoint;

            float lowestPointY = lowerstContactPoint.Value.point.y;
            float newPointY = newContactPoint.Value.point.y;
            //It doesn't make sense but i will just leave it for now (>=)
            if (lowestPointY >= newPointY)
            {
                lowerstContactPoint = newContactPoint;

                float bottomY = transform.position.y + bottomYOffset;
                if (newPointY < bottomY) lowestContactPointBottom = newContactPoint;
            }
        }
    }
}
