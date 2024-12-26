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
        [SerializeField] private float defaultSpeed = 1;
        [SerializeField] private float onGroundYVal;
        [SerializeField] private float gravitiy = -9.81f;
        [SerializeField] private float gravitiyMultiplier = 1;
        private ContactPoint? lowerstContactPoint;

        [Header("Roll Settings")]
        [SerializeField] private AnimationCurve rollCurve; // curve length should be 1.
        [SerializeField] private float debug_stmod;

        [Header("DashSetting")]
        [SerializeField] private CinemachinePositionComposer cine;
        [SerializeField] private TrailRenderer dashPar;

        private const float rollcost = 1f;
        private const float initialRollStamina = 3f;
        private float rollStamina;

        [Header("Angle Multiplier")]
        [SerializeField] private float angleMultiplier = 20f;
        private Vector3 velocity;

        [Header("Debug")]
        [SerializeField] private float db_speedMulti;
        [SerializeField] private Transform db_closestEnemyTransform;

        [Header("Reference")]
        private Rigidbody controller;
        private PlayerRenderer playerRenderer;

        [Header("Cache")]
        private readonly List<ContactPoint> contactPointList = new();

        public float GetMaxStamina => initialRollStamina + debug_stmod;
        public float SpeedMultiplier { get; set; } = 1;
        public Vector3 InputDirection { get; set; }
        public Vector3 AdditionalVector { get; set; }
        //public Vector3 RollForce { get; private set; }
        public bool AllowInputMoving { get; set; } = true;
        public bool LockOnEnemy { get; set; } = false;

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
                AddForceLocaly(Vector3.forward, db_speedMulti);
            }
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
            Vector3 oppositeVelocitiy = -controller.linearVelocity * 0.2f;
            oppositeVelocitiy.y = 0;
            if (true || input.sqrMagnitude < 0.05f)//always true 
            {
                controller.AddForce(oppositeVelocitiy, ForceMode.VelocityChange);
            }

            if (AllowInputMoving)
            {
                input = InputDirection;
                bool lockTarget = LockOnEnemy;
                if (lockTarget)
                {
                    Vector3 targetVector = GetClosestEnemy();
                    playerRenderer.LookTarget(targetVector);
                }
                else
                    playerRenderer.LookTargetSmooth(InputDirection, angleMultiplier);

                if (lowerstContactPoint != null)
                {
                    controller.useGravity = false;
                    input = Vector3.ProjectOnPlane(InputDirection, lowerstContactPoint.Value.normal);
                }
                else
                {
                    controller.useGravity = true;
                }
                //Debug.DrawRay(transform.position, input, Color.red, 0.1f);
                //UI_DebugPlayer.Instance.GetList[4].text += input.magnitude;
            }
            float wishSpeed = defaultSpeed * SpeedMultiplier;
            UI_DebugPlayer.Instance.GetList[5].text = SpeedMultiplier.ToString();
            float currentSpeed = Vector3.Magnitude(controller.linearVelocity);
            float speed = wishSpeed - currentSpeed;
            if (speed < 0) goto end;
            Vector3 addition = velocity + AdditionalVector;
            Vector3 result = input * speed + addition;

            controller.AddForce(result, ForceMode.VelocityChange);
            UI_DebugPlayer.Instance.GetList[4].text = controller.linearVelocity.ToString();
        end:
            lowerstContactPoint = null;
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
            //Vector3 normalizedDir = dir.normalized;

            controller.linearVelocity = Vector3.zero;

            float velPower = controller.linearVelocity.magnitude;
            Vector3 movement = dir * (force);

            Vector3 destination = transform.position + movement;

            cine.Damping = new Vector3(0.1f, 0.1f, 0.1f);

            //dashPar.gameObject.SetActive(true);

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
            if (newContactPoint.HasValue)
            {
                if (!lowerstContactPoint.HasValue) lowerstContactPoint = newContactPoint;
                else if (lowerstContactPoint.Value.point.y >= newContactPoint.Value.point.y)
                    lowerstContactPoint = newContactPoint;
            }
        }
    }
}
