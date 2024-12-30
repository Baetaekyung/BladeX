using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AppUI.Core;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

namespace Swift_Blade
{
    [SelectionBase]
    public class PlayerMovement : MonoBehaviour, IEntityComponent
    {
        [Header("Movement Settings")]
        [SerializeField] private float defaultSpeed = 1;
        [SerializeField] private float onGroundYVal;
        [SerializeField] private float gravitiy = -9.81f;
        [SerializeField] private float gravitiyMultiplier = 1;

        [Header("Collisin Settings")]
        [SerializeField] private float bottomYOffset; //const
        private ContactPoint? lowerstContactPoint;
        private ContactPoint? lowestContactPointBottom;

        [Header("Roll Settings")]
        [SerializeField] private AnimationCurve rollCurve; // curve length should be 1.
        [SerializeField] private float debug_stmod;

        [Header("DashSetting")]
        [SerializeField] private CinemachinePositionComposer cine;
        [SerializeField] private LayerMask whatIsObstacle;

        private Vector3 des;
        [SerializeField] private CapsuleCollider capCol;
        [SerializeField] private Transform cubePrefab;
        //[SerializeField] private TrailRenderer dashPar;

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
        private Vector3 velocity;
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
                //controller.linearVelocity = Vector3.zero;
                AddForceLocaly(Vector3.forward, db_speedMulti);
            }
            UI_DebugPlayer.Instance.DebugText(3, lowerstContactPoint.HasValue ? lowerstContactPoint.Value.point : lowerstContactPoint.HasValue, "lowestContactPoint", DBG_UI_KEYS.Keys_PlayerMovement);
            UI_DebugPlayer.Instance.DebugText(4, lowestContactPointBottom.HasValue ? lowestContactPointBottom.Value.point : lowestContactPointBottom.HasValue, "bottomPoint", DBG_UI_KEYS.Keys_PlayerMovement);
            UI_DebugPlayer.Instance.DebugText(5, controller.useGravity, "gravity", DBG_UI_KEYS.Keys_PlayerMovement);
            //Debug.DrawRay(transform.position, Vector3.up * 10, Color.yellow);
            //if (lowerstContactPoint.HasValue)
            //    Debug.DrawRay(lowerstContactPoint.Value.point, Vector3.right, Color.yellow);
        }
        private void FixedUpdate()
        {
            velocity = Vector3.MoveTowards(velocity, Vector3.zero, Time.fixedDeltaTime * 10);
            AdditionalVector = Vector3.MoveTowards(AdditionalVector, Vector3.zero, Time.fixedDeltaTime * 10);
            ApplyMovement();
        }
        private void ApplyMovement()
        {
            Vector3 input = Vector3.zero;
            //stop force
            //Vector3 oppositeVelocitiy = -controller.linearVelocity * 0.2f;
            //oppositeVelocitiy.y = 0;
            //if (true || input.sqrMagnitude < 0.05f)//always true 
            //{
            //    controller.AddForce(oppositeVelocitiy, ForceMode.VelocityChange);
            //}
            if (AllowInputMoving)
            {
                input = InputDirection;
                bool lockTarget = LockOnEnemy;
                if (lockTarget)
                {
                    Vector3 targetVector = GetClosestEnemy();
                    playerRenderer.LookTarget(targetVector);
                }
                else playerRenderer.LookTargetSmooth(InputDirection, angleMultiplier);

                if (lowestContactPointBottom != null)
                {
                    controller.useGravity = false;
                    input = Vector3.ProjectOnPlane(InputDirection, lowerstContactPoint.Value.normal);// this is causing physics error on 90 deg angle normal
                    input.Normalize();
                }
                else controller.useGravity = true;
            }

            //if I don't use it below i can unassign it here
            lowerstContactPoint = null;
            lowestContactPointBottom = null;

            float multiplier = SpeedMultiplierForward * SpeedMultiplierDefault;
            float wishSpeed = defaultSpeed * multiplier;
            UI_DebugPlayer.Instance.DebugText(0, wishSpeed, "wishSpeed", DBG_UI_KEYS.Keys_PlayerMovement);
            UI_DebugPlayer.Instance.DebugText(2, multiplier, "multiplier", DBG_UI_KEYS.Keys_PlayerMovement);

            float currentSpeed = Vector3.Magnitude(controller.linearVelocity);// change this to dot
            float speed = wishSpeed - currentSpeed;
            UI_DebugPlayer.Instance.DebugText(1, speed, "speed", DBG_UI_KEYS.Keys_PlayerMovement);
            if (speed < 0) return;

            Vector3 addition = velocity + AdditionalVector;
            Vector3 result = input * speed + addition;
            controller.linearVelocity = input * 5;
            UI_DebugPlayer.Instance.DebugText(6, input, "input", DBG_UI_KEYS.Keys_PlayerMovement);
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
            controller.AddForce(result, forceMode);
        }

        public void Dash(Vector3 dir, float force)
        {
            if (dir == Vector3.zero) return;
            //Vector3 normalizedDir = dir.normalized;

            controller.linearVelocity = Vector3.zero;

            float velPower = controller.linearVelocity.magnitude;
            Vector3 movement = dir * (force);

            Vector3 destination = (transform.position) + movement;

            cine.Damping = new Vector3(0.1f, 0.1f, 0.1f);

            //dashPar.gameObject.SetActive(true);

            if(Physics.Raycast(transform.position + capCol.center, dir, out RaycastHit hit, force, whatIsObstacle))
            {
                //Debug.Log(hit.distance);

                destination = hit.point + dir.normalized * 0.1f;
                destination += new Vector3(capCol.bounds.size.x * -(dir.x), 0, capCol.bounds.size.z * -(dir.z));
                destination -= capCol.center;
                //Instantiate(cubePrefab, destination, Quaternion.identity);
            }
            
            Debug.Log(dir);
            //Vector3 boxSize = ReCalculate(CalculateHalfExtents(capCol), dir, force);
            //Collider[] col = Physics.OverlapBox(transform.position, boxSize, Quaternion.identity, whatIsObstacle);
            //
            //if (col.Length > 0)
            //{
            //    destination = col[0].bounds.center;
            //    Debug.Log(destination);
            //}

            //destination;
            transform.DOMove(destination, 0.1f).SetEase(Ease.Flash).OnComplete(DashEnd);

            //controller.AddForce(dir * force, ForceMode.Impulse);
            //Vector3 startPos = ;
            //Debug.DrawRay(startPos, dir * force, Color.red, 5);
            //Vector3 result = ;
        }

        private void DashEnd()
        {
            cine.Damping = new Vector3(1, 1, 1);

            //dashPar.SetPosition(0, transform.position);
            //dashPar.SetPosition(1, transform.position);
            //
            //dashPar.gameObject.SetActive(false);
        }

        private Vector3 CalculateHalfExtents(CapsuleCollider capsule)
        {
            // 기본 반지름과 높이 계산
            float radius = capsule.radius;
            float height = capsule.height / 2f; // 반 높이 (캡슐 중심에서 양쪽으로 확장)

            // 캡슐 방향에 따라 크기 결정
            switch (capsule.direction)
            {
                case 0: // X축 방향
                    return new Vector3(height, radius, radius);
                case 1: // Y축 방향
                    return new Vector3(radius, height, radius);
                case 2: // Z축 방향
                    return new Vector3(radius, radius, height);
                default:
                    return Vector3.zero;
            }
        }

        private Vector3 ReCalculate(Vector3 boxSize, Vector3 dir, float force)
        {
            Vector3 halfExtents = boxSize / 2 + new Vector3(
            Mathf.Abs(dir.x) * (force / 2),
            Mathf.Abs(dir.y) * (force / 2),
                Mathf.Abs(dir.z) * (force / 2)
            );

            return halfExtents;
        }

        void DebugDrawBox(Vector3 center, Vector3 size, Color color)
        {
            Gizmos.color = color;
            Gizmos.DrawWireCube(center, size);
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
                    Debug.DrawRay(result.Value.point, new Vector3(0f, 0.25f, 1f), Color.yellow);
                return result;
            }
            ContactPoint? newContactPoint = GetLowestPoint();

            if (!newContactPoint.HasValue) return;

            if (!lowerstContactPoint.HasValue)
                lowerstContactPoint = newContactPoint;

            float lowestPointY = lowerstContactPoint.Value.point.y;
            float newPointY = newContactPoint.Value.point.y;
            //It doesn't make sense but i will just leave for now (>=)
            if (lowestPointY >= newPointY)
            {
                lowerstContactPoint = newContactPoint;

                float bottomY = transform.position.y + bottomYOffset;
                //Debug.DrawRay(transform.position + new Vector3(0, bottomY), Vector3.right, Color.red, 2);
                if (newPointY < bottomY) lowestContactPointBottom = newContactPoint;
            }
        }
    }
}
