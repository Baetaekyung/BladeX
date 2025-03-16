using System;
using System.Reflection;
using UnityEngine;

namespace Swift_Blade
{
    public class PlayerInput : MonoBehaviour, IEntityComponent, IEntityComponentStart
    {
        public Vector3 GetInputDirectionRaw { get; private set; }
        public Vector3 GetInputDirection { get; private set; }
        public Vector3 GetInputDirectionRawRotated => CameraRotationOnlyY * GetInputDirectionRaw;
        public Vector3 GetInputDirectionRotated => CameraRotationOnlyY * GetInputDirection;
        public Vector3 GetMousePositionWorld { get; private set; }
        public Vector3 GetMouseDirection { get; private set; }
        public Vector2 GetRollDirection => new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0);
        /// <summary>
        /// im not sure if this is valid
        /// </summary>
        //public Vector3 InputDirectionRotatedNormalized => CameraRotation * (InputDirection.sqrMagnitude < 1 ? InputDirection: InputDirection.normalized);
        //private Quaternion CameraRotation => playerCamera.GetResultQuaternion;
        public Quaternion CameraRotationOnlyY => playerCamera.GetResultQuaternionOnlyY;

        private PlayerCamera playerCamera;
        private PlayerMovement playerMovement;
        public static event Action OnParryInput;
        private Plane plane;
        public void EntityComponentAwake(Entity entity)
        {
            plane = new();
        }
        public void EntityComponentStart(Entity entity)
        {
            var player = entity as Player;
            playerMovement = player.GetPlayerMovement;
            playerCamera = player.GetPlayerCamera;
        }
        private void Update()
        {
            plane.SetNormalAndPosition(Vector3.up, playerMovement.transform.position);
            void LegacyInput()
            {
                if (Input.GetKeyDown(KeyCode.C))
                    OnParryInput?.Invoke();
                GetInputDirectionRaw = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
                GetInputDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                Vector3 mousePos = Input.mousePosition;
                Ray mouseRay = playerCamera.GetPlayerCamera.ScreenPointToRay(mousePos);//GetStaticCamera.ScreenPointToRay(mousePos);
                Debug.DrawRay(mouseRay.origin, mouseRay.direction * 20, Color.red);
                if (plane.Raycast(mouseRay, out float distance))
                {
                    Vector3 hitPoint = mouseRay.GetPoint(distance);
                    GetMousePositionWorld = hitPoint;
                    GetMouseDirection = hitPoint - playerMovement.transform.position;
                }
            }
            LegacyInput();
        }

    }
}
