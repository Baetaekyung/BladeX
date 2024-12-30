using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

namespace Swift_Blade
{
    [SelectionBase]
    public class PlayerMovement : MonoBehaviour, IEntityComponentRequireInit
    {
        [Header("Movement Settings")]
        [SerializeField] private float defaultSpeed = 1;
        [SerializeField] private float onGroundYVal;
        [SerializeField] private float gravitiy = -9.81f;
        [SerializeField] private float gravitiyMultiplier = 1;

        [Header("Collisin Settings")]
        [SerializeField] private float bottomYOffset;
        private ContactPoint? lowerstContactPoint;
        private ContactPoint? lowestContactPointBottom;

        [Header("Roll Settings")]
        [SerializeField] private AnimationCurve rollCurve; // curve length should be 1.
        [SerializeField] private float debug_stmod;

        [Header("DashSetting")]
        [SerializeField] private CinemachinePositionComposer cine;
        [SerializeField] private LayerMask whatIsObstacle;
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
            UI_DebugPlayer.Instance.DebugText(3, lowerstContactPoint.HasValue ? lowerstContactPoint.Value.point : lowerstContactPoint.HasValue, "lowestContactPoint", DB_UI_KEYS.Keys_PlayerMovement);
            UI_DebugPlayer.Instance.DebugText(4, lowestContactPointBottom.HasValue ? lowestContactPointBottom.Value.point : lowestContactPointBottom.HasValue, "bottomPoint", DB_UI_KEYS.Keys_PlayerMovement);
            UI_DebugPlayer.Instance.DebugText(5, controller.useGravity, "gravity", DB_UI_KEYS.Keys_PlayerMovement);
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
                }
                else controller.useGravity = true;
            }

            //if i don't use it below i can unassign it here
            lowerstContactPoint = null;
            lowestContactPointBottom = null;

            float multiplier = SpeedMultiplierForward * SpeedMultiplierDefault;
            float wishSpeed = defaultSpeed * multiplier;
            UI_DebugPlayer.Instance.DebugText(0, wishSpeed, "wishSpeed", DB_UI_KEYS.Keys_PlayerMovement);
            UI_DebugPlayer.Instance.DebugText(2, multiplier, "multiplier", DB_UI_KEYS.Keys_PlayerMovement);

            float currentSpeed = Vector3.Magnitude(controller.linearVelocity);// change this to dot
            float speed = wishSpeed - currentSpeed;
            UI_DebugPlayer.Instance.DebugText(1, speed, "speed", DB_UI_KEYS.Keys_PlayerMovement);
            if (speed < 0) return;

            Vector3 addition = velocity + AdditionalVector;
            Vector3 result = input * speed + addition;
            controller.linearVelocity = input * 5;
            Debug.DrawRay(transform.position, input, Color.cyan, 1);
            //controller.AddForce(result, ForceMode.VelocityChange);
            //UI_DebugPlayer.Instance.GetList[4].text = controller.linearVelocity.ToString();
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

        private Vector3 des;
        public void Dash(Vector3 dir, float force)
        {
            //Vector3 normalizedDir = dir.normalized;

            controller.linearVelocity = Vector3.zero;

            float velPower = controller.linearVelocity.magnitude;
            Vector3 movement = dir * (force);

            Vector3 destination = transform.position + movement;

            cine.Damping = new Vector3(0.1f, 0.1f, 0.1f);

            //dashPar.gameObject.SetActive(true);

            if(Physics.Raycast(transform.position, destination, out RaycastHit hit, force, whatIsObstacle))
            {
                Debug.Log("����");
                Debug.Log(hit.distance);

                destination = hit.point - dir.normalized * 0.1f;
            }

            des = destination;
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
