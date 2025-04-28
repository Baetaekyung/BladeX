using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Swift_Blade.Inputs
{
    public enum InputType
    {
        Movement_Forward, Movement_Left, Movement_Back, Movement_Right, Roll, Parry, Inventory, ChangeQuick, UseQuick, Attack1, Attack2
    }

    [MonoSingletonUsage(MonoSingletonFlags.DontDestroyOnLoad)]
    [DefaultExecutionOrder(-200)]
    public class InputManager : MonoSingleton<InputManager>
    {
        public static event Action RollEvent;
        public static event Action ParryEvent;
        public static event Action InventoryEvent;
        public static event Action ChangeQuickEvent;
        public static event Action UseQuickEvent;
        public static event Action Attack1Event;
        public static event Action Attack2Event;

        [SerializeField] private CustomInputSO _input;

        private Plane _plane;

        public Vector2 InputDirection => _input.Movement;
        public Vector3 InputDirectionVector3 => new Vector3(InputDirection.x, 0, InputDirection.y);
        public Vector2 MousePos => _input.MousePosition;
        public Vector3 MousePosWorld
        {
            get
            {
                _plane.SetNormalAndPosition(Vector3.up, Player.Instance.transform.position);

                Ray ray = Camera.main.ScreenPointToRay(MousePos);
                if (_plane.Raycast(ray, out float distance))
                {
                    Vector3 hitPoint = ray.GetPoint(distance);
                    return hitPoint;
                }

                return Vector3.zero;
            }
        }

        protected override void Awake()
        {
            base.Awake();

            _plane = new Plane();

            if (_input == null)
                Debug.LogWarning("[InputManager] InputSO is null.");

            CustomInputSO.RollEvent += HandleRoll;
            CustomInputSO.ParryEvent += HandleParry;
            CustomInputSO.InventoryEvent += HandleInventory;
            CustomInputSO.ChangeQuickEvent += HandleChangeQuick;
            CustomInputSO.UseQuickEvent += HandleUseQuick;
            CustomInputSO.Attack1Event += HandleAttack1;
            CustomInputSO.Attack2Event += HandleAttack2;
        }

        protected override void OnDestroy()
        {
            //_input.ResetInputs();
            //RollEvent = null;
            //ParryEvent = null;
            //InventoryEvent = null;
            //ChangeQuickEvent = null;
            //UseQuickEvent = null;
            //Attack1Event = null;
            //Attack2Event = null;
            base.OnDestroy();
        }

        public void Rebind(InputType type, bool mouseEnable = true)
        {
            _input.CustomInput.Player.Disable();

            InputAction inputAction = InputTypeToInputAction(type);
            InputActionRebindingExtensions.RebindingOperation operation = inputAction.PerformInteractiveRebinding();

            if ((int)type < 4)
                operation.WithTargetBinding((int)type + 1).Start();

            if (mouseEnable)
                operation.WithControlsExcluding("Mouse");

            operation.WithCancelingThrough("<keyboard>/escape")
                .OnComplete(op =>
                {
                    op.Dispose();
                    _input.CustomInput.Player.Enable();
                }).OnCancel(op =>
                {
                    op.Dispose();
                    _input.CustomInput.Player.Enable();
                }).Start();
        }

        public string GetCurrentKeyByType(InputType type)
        {
            InputAction inputAction = InputTypeToInputAction(type);

            if((int)type < 4)
                return inputAction.GetBindingDisplayString((int)type + 1);
                
            return inputAction.GetBindingDisplayString();
        }

        private InputAction InputTypeToInputAction(InputType type)
        {
            return type switch
            {
                InputType.Movement_Forward or InputType.Movement_Left or InputType.Movement_Back or InputType.Movement_Right => _input.CustomInput.Player.Movement,
                InputType.Roll => _input.CustomInput.Player.Roll,
                InputType.Parry => _input.CustomInput.Player.Parry,
                InputType.Inventory => _input.CustomInput.Player.Inventory,
                InputType.ChangeQuick => _input.CustomInput.Player.ChangeQuick,
                InputType.UseQuick => _input.CustomInput.Player.UseQuick,
                InputType.Attack1 => _input.CustomInput.Player.Attack1,
                InputType.Attack2 => _input.CustomInput.Player.Attack2,
                _ => default,
            };
        }

        #region Handle

        private void HandleRoll()
        {
            if (PopupManager.Instance.IsRemainPopup)
                return;

            RollEvent?.Invoke();
        }

        private void HandleParry()
        {
            if (PopupManager.Instance.IsRemainPopup)
                return;

            ParryEvent?.Invoke();
        }

        private void HandleInventory()
        {
            if (PopupManager.Instance.IsRemainPopup)
                return;

            InventoryEvent?.Invoke();
        }

        private void HandleChangeQuick()
        {
            if (PopupManager.Instance.IsRemainPopup)
                return;

            ChangeQuickEvent?.Invoke();
        }

        private void HandleUseQuick()
        {
            if (PopupManager.Instance.IsRemainPopup)
                return;

            UseQuickEvent?.Invoke();
        }

        private void HandleAttack1()
        {
            if (PopupManager.Instance.IsRemainPopup)
                return;

            Attack1Event?.Invoke();
        }

        private void HandleAttack2()
        {
            if (PopupManager.Instance.IsRemainPopup)
                return;

            Attack2Event?.Invoke();
        }

        #endregion
    }
}
