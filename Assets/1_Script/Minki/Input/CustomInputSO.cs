using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Swift_Blade.Inputs
{
    [CreateAssetMenu(fileName = "CustomInput", menuName = "SO/CustomInput")]
    public class CustomInputSO : ScriptableObject, CustomInput.IPlayerActions
    {
        public event Action RollEvent;
        public event Action ParryEvent;
        public event Action InventoryEvent;
        public event Action ChangeQuickEvent;
        public event Action UseQuickEvent;
        public event Action Attack1Event;
        public event Action Attack2Event;

        public Vector2 Movement { get; private set; }
        public Vector2 MousePosition { get; private set; }

        private CustomInput _input;

        public CustomInput CustomInput => _input;

        private void OnEnable()
        {
            if(_input == null)
            {
                _input = new CustomInput();
                _input.Player.SetCallbacks(this);
            }
        }

        private void OnDisable()
        {
            _input.Player.SetCallbacks(null);
        }

        public void ResetInputs()
        {
            RollEvent = null;
            ParryEvent = null;
            InventoryEvent = null;
            ChangeQuickEvent = null;
            UseQuickEvent = null;
            Attack1Event = null;
            Attack2Event = null;
        }

        public void OnMovement(InputAction.CallbackContext context)
        {
            Movement = context.ReadValue<Vector2>();
        }

        public void OnMousePos(InputAction.CallbackContext context)
        {
            MousePosition = context.ReadValue<Vector2>();
        }

        public void OnRoll(InputAction.CallbackContext context)
        {
            if(context.performed) RollEvent?.Invoke();
        }

        public void OnParry(InputAction.CallbackContext context)
        {
            if(context.performed) ParryEvent?.Invoke();
        }

        public void OnInventory(InputAction.CallbackContext context)
        {
            if(context.performed) InventoryEvent?.Invoke();
        }

        public void OnChangeQuick(InputAction.CallbackContext context)
        {
            if(context.performed) ChangeQuickEvent?.Invoke();
        }

        public void OnUseQuick(InputAction.CallbackContext context)
        {
            if(context.performed) UseQuickEvent?.Invoke();
        }
        
        public void OnAttack1(InputAction.CallbackContext context)
        {
            if(context.performed) Attack1Event?.Invoke();
        }

        public void OnAttack2(InputAction.CallbackContext context)
        {
            if(context.performed) Attack2Event?.Invoke();
        }
    }
}
