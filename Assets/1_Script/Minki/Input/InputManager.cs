using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Swift_Blade.Inputs
{
    public enum InputType
    {
        Movement_Forward, Movement_Left, Movement_Back, Movement_Right, Roll, Parry, Inventory, ChangeQuick, UseQuick, Attack1, Attack2
    }

    public class InputManager : MonoSingleton<InputManager>
    {
        public event Action RollEvent;
        public event Action ParryEvent;
        public event Action InventoryEvent;
        public event Action ChangeQuickEvent;
        public event Action UseQuickEvent;
        public event Action Attack1Event;
        public event Action Attack2Event;
        
        [SerializeField] private CustomInputSO _input;

        private Plane _plane;
        
        public Vector2 InputDirection => _input.Movement;
        public Vector2 MousePos => _input.MousePosition;
        public Vector2 MousePosWorld
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
                
                return Vector2.zero;
            }
        }

        protected override void Awake() {
            base.Awake();

            _plane = new Plane();

            if(_input == null)
                Debug.LogWarning("[InputManager] InputSO is null.");

            _input.RollEvent += HandleRoll;
            _input.ParryEvent += HandleParry;
            _input.InventoryEvent += HandleInventory;
            _input.ChangeQuickEvent += HandleChangeQuick;
            _input.UseQuickEvent += HandleUseQuick;
            _input.Attack1Event += HandleAttack1;
            _input.Attack2Event += HandleAttack2;
        }

        protected override void OnDestroy()
        {
            _input.ResetInputs();

            base.OnDestroy();
        }

        public void Rebind(InputType type, bool mouseEnable = true)
        {
            _input.CustomInput.Player.Disable();

            InputAction inputAction = null;

            switch(type) 
            {
                case InputType.Movement_Forward:
                case InputType.Movement_Left:
                case InputType.Movement_Back:
                case InputType.Movement_Right:
                    inputAction = _input.CustomInput.Player.Movement;
                    break;
                case InputType.Roll:
                    inputAction = _input.CustomInput.Player.Roll;
                    break;
                case InputType.Parry:
                    inputAction = _input.CustomInput.Player.Parry;
                    break;
                case InputType.Inventory:
                    inputAction = _input.CustomInput.Player.Inventory;
                    break;
                case InputType.ChangeQuick:
                    inputAction = _input.CustomInput.Player.ChangeQuick;
                    break;
                case InputType.UseQuick:
                    inputAction = _input.CustomInput.Player.UseQuick;
                    break;
                case InputType.Attack1:
                    inputAction = _input.CustomInput.Player.Attack1;
                    break;
                case InputType.Attack2:
                    inputAction = _input.CustomInput.Player.Attack2;
                    break;
            }

            InputActionRebindingExtensions.RebindingOperation operation = inputAction.PerformInteractiveRebinding();

            if((int)type < 4)
                operation.WithTargetBinding((int)type + 1).Start();

            if(mouseEnable)
                operation.WithControlsExcluding("Mouse");

            operation.WithCancelingThrough("<keyboard>/escape")
                .OnComplete(op => {
                    op.Dispose();
                    _input.CustomInput.Player.Enable();
                }).OnCancel(op => {
                    op.Dispose();
                    _input.CustomInput.Player.Enable();
                }).Start();
        }

        #region Handle

        private void HandleRoll() {
            if(PopupManager.Instance.IsRemainPopup)
                return;

            RollEvent?.Invoke();
        }

        private void HandleParry() {
            if(PopupManager.Instance.IsRemainPopup)
                return;

            ParryEvent?.Invoke();
        }

        private void HandleInventory() {
            if(PopupManager.Instance.IsRemainPopup)
                return;

            InventoryEvent?.Invoke();
        }

        private void HandleChangeQuick() {
            if(PopupManager.Instance.IsRemainPopup)
                return;

            ChangeQuickEvent?.Invoke();
        }

        private void HandleUseQuick() {
            if(PopupManager.Instance.IsRemainPopup)
                return;

            UseQuickEvent?.Invoke();
        }

        private void HandleAttack1() {
            if(PopupManager.Instance.IsRemainPopup)
                return;

            Attack1Event?.Invoke();
        }

        private void HandleAttack2() {
            if(PopupManager.Instance.IsRemainPopup)
                return;

            Attack2Event?.Invoke();
        }

        #endregion
    }
}
