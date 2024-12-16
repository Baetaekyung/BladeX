using System;
using System.Reflection;
using UnityEngine;

namespace Swift_Blade
{
    public class PlayerInput : PlayerComponentBase
    {
        public Vector3 InputDirectionRaw { get; private set; }
        public Vector3 InputDirection { get; private set; }
        public static event Action OnParryInput;
        private void Update()
        {
            void LegacyInput()
            {
                if (Input.GetKeyDown(KeyCode.C))
                    OnParryInput?.Invoke();
                InputDirectionRaw = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
                InputDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            }
            LegacyInput();
        }
    }
}
